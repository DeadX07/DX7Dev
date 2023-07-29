using Microsoft.AspNetCore.Mvc;

namespace Web
{
    [ApiController]
    [Route("api/upload")]
    public class ApiController : ControllerBase
    {
        [HttpPost]
        public IActionResult Upload()
        {
            return Ok();
        }
    }
}