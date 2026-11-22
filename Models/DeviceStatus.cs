namespace SmartHomeHub.Models;

/// <summary>
/// Represents the current status of a smart home device,
/// including its availability, information and last seen timestamp.
/// </summary>
public class DeviceStatus
{
    /// <summary>
    /// Gets or sets the name of the smart home device.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the API URL of the device.
    /// </summary>
    public string Url { get; set; } = string.Empty;

    /// <summary>
    /// Indicates whether the device is currently online.
    /// </summary>
    public bool IsOnline { get; set; }

    /// <summary>
    /// Gets or sets the detailed device information and metrics.
    /// </summary>
    public DeviceInfo? Info { get; set; }

    /// <summary>
    /// Gets or sets the timestamp when the device was last seen online.
    /// </summary>
    public DateTime? LastSeen { get; set; }
}