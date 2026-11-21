using SmartHomeHub.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddHttpClient("Devices", client =>
{
    client.Timeout = TimeSpan.FromSeconds(5);
});

builder.Services.AddSingleton<DeviceService>(provider =>
{
    var factory = provider.GetRequiredService<IHttpClientFactory>();
    return new DeviceService(factory.CreateClient("Devices"));
});

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

app.MapControllers();

app.Run();
