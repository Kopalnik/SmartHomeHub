using Microsoft.AspNetCore.Mvc;
using SmartHomeHub.Services;

namespace SmartHomeHub.Controllers;

/// <summary>
/// Provides REST API endpoints for retrieving smart home
/// devices and their status summary.
/// </summary>
[ApiController]
[Route("api/devices")]
public class DevicesController : ControllerBase
{
    private readonly DeviceService _deviceService;

    /// <summary>
    /// Initializes the controller with the device service.
    /// </summary>
    public DevicesController(DeviceService deviceService)
    {
        _deviceService = deviceService;
    }

    /// <summary>
    /// Retrieves all monitored devices and their current status.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetDevices()
    {
        var devices = await _deviceService.GetDevicesAsync();

        return Ok(devices);
    }

    /// <summary>
    /// Retrieves a summary of total, online, and offline devices.
    /// </summary>
    [HttpGet("summary")]
    public async Task<IActionResult> GetSummary()
    {
        var devices = await _deviceService.GetDevicesAsync();

        // Calculates device statistics for the dashboard.
        return Ok(new
        {
            Total = devices.Length,
            Online = devices.Count(d => d.IsOnline),
            Offline = devices.Count(d => !d.IsOnline)
        });
    }
}