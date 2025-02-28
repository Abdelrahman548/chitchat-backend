using ChitChat.Service.Implementations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChitChat.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "User")]
    public class UsersController : ControllerBase
    {
        private readonly ServicesUnit services;

        public UsersController(ServicesUnit services)
        {
            this.services = services;
        }
    }
}
