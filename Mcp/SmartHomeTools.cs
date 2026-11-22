using System.ComponentModel;
using System.Text.Json;
using ModelContextProtocol.Server;
using SmartHomeHub.Services;

namespace SmartHomeHub.Mcp;

/// <summary>
/// Provides MCP tools for monitoring smart home devices
/// and executing supported device actions.
/// </summary>
[McpServerToolType]
public class SmartHomeTools
{
    /// <summary>
    /// Retrieves all monitored devices and their current status.
    /// </summary>
    [McpServerTool]
    [Description("Get all smart home devices and their current status.")]
    public static async Task<string> GetDevices(
        DeviceService deviceService)
    {
        var devices = await deviceService.GetDevicesAsync();

        // Serializes device information into JSON format.
        return JsonSerializer.Serialize(devices);
    }

    /// <summary>
    /// Retrieves available actions for a specific device.
    /// </summary>
    [McpServerTool]
    [Description("Get available actions for a specific smart home device.")]
    public static async Task<string> GetDeviceActions(
        DeviceService deviceService,
        [Description("Name of the device")] string deviceName)
    {
        var config = await deviceService.GetDeviceConfigAsync(deviceName);

        // Returns an error message if the device cannot be found.
        if (config == null)
        {
            return "Device not found.";
        }

        // Serializes available device actions into JSON format.
        return JsonSerializer.Serialize(config.Actions);
    }

    /// <summary>
    /// Executes a supported action on the specified smart home device.
    /// </summary>
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