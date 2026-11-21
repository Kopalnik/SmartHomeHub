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
}