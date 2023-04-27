using ApplicationX.Model;
using ApplicationX.Services;
using VehicleApplication.ExtensionMethods;
using VehicleApplication.Hubs;


var builder = WebApplication.CreateBuilder(args);

var allowedVehicleAppUrls = builder.Configuration.VerifyGetEnumerable<string[]>("AllowedVehicleAppUrls");
builder.Services.AddVehicleAppServices<Car, CarFactoryService>(allowedVehicleAppUrls);

var app = builder.Build();

var vehicleHubRoute = builder.Configuration.VerifyGet<string>("VehicleHubRoute");
app.UseVehicleAppServices<Car>(vehicleHubRoute);

app.Run();