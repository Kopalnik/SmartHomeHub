namespace SmartHomeHub.Models;

public class DeviceConfig
{
    public string Name { get; set; } = string.Empty;

    public string Address { get; set; } = string.Empty;

    public List<DeviceAction> Actions { get; set; } = new();
}