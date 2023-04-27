using ApplicationY.Model;
using ApplicationY.Services;
using VehicleApplication.ExtensionMethods;


var builder = WebApplication.CreateBuilder(args);

var allowedVehicleAppUrls = builder.Configuration.VerifyGetEnumerable<string[]>("AllowedVehicleAppUrls");
builder.Services.AddVehicleAppServices<MotorCycle, MotorcycleFactoryService>(allowedVehicleAppUrls);

var app = builder.Build();

var vehicleHubRoute = builder.Configuration.VerifyGet<string>("VehicleHubRoute");
app.UseVehicleAppServices<MotorCycle>(vehicleHubRoute);

app.Run();