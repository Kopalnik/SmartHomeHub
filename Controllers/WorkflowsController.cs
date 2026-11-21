using Microsoft.AspNetCore.Mvc;
using SmartHomeHub.Models;
using SmartHomeHub.Services;

namespace SmartHomeHub.Controllers;

[ApiController]
[Route("api/workflows")]
public class WorkflowsController : ControllerBase
{
    private readonly WorkflowService _workflowService;

    public WorkflowsController(WorkflowService workflowService)
    {
        _workflowService = workflowService;
    }

    [HttpGet]
    public IActionResult GetWorkflows()
    {
        return Ok(_workflowService.GetWorkflows());
    }

    [HttpGet("{id:guid}")]
    public IActionResult GetWorkflow(Guid id)
    {
        var workflow = _workflowService.GetWorkflow(id);

        if (workflow == null)
        {
            return NotFound();
        }

        return Ok(workflow);
    }

    [HttpPost]
    public IActionResult AddWorkflow(Workflow workflow)
    {
        var created = _workflowService.AddWorkflow(workflow);

        return CreatedAtAction(
            nameof(GetWorkflow),
            new { id = created.Id },
            created);
    }

    [HttpDelete("{id:guid}")]
    public IActionResult DeleteWorkflow(Guid id)
    {
        if (!_workflowService.DeleteWorkflow(id))
        {
            return NotFound();
        }

        return NoContent();
    }
}