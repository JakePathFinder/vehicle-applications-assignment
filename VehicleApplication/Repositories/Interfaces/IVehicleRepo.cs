using VehicleApplication.Model.Interfaces;

namespace VehicleApplication.Repositories.Interfaces;

public interface IVehicleRepo<T> where T:IVehicle
{
    bool AddVehicle(T vehicle);
    T? GetVehicle(string plateNumber);
}