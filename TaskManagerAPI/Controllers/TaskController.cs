using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class TasksController : ControllerBase
{
    [HttpGet]
    [Authorize]
    public IActionResult GetTasks()
    {
        return Ok(new { message = "Tasks protected" });
    }
}
