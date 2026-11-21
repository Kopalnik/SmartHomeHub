using SmartHomeHub.Models;
using System.Collections.Concurrent;

namespace SmartHomeHub.Services;

public class DeviceService
{
    private readonly HttpClient _httpClient;

    private readonly ConcurrentDictionary<string, DateTime> _lastSeen = new();

    private readonly (string Name, string Url)[] _devices =
    {
        ("Device1", "https://localhost:1234"),
        ("Device2", "https://localhost:1236")
    };

    public DeviceService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<DeviceStatus[]> GetDevicesAsync()
    {
        return await Task.WhenAll(_devices.Select(GetDeviceStatusAsync));
    }

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
            status.Info = await _httpClient
                .GetFromJsonAsync<DeviceInfo>($"{device.Url}/info");

            status.IsOnline = status.Info != null;

            if (status.IsOnline)
            {
                _lastSeen[device.Url] = DateTime.UtcNow;
            }
        }
        catch (HttpRequestException)
        {
            status.IsOnline = false;
        }
        catch (OperationCanceledException)
        {
            status.IsOnline = false;
        }

        if (_lastSeen.TryGetValue(device.Url, out var lastSeen))
        {
            status.LastSeen = lastSeen;
        }

        return status;
    }
 public async Task<DeviceConfig?> GetDeviceConfigAsync(string deviceName)
{
    var device = _devices.FirstOrDefault(d =>
        d.Name.Equals(deviceName, StringComparison.OrdinalIgnoreCase));

    if (string.IsNullOrEmpty(device.Url))
    {
        return null;
    }

    return await _httpClient.GetFromJsonAsync<DeviceConfig>(
        $"{device.Url}/config");
}
public async Task<string> ExecuteDeviceActionAsync(
    string deviceName, string actionName)
{
    var device = _devices.FirstOrDefault(d =>
        d.Name.Equals(deviceName, StringComparison.OrdinalIgnoreCase));

    if (string.IsNullOrEmpty(device.Url))
    {
        return "Device not found.";
    }

    var config = await GetDeviceConfigAsync(deviceName);

    var action = config?.Actions.FirstOrDefault(a =>
        a.Action.Equals(actionName, StringComparison.OrdinalIgnoreCase));

    if (action == null)
    {
        return "Action not found.";
    }

    if (action.CommandType == 0 && action.Command == "info")
    {
        return await _httpClient.GetStringAsync(
            $"{device.Url}/info");
    }

    return "This action is not enabled.";
    }
}