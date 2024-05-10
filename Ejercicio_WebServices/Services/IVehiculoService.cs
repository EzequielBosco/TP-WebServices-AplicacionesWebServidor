using Ejercicio_WebServices.DTOs;
using Ejercicio_WebServices.Models;

namespace Ejercicio_WebServices.Services
{
    public interface IVehiculoService
    {
        Task<IEnumerable<Vehiculo>> GetAllVehiculos();
        Task<IEnumerable<Vehiculo>> GetVehiculosByMarca(string marca);
        Task<Vehiculo> GetVehiculoById(int id);
        Task<Vehiculo> CreateVehiculo(VehiculoDTO vehiculo);
        Task<VehiculoDTO> UpdateVehiculo(int id, VehiculoDTO vehiculo);
        Task<bool> DeleteVehiculo(int id);
    }
}