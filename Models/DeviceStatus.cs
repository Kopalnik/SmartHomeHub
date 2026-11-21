namespace SmartHomeHub.Models;

public class DeviceStatus
{
    public string Name { get; set; } = string.Empty;

    public string Url { get; set; } = string.Empty;

    public bool IsOnline { get; set; }

    public DeviceInfo? Info { get; set; }

    public DateTime? LastSeen { get; set; }
}