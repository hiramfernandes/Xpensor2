using Microsoft.AspNetCore.Mvc;

namespace Xpensor2.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        [HttpGet("hello")]
        public async Task<IActionResult> Startup()
        {
            await Task.Delay(1000);

            return Ok("Hello");
        }
    }
}
