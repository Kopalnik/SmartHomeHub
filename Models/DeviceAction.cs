namespace SmartHomeHub.Models;

public class DeviceAction
{
    public string Action { get; set; } = string.Empty;

    public int CommandType { get; set; }

    public string Command { get; set; } = string.Empty;

    public string? CommandArgs { get; set; }
}