using ChitChat.Service.Implementations;
using Microsoft.AspNetCore.Mvc;

namespace ChitChat.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ServicesUnit services;

        public AuthController(ServicesUnit services)
        {
            this.services = services;
        }
    }
}
