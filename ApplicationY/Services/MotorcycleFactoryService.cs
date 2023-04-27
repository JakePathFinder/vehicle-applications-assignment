using ApplicationY.Model;
using VehicleApplication.Services;

namespace ApplicationY.Services;

public class MotorcycleFactoryService : VehicleFactoryService<MotorCycle>
{
    public MotorcycleFactoryService(IConfiguration cfg) : base(cfg)
    {
    }
    public override MotorCycle GenerateNewVehicle()
    {
        var motorCycle = base.GenerateNewVehicle();
        motorCycle.HasSidecar = GenRandomBoolValue();
        return motorCycle;
    }
}