namespace SmartHomeHub.Services;

/// <summary>
/// Provides a background automation engine that executes
/// workflows based on defined triggers and conditions.
/// </summary>
public class WorkflowEngine : BackgroundService
{
    private readonly WorkflowService _workflowService;
    private readonly DeviceService _deviceService;
    private readonly ILogger<WorkflowEngine> _logger;

    /// <summary>
    /// Initializes the workflow engine with the required services
    /// and logger.
    /// </summary>
    public WorkflowEngine(
        WorkflowService workflowService,
        DeviceService deviceService,
        ILogger<WorkflowEngine> logger)
    {
        _workflowService = workflowService;
        _deviceService = deviceService;
        _logger = logger;
    }

    /// <summary>
    /// Continuously evaluates and executes enabled workflows
    /// until the background service is stopped.
    /// </summary>
    protected override async Task ExecuteAsync(
        CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            // Retrieves all registered workflows.
            foreach (var workflow in _workflowService.GetWorkflows())
            {
                // Skips disabled workflows.
                if (!workflow.IsEnabled)
                    continue;

                try
                {
                    // Evaluates the unconditional execution condition.
                    bool conditionMet = workflow.Condition == "always";

                    // Checks whether the target device is online.
                    if (workflow.Condition == "online")
                    {
                        var devices = await _deviceService.GetDevicesAsync();

                        conditionMet = devices.Any(d =>
                            d.Name.Equals(workflow.DeviceName,
                                StringComparison.OrdinalIgnoreCase) &&
                            d.IsOnline);
                    }

                    // Executes the action when the trigger and condition are satisfied.
                    if (workflow.Trigger == "interval" && conditionMet)
                    {
                        var result = await _deviceService
                            .ExecuteDeviceActionAsync(
                                workflow.DeviceName,
                                workflow.Action);

                        // Logs the workflow execution result.
                        _logger.LogInformation(
                            "Workflow {Name}: {Result}",
                            workflow.Name,
                            result);
                    }
                }
                catch (Exception ex)
                {
                    // Logs workflow execution errors.
                    _logger.LogError(
                        ex,
                        "Workflow {Name} failed",
                        workflow.Name);
                }
            }

            // Waits 10 seconds before the next evaluation cycle.
            await Task.Delay(
                TimeSpan.FromSeconds(10),
                stoppingToken);
        }
    }
}