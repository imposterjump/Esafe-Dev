using BankProject.Data.Enums;
using BankProject.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace BankProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : BaseController
    {
        [Authorize(Role.Admin)]
        [HttpGet("test_authentication3")]
        public async Task<ActionResult> testingAuth2()
        {

            if (this.HttpContext.Response.StatusCode == 200)
            {
                var client = Admin;
                return Ok(client);
            }
            else
            {
                return Content("sorry but unothorised or couldnt find content");
            }
        }
    }
}
