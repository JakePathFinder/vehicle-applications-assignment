using VehicleApplication.Model;

namespace VehicleApplication.Services.Interfaces;

public interface IVehicleService : IHostedService
{
    Task StartGeneration(CancellationToken cancellationToken);
    bool AddConnection(string connectionId, string identity);
    bool RemoveConnection(string connectionId);
}