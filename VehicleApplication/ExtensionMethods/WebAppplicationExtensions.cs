using Microsoft.AspNetCore.SignalR;
using VehicleApplication.Hubs;
using VehicleApplication.Model.Interfaces;

namespace VehicleApplication.ExtensionMethods;

public static class WebAppplicationExtensions
{
    public static void UseVehicleAppServices<T>(this WebApplication app, string vehicleHubRoute) where T: IVehicle
    {
        app.UseCors(ServiceCollectionExtension.VehicleAppCorsPolicyName);
        app.MapHub<VehicleHub<T>>(vehicleHubRoute);
    }
}