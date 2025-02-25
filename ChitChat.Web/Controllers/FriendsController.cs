using ChitChat.Service.Implementations;
using Microsoft.AspNetCore.Mvc;

namespace ChitChat.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FriendsController : ControllerBase
    {
        private readonly ServicesUnit services;

        public FriendsController(ServicesUnit services)
        {
            this.services = services;
        }
    }
}
