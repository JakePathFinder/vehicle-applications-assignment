using VehicleApplication.Model;

namespace ApplicationX.Model;

public class Car : BaseVehicle
{
    public bool IsFourWheelDrive { get; set; }

    public override string ToString()
    {
        return $"{base.ToString()}, {nameof(IsFourWheelDrive)}: {IsFourWheelDrive}";
    }
}
