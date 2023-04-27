using System.Collections.Concurrent;
using Microsoft.AspNetCore.SignalR;
using VehicleApplication.ExtensionMethods;
using VehicleApplication.Hubs;
using VehicleApplication.Model.Interfaces;
using VehicleApplication.Repositories.Interfaces;
using VehicleApplication.Services.Interfaces;

namespace VehicleApplication.Services;

public class VehicleService<T> : IVehicleService where T : IVehicle
{
    public VehicleService(IHubContext<VehicleHub<T>, IVehicleObserver<T>> vehicleHub,
        IVehicleRepo<T> vehicleRepo,
        IVehicleFactoryService<T> vehicleGenerator, IConfiguration cfg)
    {
        _generator = vehicleGenerator;
        _generationFreq = cfg.VerifyGet<int>("GenerationFreqMs");
        _connectionRetryDelaySec = cfg.VerifyGet<int>("ConnectionRetryDelayMs");
        _cts = new CancellationTokenSource();
        _connections = new ConcurrentDictionary<string, string>();
        _vehicleHub = vehicleHub;
        _repo = vehicleRepo;
        var requiredClients =
            cfg.VerifyGetEnumerable<IReadOnlyCollection<string>>("RequiredIdentitiesForGenerationStart");
        _remainingRequiredClients = new ConcurrentDictionary<string, string>(
            requiredClients.ToDictionary(x => x.Sanitize(), x=> x));
    }

    public async Task StartGeneration(CancellationToken cancellationToken)
    {
        await AwaitAllConnections(cancellationToken);
        while (!cancellationToken.IsCancellationRequested)
        {
            await Task.Delay(_generationFreq, cancellationToken);
            
            var vehicle = _generator.GenerateNewVehicle();
            _repo.AddVehicle(vehicle);
            
            Console.WriteLine($"Latest generated vehicle: {vehicle.Name} @ {vehicle.GeneratedDateTimeUtc}");
            await _vehicleHub.Clients.All.NewVehicleGenerated(vehicle);
        }
    }

    public bool AddConnection(string connectionId, string identity)
    {
        Console.WriteLine($"Adding {identity} with connectionId:{connectionId}");
        var sanitizedIdentity = identity.Sanitize();
        if (_remainingRequiredClients.ContainsKey(sanitizedIdentity))
        {
            _remainingRequiredClients.TryRemove(sanitizedIdentity, out _);
        }

        var added = _connections.TryAdd(connectionId, identity);
        return added;
    }

    public bool RemoveConnection(string connectionId)
    {
        var removed = _connections.TryRemove(connectionId, out var identity);
        Console.WriteLine($"Removing {identity} connectionId:{connectionId}");

        return removed;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        var linkedToken = CancellationTokenSource.CreateLinkedTokenSource(_cts.Token, cancellationToken).Token;

        Task.Run(() => StartGeneration(linkedToken), linkedToken);
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _cts.Cancel();
        Console.WriteLine("Stopping Generation");
        return Task.CompletedTask;
    }

    private readonly int _generationFreq;
    private readonly CancellationTokenSource _cts;
    private readonly IVehicleFactoryService<T> _generator;
    private readonly ConcurrentDictionary<string, string> _connections;
    private readonly IHubContext<VehicleHub<T>, IVehicleObserver<T>> _vehicleHub;
    private readonly ConcurrentDictionary<string, string> _remainingRequiredClients;
    private readonly IVehicleRepo<T> _repo;
    private readonly int _connectionRetryDelaySec;

    private async Task AwaitAllConnections(CancellationToken cancellationToken)
    {
        while (!_remainingRequiredClients.IsEmpty)
        {
            cancellationToken.ThrowIfCancellationRequested();
            Console.WriteLine($"Waiting for [{string.Join(", ",_remainingRequiredClients.Values)}] " +
                              $"Connected Users [{string.Join(", ",_connections.Values)}]");
            await Task.Delay(_connectionRetryDelaySec, cancellationToken);
        }

        Console.WriteLine("All required clients have connected");
    }
}
    
