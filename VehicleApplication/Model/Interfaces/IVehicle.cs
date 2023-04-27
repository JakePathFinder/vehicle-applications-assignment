namespace VehicleApplication.Model.Interfaces;


public interface IVehicle
{
    string Name { get; }
    string Color { get; init; }
    string PlateNumber { get; init; }
    string Type { get; init; }
    public int Year { get; init; }
    public DateTime GeneratedDateTimeUtc { get; init; }
}