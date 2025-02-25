using ChitChat.Service.Implementations;
using Microsoft.AspNetCore.Mvc;

namespace ChitChat.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatsController : ControllerBase
    {
        private readonly ServicesUnit services;

        public ChatsController(ServicesUnit services)
        {
            this.services = services;
        }
    }
}
