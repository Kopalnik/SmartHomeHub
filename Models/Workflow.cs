namespace SmartHomeHub.Models;

/// <summary>
/// Represents an automation workflow that defines
/// a trigger, a condition and a device action.
/// </summary>
public class Workflow
{
    /// <summary>
    /// Gets or sets the unique identifier of the workflow.
    /// </summary>
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Gets or sets the name of the workflow.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Indicates whether the workflow is enabled.
    /// </summary>
    public bool IsEnabled { get; set; } = true;

    /// <summary>
    /// Gets or sets the trigger that determines
    /// when the workflow should be executed.
    /// </summary>
    public string Trigger { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the name of the target smart home device.
    /// </summary>
    public string DeviceName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the condition that must be satisfied
    /// before executing the device action.
    /// </summary>
    public string Condition { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the device action to be executed.
    /// </summary>
    public string Action { get; set; } = string.Empty;
}