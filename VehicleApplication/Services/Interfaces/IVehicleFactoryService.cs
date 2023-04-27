using VehicleApplication.Model;
using VehicleApplication.Model.Interfaces;

namespace VehicleApplication.Services.Interfaces;

public interface IVehicleFactoryService<T> where T:IVehicle
{
    T GenerateNewVehicle();
}