using Microsoft.AspNetCore.SignalR;
using VehicleApplication.Model.Interfaces;
using VehicleApplication.Services.Interfaces;

namespace VehicleApplication.Hubs;

public class VehicleHub<T> : Hub<IVehicleObserver<T>> where T:IVehicle
{
    public VehicleHub(IVehicleService vehicleService)
    {
        _vehicleService = vehicleService;
    }
    public override Task OnConnectedAsync()
    {
        var cId = Context.ConnectionId;
        Console.WriteLine($"New connection detected: {cId}");
        return base.OnConnectedAsync();
    }
    
    public override Task OnDisconnectedAsync(Exception? exception)
    {
        var cId = Context.ConnectionId;
        Console.WriteLine($"Disconnecting {cId}");
        _vehicleService.RemoveConnection(cId);
        return base.OnDisconnectedAsync(exception);
    }

    public async Task Subscribe(string identity)
    {
        var cId = Context.ConnectionId;
        _vehicleService.AddConnection(connectionId:cId, identity:identity);
        Console.WriteLine($"Subscription requested from {identity}: connectionId:{Context.ConnectionId}");

        await Task.CompletedTask;
    }
    
    private readonly IVehicleService _vehicleService;
}