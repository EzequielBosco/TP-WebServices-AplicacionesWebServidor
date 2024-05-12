using System;
using Ejercicio_WebServices.DTOs;
using Ejercicio_WebServices.Models;
using Ejercicio_WebServices.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ejercicio_WebServices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehiculoController : ControllerBase
    {
        private readonly IVehiculoService _vehiculoService;
        private readonly ILogger<VehiculoController> _logger;

        public VehiculoController(IVehiculoService vehiculoService, ILogger<VehiculoController> logger)
        {
            _vehiculoService = vehiculoService;
            _logger = logger;
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<Vehiculo>>> GetAllVehiculos()
        {
            try
            {
                _logger.LogInformation("Se invocó el EndPoint GetAll");
                var vehiculos = await _vehiculoService.GetAllVehiculos();
                return Ok(vehiculos);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrió un error al obtener los vehículos: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocurrió un error al obtener los vehículos");
            }
        }

        [HttpGet("GetByMarca/{marca}")]
        public async Task<ActionResult<Vehiculo>> GetVehiculosByMarca(string marca)
        {
            try
            {
                var vehiculos = await _vehiculoService.GetVehiculosByMarca(marca);
                if (vehiculos == null || !vehiculos.Any())
                {
                    return NotFound();
                }
                return Ok(vehiculos);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrió un error al obtener los vehículos por marca: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocurrió un error al obtener los vehículos por marca");
            }
        }

        [HttpGet("GetById/{id}")]
        public async Task<ActionResult<Vehiculo>> GetVehiculosByMarca(int id)
        {
            try
            {
                var vehiculo = await _vehiculoService.GetVehiculoById(id);
                if (vehiculo == null)
                {
                    return NotFound();
                }
                return Ok(vehiculo);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrió un error al obtener el vehículo por id: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocurrió un error al obtener el vehículo por id");
            }
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Vehiculo>> CreateVehiculo(VehiculoDTO vehiculo)
        {
            try
            {
                var newVehiculo = await _vehiculoService.CreateVehiculo(vehiculo);
                return newVehiculo;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrió un error al crear el vehículo: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocurrió un error al crear el vehículo");
            }
        }

        [Authorize]
        [HttpPut("Update/{id}")]
        public async Task<IActionResult> UpdateVehiculo(int id, VehiculoDTO vehiculo)
        {
            try
            {
                var vehiculoUpdate = await _vehiculoService.GetVehiculoById(id);
                _logger.LogInformation($"El vehiculo a editar es: {vehiculo}");
                if (vehiculoUpdate == null)
                {
                    return NotFound();
                }

                var newVehiculo = await _vehiculoService.UpdateVehiculo(id, vehiculo);
     
                return Ok(newVehiculo);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrió un error al editar el vehículo: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocurrió un error al editar el vehículo");
            }
        }

        [Authorize]
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteVehiculo(int id)
        {
            try
            {
                var deleted = await _vehiculoService.DeleteVehiculo(id);
                if (!deleted)
                {
                    return NotFound();
                }
                _logger.LogWarning($"Se eliminó un vehículo con id: {id}");

                return Ok(deleted);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrió un error al eliminar el vehículo: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocurrió un error al eliminar el vehículo");
            }
        }
    }
}