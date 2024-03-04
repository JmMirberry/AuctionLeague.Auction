using Microsoft.AspNetCore.Mvc;

namespace SlackAPI.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class ManualController : ControllerBase
    {
        [HttpGet]
        public IActionResult Test()
        {
            return Ok("This is a test");
        }
    }
}