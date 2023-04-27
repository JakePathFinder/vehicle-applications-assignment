using VehicleApplication.Model.Interfaces;

namespace VehicleApplication.Model;

public abstract class BaseVehicle : IVehicle
{
    public string Name => $"{GetType().Name} {Type} {PlateNumber}";
    public string Color { get; init; }
    public string PlateNumber { get; init; }
    public string Type { get; init; }
    public int Year { get; init; }
    public DateTime GeneratedDateTimeUtc { get; init; } = DateTime.UtcNow;

    public override string ToString()
    {
        return $"{nameof(Type)}: {Type}, {nameof(PlateNumber)}: {PlateNumber}, {nameof(Color)}: {Color}, {nameof(Year)}: {Year}, {nameof(GeneratedDateTimeUtc)}: {GeneratedDateTimeUtc}";
    }
}