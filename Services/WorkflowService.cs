using SmartHomeHub.Models;
using System.Collections.Concurrent;

namespace SmartHomeHub.Services;

public class WorkflowService
{
    private readonly ConcurrentDictionary<Guid, Workflow> _workflows = new();

    public IEnumerable<Workflow> GetWorkflows()
    {
        return _workflows.Values.ToList();
    }

    public Workflow? GetWorkflow(Guid id)
    {
        return _workflows.TryGetValue(id, out var workflow)
            ? workflow
            : null;
    }

    public Workflow AddWorkflow(Workflow workflow)
    {
        workflow.Id = Guid.NewGuid();
        _workflows[workflow.Id] = workflow;

        return workflow;
    }

    public bool DeleteWorkflow(Guid id)
    {
        return _workflows.TryRemove(id, out _);
    }
}