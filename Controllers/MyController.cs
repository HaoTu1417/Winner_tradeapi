using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class MyController : ControllerBase
{
    /// <summary>
    /// Gets a new example response
    /// </summary>
    /// <returns>A simple message</returns>
    /// <response code="200">Returns the new message</response>
    [HttpGet("new-endpoint")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult GetNewEndpoint()
    {
        return Ok(new { message = "Hello, this is a new endpoint" });
    }
}