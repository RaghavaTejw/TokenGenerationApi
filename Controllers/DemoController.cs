using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TokenGenerationApi.Authorization;
using TokenGenerationApi.Models;

namespace TokenGenerationApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DemoController : ControllerBase
    {
        [HttpGet]
        [HasPermissionAttribue(Permission.Read)]
        public IActionResult Display()
        {

            return Ok(new string[] {"DCT","UAQ"});
        }
    }
}
