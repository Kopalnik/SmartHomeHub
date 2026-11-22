using SmartHomeHub.Models;
using System.Collections.Concurrent;

namespace SmartHomeHub.Services;

/// <summary>
/// Provides management and in-memory storage
/// of smart home automation workflows.
/// </summary>
public class WorkflowService
{
    // Stores workflows in a thread-safe dictionary.
    private readonly ConcurrentDictionary<Guid, Workflow> _workflows = new();

    /// <summary>
    /// Retrieves a list of all registered workflows.
    /// </summary>
    public IEnumerable<Workflow> GetWorkflows()
    {
        return _workflows.Values.ToList();
    }

    /// <summary>
    /// Retrieves a specific workflow by its unique identifier.
    /// Returns null if the workflow does not exist.
    /// </summary>
    public Workflow? GetWorkflow(Guid id)
    {
        return _workflows.TryGetValue(id, out var workflow)
            ? workflow
            : null;
    }

    /// <summary>
    /// Creates and registers a new automation workflow.
    /// </summary>
    public Workflow AddWorkflow(Workflow workflow)
    {
        // Generates a unique identifier for the new workflow.
        workflow.Id = Guid.NewGuid();

        // Stores the workflow in the dictionary.
        _workflows[workflow.Id] = workflow;

        return workflow;
    }

    /// <summary>
    /// Removes a workflow by its unique identifier.
    /// Returns true if the workflow was successfully removed.
    /// </summary>
    public bool DeleteWorkflow(Guid id)
    {
        return _workflows.TryRemove(id, out _);
    }
}