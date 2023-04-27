using Microsoft.AspNetCore.SignalR.Client;
using VehicleApplication.ExtensionMethods;
using VehicleApplication.Hubs;
using VehicleApplication.Model.Interfaces;
using VehicleApplication.Services.Interfaces;

namespace VehicleApplication.Services;

public class VehicleObserver<T>: IVehicleObserver<T> where T:IVehicle
{
    public VehicleObserver(IConfiguration cfg)
    {
        _identity = cfg.VerifyGet<string>("Identity");
        _connectionRetryDelaySec = cfg.VerifyGet<int>("ConnectionRetryDelayMs");
        
        var subscriptionUrl = cfg.VerifyGet<string>("SubscriptionUrl");
        var vehicleHubRoute = cfg.VerifyGet<string>("VehicleHubRoute");
        _subscriptionFullRoute = $"{subscriptionUrl}{vehicleHubRoute}";
        _connection = new HubConnectionBuilder()
            .WithUrl(_subscriptionFullRoute)
            .Build();
    }

    public async Task SubscribeAsync(string identity, CancellationToken cancellationToken)
    {
        Console.WriteLine($"Subscribe as {identity}");
        await _connection.InvokeAsync(nameof(VehicleHub<T>.Subscribe), identity, cancellationToken);
    }
    
    public async Task NewVehicleGenerated(T vehicle)
    {
        Console.WriteLine($"Received New Vehicle: {vehicle}");
        await Task.CompletedTask;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        Task.Run(() => Connect(cancellationToken),cancellationToken);
        await Task.CompletedTask;
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await _connection.StopAsync(cancellationToken);
    }
    
    private readonly string _identity;
    private readonly string _subscriptionFullRoute;
    private readonly HubConnection _connection;
    private readonly int _connectionRetryDelaySec;
    
    private async Task Connect(CancellationToken cancellationToken)
    {
        _connection.On<T>(nameof(NewVehicleGenerated), NewVehicleGenerated);
        Console.WriteLine($"Attempting to connect to: {_subscriptionFullRoute}");
        while (_connection.State != HubConnectionState.Connected)
        {
            try
            {
                await _connection.StartAsync(cancellationToken);
                break;
            }
            catch (Exception)
            {
                Console.WriteLine($"Connection to {_subscriptionFullRoute} is not ready yet. Attempting retry.");
                await Task.Delay(_connectionRetryDelaySec, cancellationToken);
            }
        }
        
        
        Console.WriteLine($"Subscribing to {_subscriptionFullRoute}");
        await SubscribeAsync(_identity, cancellationToken);
    }

}