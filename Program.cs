using SmartHomeHub.Services;
using SmartHomeHub.Mcp;

// Creates the web application builder.
var builder = WebApplication.CreateBuilder(args);

// Registers REST API controllers.
builder.Services.AddControllers();

// Configures the HTTP client used for device communication.
builder.Services.AddHttpClient("Devices", client =>
{
    client.Timeout = TimeSpan.FromSeconds(5);
});

// Registers the device monitoring service as a singleton.
builder.Services.AddSingleton<DeviceService>(provider =>
{
    var factory = provider.GetRequiredService<IHttpClientFactory>();
    return new DeviceService(factory.CreateClient("Devices"));
});

// Registers the workflow management service.
builder.Services.AddSingleton<WorkflowService>();

// Registers the background workflow automation engine.
builder.Services.AddHostedService<WorkflowEngine>();

// Configures the MCP server with HTTP transport and smart home tools.
builder.Services.AddMcpServer()
    .WithHttpTransport()
    .WithTools<SmartHomeTools>();

// Builds the web application.
var app = builder.Build();

// Enables default and static files for the dashboard.
app.UseDefaultFiles();
app.UseStaticFiles();

// Maps REST API controller endpoints.
app.MapControllers();

// Exposes the MCP server endpoint.
app.MapMcp("/mcp");

// Starts the web application.
app.Run();