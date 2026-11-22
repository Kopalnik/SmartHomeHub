using Microsoft.AspNetCore.Mvc;
using SmartHomeHub.Models;
using SmartHomeHub.Services;

namespace SmartHomeHub.Controllers;

/// <summary>
/// Provides REST API endpoints for managing
/// smart home automation workflows.
/// </summary>
[ApiController]
[Route("api/workflows")]
public class WorkflowsController : ControllerBase
{
    private readonly WorkflowService _workflowService;

    /// <summary>
    /// Initializes the controller with the workflow service.
    /// </summary>
    public WorkflowsController(WorkflowService workflowService)
    {
        _workflowService = workflowService;
    }

    /// <summary>
    /// Retrieves all registered automation workflows.
    /// </summary>
    [HttpGet]
    public IActionResult GetWorkflows()
    {
        return Ok(_workflowService.GetWorkflows());
    }

    /// <summary>
    /// Retrieves a specific workflow by its unique identifier.
    /// </summary>
    [HttpGet("{id:guid}")]
    public IActionResult GetWorkflow(Guid id)
    {
        var workflow = _workflowService.GetWorkflow(id);

        // Returns HTTP 404 if the workflow does not exist.
        if (workflow == null)
        {
            return NotFound();
        }

        return Ok(workflow);
    }

    /// <summary>
    /// Creates and registers a new automation workflow.
    /// </summary>
    [HttpPost]
    public IActionResult AddWorkflow(Workflow workflow)
    {
        var created = _workflowService.AddWorkflow(workflow);

        // Returns HTTP 201 with the location of the created workflow.
        return CreatedAtAction(
            nameof(GetWorkflow),
            new { id = created.Id },
            created);
    }

    /// <summary>
    /// Deletes an automation workflow by its unique identifier.
    /// </summary>
    [HttpDelete("{id:guid}")]
    public IActionResult DeleteWorkflow(Guid id)
    {
        // Returns HTTP 404 if the workflow does not exist.
        if (!_workflowService.DeleteWorkflow(id))
        {
            return NotFound();
        }

        // Returns HTTP 204 after successful deletion.
        return NoContent();
    }
}