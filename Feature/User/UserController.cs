
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace hyouka_api
{
  public class UserController : Controller
  {
    private IMediator mediator;

    public UserController(IMediator mediator)
    {
      this.mediator = mediator;
    }

    [HttpGet("Login")]
    public async Task<UserEnvelope> Login([FromBody] Login.Command command)
    {

    }
  }
}