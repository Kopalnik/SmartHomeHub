using Microsoft.AspNetCore.Mvc;
using SmartHomeHub.Services;

namespace SmartHomeHub.Controllers;

[ApiController]
[Route("api/devices")]
public class DevicesController : ControllerBase
{
    private readonly DeviceService _deviceService;

    public DevicesController(DeviceService deviceService)
    {
        _deviceService = deviceService;
    }

    [HttpGet]
    public async Task<IActionResult> GetDevices()
    {
        var devices = await _deviceService.GetDevicesAsync();

        return Ok(devices);
    }

    [HttpGet("summary")]
    public async Task<IActionResult> GetSummary()
    {
        var devices = await _deviceService.GetDevicesAsync();

        return Ok(new
        {
            Total = devices.Length,
            Online = devices.Count(d => d.IsOnline),
            Offline = devices.Count(d => !d.IsOnline)
        });
    }
}