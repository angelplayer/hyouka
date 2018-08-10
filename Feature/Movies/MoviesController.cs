

using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace hyouka_api.Feature
{
  [Route("api/[controller]")]
  public class MoviesController : Controller
  {
    private IMediator _mediator;

    public MoviesController(IMediator _mediator)
    {
      this._mediator = _mediator;
    }

    [HttpPost]
    public async Task<MovieEnvelope> Create([FromBody] Create.Command command)
    {
      return await this._mediator.Send(command);
    }
  }
}