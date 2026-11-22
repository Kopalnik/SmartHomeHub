namespace SmartHomeHub.Models;

/// <summary>
/// Represents information about a smart home device,
/// including its name, network address and metrics.
/// </summary>
public class DeviceInfo
{
    /// <summary>
    /// Gets or sets the name of the smart home device.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the network address of the device.
    /// </summary>
    public string Address { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the device metrics, such as
    /// temperature and uptime.
    /// </summary>
    public Dictionary<string, string> Metrics { get; set; } = new();
}