using ApplicationX.Model;
using VehicleApplication.Services;

namespace ApplicationX.Services;

public class CarFactoryService : VehicleFactoryService<Car>
{
    public CarFactoryService(IConfiguration cfg) : base(cfg)
    {
    }
    public override Car GenerateNewVehicle()
    {
        var car = base.GenerateNewVehicle();
        car.IsFourWheelDrive = GenRandomBoolValue();
        return car;
    }
}