namespace SmartHomeHub.Models;

/// <summary>
/// Represents an action defined in a smart home device configuration.
/// </summary>
public class DeviceAction
{
    /// <summary>
    /// Gets or sets the name of the device action.
    /// </summary>
    public string Action { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the numeric type of the device command.
    /// </summary>
    public int CommandType { get; set; }

    /// <summary>
    /// Gets or sets the command associated with the action.
    /// </summary>
    public string Command { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the optional arguments required by the command.
    /// </summary>
    public string? CommandArgs { get; set; }
}