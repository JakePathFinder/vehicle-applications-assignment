using VehicleApplication.ExtensionMethods;
using VehicleApplication.Model.Interfaces;
using VehicleApplication.Services.Interfaces;

namespace VehicleApplication.Services;

public class VehicleFactoryService<T> : IVehicleFactoryService<T> where T:IVehicle, new()
{
    public VehicleFactoryService(IConfiguration cfg)
    {
        AllowedTypes = cfg.VerifyGetEnumerable<IReadOnlyCollection<string>>("AllowedVehicleTypes");
        AllowedColors = cfg.VerifyGetEnumerable<IReadOnlyCollection<string>>("AllowedVehicleColors");
    }
    protected readonly Random Random = new();
    protected readonly IReadOnlyCollection<string> AllowedTypes;
    protected readonly IReadOnlyCollection<string> AllowedColors;
    
    public virtual T GenerateNewVehicle()
    {
        return new T
        {
            Color = GenRandomFrom(AllowedColors),
            Type = GenRandomFrom(AllowedTypes),
            Year = DateTime.Now.Year + Random.Next(-10, +10),
            PlateNumber = GenLicensePlate()
        };
    }
    
    protected string GenRandomFrom(IReadOnlyCollection<string> collection)
    {
        var idx = Random.Next(collection.Count);
        return collection.ElementAt(idx);
    }
    
    protected bool GenRandomBoolValue()
    {
        return Random.Next(0,2) == 0;
    }
    
    protected virtual string GenLicensePlate()
    {
        return $"{Random.Next(10, 100):D2}-{Random.Next(1000):D3}-{Random.Next(100):D2}";
    }

}