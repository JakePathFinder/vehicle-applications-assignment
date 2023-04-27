using VehicleApplication.Model.Interfaces;
using VehicleApplication.Repositories;
using VehicleApplication.Repositories.Interfaces;
using VehicleApplication.Services;
using VehicleApplication.Services.Interfaces;

namespace VehicleApplication.ExtensionMethods;

public static class ServiceCollectionExtension
{
    
    public const string VehicleAppCorsPolicyName = "VehicleAppCorsPolicy";
    public static void AddVehicleAppServices<TVehicle, TVehicleFactory>
        (this IServiceCollection services, string[] allowedVehicleAppUrls) 
        where TVehicle:IVehicle, new() where TVehicleFactory:class,IVehicleFactoryService<TVehicle>
    {
        services.AddSignalR(options =>
        {
            options.EnableDetailedErrors = true;
        });
        
        services.AddCors(options =>
        {
            options.AddPolicy(VehicleAppCorsPolicyName,corsBuilder => 
                corsBuilder.WithOrigins(allowedVehicleAppUrls)
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials()
            );
        });
        
        services.AddSingleton<IVehicleFactoryService<TVehicle>,TVehicleFactory>();
        services.AddSingleton<IVehicleFactoryService<TVehicle>, VehicleFactoryService<TVehicle>>();
        
        services.AddSingleton<IVehicleService, VehicleService<TVehicle>>();
        services.AddHostedService(provider => provider.GetRequiredService<IVehicleService>());

        services.AddSingleton<IVehicleObserver<TVehicle>,VehicleObserver<TVehicle>>();
        services.AddHostedService(provider => provider.GetRequiredService<IVehicleObserver<TVehicle>>());
        
        services.AddSingleton<IVehicleFactoryService<TVehicle>,TVehicleFactory>();

        services.AddSingleton<IVehicleRepo<TVehicle>, VehicleInMemRepo<TVehicle>>();
        
        
    }
}