using SmartHomeHub.Models;
using System.Collections.Concurrent;

namespace SmartHomeHub.Services;

/// <summary>
/// Provides communication with smart home devices,
/// monitors their status and executes supported actions.
/// </summary>
public class DeviceService
{
    private readonly HttpClient _httpClient;

    // Stores the last successful connection timestamp for each device.
    private readonly ConcurrentDictionary<string, DateTime> _lastSeen = new();

    // Defines the monitored smart home devices and their API URLs.
    private readonly (string Name, string Url)[] _devices =
    {
        ("Device1", "https://localhost:1234"),
        ("Device2", "https://localhost:1236")
    };

    /// <summary>
    /// Initializes the device service with an HTTP client.
    /// </summary>
    public DeviceService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    /// <summary>
    /// Retrieves the current status of all configured devices in parallel.
    /// </summary>
    public async Task<DeviceStatus[]> GetDevicesAsync()
    {
        return await Task.WhenAll(_devices.Select(GetDeviceStatusAsync));
    }

    /// <summary>
    /// Retrieves the status and available information of a specific device.
    /// </summary>
    private async Task<DeviceStatus> GetDeviceStatusAsync(
        (string Name, string Url) device)
    {
        var status = new DeviceStatus
        {
            Name = device.Name,
            Url = device.Url
        };

        try
        {
            // Retrieves device information from the API.
            status.Info = await _httpClient
                .GetFromJsonAsync<DeviceInfo>($"{device.Url}/info");

            status.IsOnline = status.Info != null;

            // Updates the timestamp when the device is online.
            if (status.IsOnline)
            {
                _lastSeen[device.Url] = DateTime.UtcNow;
            }
        }
        catch (HttpRequestException)
        {
            // Marks the device as offline when an HTTP request fails.
            status.IsOnline = false;
        }
        catch (OperationCanceledException)
        {
            // Marks the device as offline when the request is canceled.
            status.IsOnline = false;
        }

        // Retrieves the last known online timestamp.
        if (_lastSeen.TryGetValue(device.Url, out var lastSeen))
        {
            status.LastSeen = lastSeen;
        }

        return status;
    }

    /// <summary>
    /// Retrieves the configuration and available actions of a selected device.
    /// </summary>
    public async Task<DeviceConfig?> GetDeviceConfigAsync(string deviceName)
    {
        // Finds the device by its name, ignoring letter case.
        var device = _devices.FirstOrDefault(d =>
            d.Name.Equals(deviceName, StringComparison.OrdinalIgnoreCase));

        if (string.IsNullOrEmpty(device.Url))
        {
            return null;
        }

        return await _httpClient.GetFromJsonAsync<DeviceConfig>(
            $"{device.Url}/config");
    }

    /// <summary>
    /// Executes a supported action on a selected smart home device.
    /// </summary>
    public async Task<string> ExecuteDeviceActionAsync(
        string deviceName, string actionName)
    {
        // Finds the target device.
        var device = _devices.FirstOrDefault(d =>
            d.Name.Equals(deviceName, StringComparison.OrdinalIgnoreCase));

        if (string.IsNullOrEmpty(device.Url))
        {
            return "Device not found.";
        }

        var config = await GetDeviceConfigAsync(deviceName);

        // Finds the requested action in the device configuration.
        var action = config?.Actions.FirstOrDefault(a =>
            a.Action.Equals(actionName, StringComparison.OrdinalIgnoreCase));

        if (action == null)
        {
            return "Action not found.";
        }

        // Executes the supported INFO action.
        if (action.CommandType == 0 && action.Command == "info")
        {
            return await _httpClient.GetStringAsync(
                $"{device.Url}/info");
        }

        // Prevents execution of actions that are not enabled.
        return "This action is not enabled.";
    }
}