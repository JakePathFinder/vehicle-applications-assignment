using VehicleApplication.Model;

namespace ApplicationY.Model;

public class MotorCycle : BaseVehicle
{
    public bool HasSidecar { get; set; }

    public override string ToString()
    {
        return $"{base.ToString()}, {nameof(HasSidecar)}: {HasSidecar}";
    }
}
