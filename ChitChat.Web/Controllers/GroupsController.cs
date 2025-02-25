using ChitChat.Service.Implementations;
using Microsoft.AspNetCore.Mvc;

namespace ChitChat.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GroupsController : ControllerBase
    {
        private readonly ServicesUnit services;

        public GroupsController(ServicesUnit services)
        {
            this.services = services;
        }
    }
}
