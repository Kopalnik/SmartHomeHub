using System.ComponentModel;
using System.Text.Json;
using ModelContextProtocol.Server;
using SmartHomeHub.Services;

namespace SmartHomeHub.Mcp;

[McpServerToolType]
public class SmartHomeTools
{
    [McpServerTool]
    [Description("Get all smart home devices and their current status.")]
    public static async Task<string> GetDevices(
        DeviceService deviceService)
    {
        var devices = await deviceService.GetDevicesAsync();

        return JsonSerializer.Serialize(devices);
    }

    [McpServerTool]
    [Description("Get available actions for a specific smart home device.")]
    public static async Task<string> GetDeviceActions(
        DeviceService deviceService,
        [Description("Name of the device")] string deviceName)
    {
        var config = await deviceService.GetDeviceConfigAsync(deviceName);

        if (config == null)
        {
            return "Device not found.";
        }

        return JsonSerializer.Serialize(config.Actions);
    }
        [McpServerTool]
    [Description("Execute an available smart home device action.")]
    public static async Task<string> ExecuteDeviceAction(
        DeviceService deviceService,
        [Description("Name of the device")] string deviceName,
        [Description("Name of the action")] string actionName)
    {
        return await deviceService.ExecuteDeviceActionAsync(
            deviceName, actionName);
    }
}