namespace SmartHomeHub.Services;

public class WorkflowEngine : BackgroundService
{
    private readonly WorkflowService _workflowService;
    private readonly DeviceService _deviceService;
    private readonly ILogger<WorkflowEngine> _logger;

    public WorkflowEngine(
        WorkflowService workflowService,
        DeviceService deviceService,
        ILogger<WorkflowEngine> logger)
    {
        _workflowService = workflowService;
        _deviceService = deviceService;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(
        CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            foreach (var workflow in _workflowService.GetWorkflows())
            {
                if (!workflow.IsEnabled)
                    continue;

                try
                {
                    bool conditionMet = workflow.Condition == "always";

                    if (workflow.Condition == "online")
                    {
                        var devices = await _deviceService.GetDevicesAsync();

                        conditionMet = devices.Any(d =>
                            d.Name.Equals(workflow.DeviceName,
                                StringComparison.OrdinalIgnoreCase) &&
                            d.IsOnline);
                    }

                    if (workflow.Trigger == "interval" && conditionMet) 
                    {
                        var result = await _deviceService
                            .ExecuteDeviceActionAsync(
                                workflow.DeviceName,
                                workflow.Action);

                        _logger.LogInformation(
                            "Workflow {Name}: {Result}",
                            workflow.Name,
                            result);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(
                        ex,
                        "Workflow {Name} failed",
                        workflow.Name);
                }
            }

            await Task.Delay(
                TimeSpan.FromSeconds(10),
                stoppingToken);
        }
    }
}