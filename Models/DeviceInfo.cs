namespace SmartHomeHub.Models;

public class DeviceInfo
{
    public string Name { get; set; } = string.Empty;

    public string Address { get; set; } = string.Empty;

    public Dictionary<string, string> Metrics { get; set; } = new();
}