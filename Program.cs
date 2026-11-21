using SmartHomeHub.Services;
using SmartHomeHub.Mcp;

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

builder.Services.AddMcpServer()
    .WithHttpTransport()
    .WithTools<SmartHomeTools>();

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

app.MapControllers();

app.MapMcp("/mcp");

app.Run();