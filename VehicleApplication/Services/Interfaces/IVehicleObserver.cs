using VehicleApplication.Model;
using VehicleApplication.Model.Interfaces;

namespace VehicleApplication.Services.Interfaces;

public interface IVehicleObserver<in T> : IHostedService where T:IVehicle
{
    Task SubscribeAsync(string identity, CancellationToken cancellationToken);
    Task NewVehicleGenerated(T vehicle);
}