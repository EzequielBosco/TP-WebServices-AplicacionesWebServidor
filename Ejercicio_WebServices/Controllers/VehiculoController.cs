﻿using System;
using Ejercicio_WebServices.DTOs;
using Ejercicio_WebServices.Models;
using Ejercicio_WebServices.Services;
using Microsoft.AspNetCore.Mvc;

namespace Ejercicio_WebServices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehiculoController : ControllerBase
    {
        private readonly IVehiculoService _vehiculoService;

        public VehiculoController(IVehiculoService vehiculoService)
        {
            _vehiculoService = vehiculoService;
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<Vehiculo>>> GetAllVehiculos()
        {
            var vehiculos = await _vehiculoService.GetAllVehiculos();
            return Ok(vehiculos);
        }

        [HttpGet("GetByMarca/{marca}")]
        public async Task<ActionResult<Vehiculo>> GetVehiculosByMarca(string marca)
        {
            var vehiculos = await _vehiculoService.GetVehiculosByMarca(marca);
            if (vehiculos == null || !vehiculos.Any())
            {
                return NotFound();
            }
            return Ok(vehiculos);
        }

        [HttpGet("GetById/{id}")]
        public async Task<ActionResult<Vehiculo>> GetVehiculosByMarca(int id)
        {
            var vehiculo = await _vehiculoService.GetVehiculoById(id);
            if (vehiculo == null)
            {
                return NotFound();
            }
            return Ok(vehiculo);
        }

        [HttpPost]
        public async Task<ActionResult<Vehiculo>> CreateVehiculo(VehiculoDTO vehiculo)
        {
            var newVehiculo = await _vehiculoService.CreateVehiculo(vehiculo);
            return newVehiculo;
        }

        [HttpPut("Update/{id}")]
        public async Task<IActionResult> UpdateVehiculo(int id, VehiculoDTO vehiculo)
        {
            var vehiculoUpdate = await _vehiculoService.GetVehiculoById(id);
            if (vehiculoUpdate == null)
            {
                return NotFound();
            }
            
            var newVehiculo = await _vehiculoService.UpdateVehiculo(id, vehiculo);
     
            return Ok(newVehiculo);
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteVehiculo(int id)
        {
            var deleted = await _vehiculoService.DeleteVehiculo(id);
            if (!deleted)
            {
                return NotFound();
            }

            return Ok(deleted);
        }
    }
}