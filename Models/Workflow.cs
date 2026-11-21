namespace SmartHomeHub.Models;

public class Workflow
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Name { get; set; } = string.Empty;

    public bool IsEnabled { get; set; } = true;

    public string Trigger { get; set; } = string.Empty;

    public string DeviceName { get; set; } = string.Empty;

    public string Condition { get; set; } = string.Empty;

    public string Action { get; set; } = string.Empty;
}