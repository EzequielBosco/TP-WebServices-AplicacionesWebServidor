using Ejercicio_WebServices.DTOs;
using Ejercicio_WebServices.Models;

namespace Ejercicio_WebServices.Services
{
    public class VehiculoService : IVehiculoService
    {
        private static List<Vehiculo> _autos;

        public VehiculoService()
        {
            if (_autos == null)
            {
                _autos = new List<Vehiculo>();
                _autos.Add(new Vehiculo { Id = 1, Marca = "Chevrolet", Modelo = "Corsa", Color = "Rojo", Patente = "ABC123", Año = 2010, Kilometraje = 100000, TipoVehiculo = "Auto" });
                _autos.Add(new Vehiculo { Id = 2, Marca = "Ford", Modelo = "Fiesta", Color = "Azul", Patente = "DEF456", Año = 2015, Kilometraje = 50000, TipoVehiculo = "Auto" });
                _autos.Add(new Vehiculo { Id = 3, Marca = "Renault", Modelo = "Kangoo", Color = "Blanco", Patente = "GHI789", Año = 2018, Kilometraje = 20000, TipoVehiculo = "Utilitario" });
            }
        }

        public Task<IEnumerable<Vehiculo>> GetAllVehiculos()
        {
            var vehiculos = _autos.Select(v => v);
            return Task.FromResult(vehiculos);
        }

        public Task<IEnumerable<Vehiculo>> GetVehiculosByMarca(string marca)
        {
            var vehiculos = _autos.Where(v => v.Marca == marca);
            return Task.FromResult(vehiculos);
        }

        public Task<Vehiculo> GetVehiculoById(int id)
        {
            var vehiculo = _autos.FirstOrDefault(v => v.Id == id);
            return Task.FromResult(vehiculo);
        }

        public Task<Vehiculo> CreateVehiculo(VehiculoDTO vehiculo)
        {
            int ultimoId = _autos.Max(v => v.Id);
            int nuevoId = ultimoId + 1;

            var nuevoVehiculo = new Vehiculo
            {
                Id = nuevoId,
                Marca = vehiculo.Marca,
                Modelo = vehiculo.Modelo,
                Color = vehiculo.Color,
                Patente = vehiculo.Patente,
                Año = vehiculo.Año,
                Kilometraje = vehiculo.Kilometraje,
                TipoVehiculo = vehiculo.TipoVehiculo
            };
            _autos.Add(nuevoVehiculo);

            return Task.FromResult(nuevoVehiculo);
        }

        public Task<VehiculoDTO> UpdateVehiculo(int id, VehiculoDTO vehiculo)
        {
            var vehiculoToUpdate = _autos.LastOrDefault(v => v.Id == id);

            if (vehiculoToUpdate != null)
            { 
                vehiculoToUpdate.Marca = vehiculo.Marca;
                vehiculoToUpdate.Modelo = vehiculo.Modelo;
                vehiculoToUpdate.Color = vehiculo.Color;
                vehiculoToUpdate.Patente = vehiculo.Patente;
                vehiculoToUpdate.Año = vehiculo.Año;
                vehiculoToUpdate.Kilometraje = vehiculo.Kilometraje;
                vehiculoToUpdate.TipoVehiculo = vehiculo.TipoVehiculo;
            }

            var newVehiculoDTO = new VehiculoDTO
            {
                Marca = vehiculoToUpdate.Marca,
                Modelo = vehiculoToUpdate.Modelo,
                Color = vehiculoToUpdate.Color,
                Patente = vehiculoToUpdate.Patente,
                Año = vehiculoToUpdate.Año,
                Kilometraje = vehiculoToUpdate.Kilometraje,
                TipoVehiculo = vehiculoToUpdate.TipoVehiculo
            };

            return Task.FromResult(newVehiculoDTO);
        }

        public Task<bool> DeleteVehiculo(int id)
        {
            var vehiculoToDelete = _autos.LastOrDefault(v => v.Id == id);

            if (vehiculoToDelete != null)
            {
                _autos.Remove(vehiculoToDelete); 
                return Task.FromResult(true);
            }

            return Task.FromResult(false);
        }
    }
}