using System.Collections.Concurrent;
using VehicleApplication.Model.Interfaces;
using VehicleApplication.Repositories.Interfaces;

namespace VehicleApplication.Repositories;

public class VehicleInMemRepo<T>: IVehicleRepo<T> where T:IVehicle
{
    public VehicleInMemRepo()
    {
        _vehicles = new ConcurrentDictionary<string, T>();
    }
    public bool AddVehicle(T vehicle)
    {
        return _vehicles.TryAdd(vehicle.PlateNumber, vehicle);
    }

    public T? GetVehicle(string plateNumber)
    {
        _vehicles.TryGetValue(plateNumber, out var value);
        return value;
    }

    private readonly ConcurrentDictionary<string, T> _vehicles;
}