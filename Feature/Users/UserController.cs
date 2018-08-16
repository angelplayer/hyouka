
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace hyouka_api.Feature.Users
{
    [Route("api/[Controller]")]
    public class UserController : Controller
    {
        private IMediator mediator;

        public UserController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        // [HttpGet("Login")]
        // public async Task<UserEnvelope> Login([FromBody] Login.Command command)
        // {

        // }

        [HttpPost]
        public async Task<UserEnvelope> Register([FromBody] Create.Command command)
        {
            return await this.mediator.Send(command);
        }
    }
}