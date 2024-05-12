using Ejercicio_WebServices.DTOs;
using Ejercicio_WebServices.Models;
using Ejercicio_WebServices.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Ejercicio_WebServices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        public IConfiguration _configuration;
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserService userService, IConfiguration configuration, ILogger<UserController> logger)
        {
            _userService = userService;
            _configuration = configuration;
            _logger = logger;
        }

        [Authorize]
        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<User>>> GetAllUsers()
        {
            try {
                var users = await _userService.GetAllUsers();
                if (users != null)
                {
                    return Ok(users);
                }

                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrió un error al obtener los usuarios: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "No se encuentra logueado" + ex.Message);
            }
        }

        [HttpPost("Login")]
        public async Task<ActionResult<User>> GetUser(UserLoginRequest user)
        {
            try
            {
                var userResponse = await _userService.GetUser(user.NombreUsuario, user.Contraseña);
                if (userResponse == null)
                {
                    return NotFound();
                }

                var rolClaimValue = userResponse.Rol.ToString() == "Admin" ? "Admin" : "User";

                var claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub,_configuration["Jwt:Subject"]),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                    new Claim("Rol", rolClaimValue),
                    new Claim("Nombre", userResponse.Nombre),
                    new Claim("NombreUsuario", userResponse.NombreUsuario),
                    new Claim("Correo", userResponse.Correo)
                };
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(
                    _configuration["Jwt:Issuer"],
                    _configuration["Jwt:Audience"],
                    claims,
                    expires: DateTime.UtcNow.AddMinutes(10),
                    signingCredentials: signIn);

                var claimsIdentity = new ClaimsIdentity(
                    claims,
                    CookieAuthenticationDefaults.AuthenticationScheme
                    );

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity));

                var userResponseDTO = new UserResponseDTO
                {
                    Nombre = userResponse.Nombre,
                    NombreUsuario = userResponse.NombreUsuario,
                    Correo = userResponse.Correo,
                    Rol = userResponse.Rol,
                    Token = new JwtSecurityTokenHandler().WriteToken(token)
                };

                return Ok(userResponseDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrió un error al obtener el usuario: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [Authorize]
        [HttpPost("Logout")]
        public async Task<IActionResult> Logout()
        {
            try
            {
                await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrió un error al cerrar sesión: {ex.Message}");
                return StatusCode(500, ex.Message);
            }
        }
    }
}
