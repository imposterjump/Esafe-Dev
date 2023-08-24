using BankProject.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BankProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public abstract class BaseController : ControllerBase
    {
        public Client Client => (Client)HttpContext.Items["Client"];
        public Admin Admin => (Admin)HttpContext.Items["Client"];
        


    }
}
