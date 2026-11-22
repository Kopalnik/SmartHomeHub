namespace SmartHomeHub.Models;

/// <summary>
/// Represents the configuration of a smart home device,
/// including its name, address and available actions.
/// </summary>
public class DeviceConfig
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
    /// Gets or sets the list of actions defined
    /// in the device configuration.
    /// </summary>
    public List<DeviceAction> Actions { get; set; } = new();
}