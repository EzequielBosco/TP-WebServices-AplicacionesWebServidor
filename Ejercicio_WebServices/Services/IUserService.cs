using Ejercicio_WebServices.DTOs;
using Ejercicio_WebServices.Models;

namespace Ejercicio_WebServices.Services
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAllUsers();
        Task<User> GetUser(string nombreUsuario, string contraseña);
    }
}
