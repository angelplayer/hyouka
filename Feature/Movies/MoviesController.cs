

using System.Threading.Tasks;
using hyouka_api.Feature;
using hyouka_api.Feature.Movies;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace hyouka_api.Feature.Movies
{
  [Route("api/[controller]")]
  public class MoviesController : Controller
  {
    private IMediator _mediator;

    public MoviesController(IMediator _mediator)
    {
      this._mediator = _mediator;
    }

    [HttpGet]
    public async Task<MoviesEnvelope> Get([FromQuery]string tag)
    {
      return await this._mediator.Send(new List.Query(tag));
    }

    [HttpGet("{id}")]
    public async Task<MovieEnvelope> Get(int id)
    {
      return await this._mediator.Send(new Detail.Query(id));
    }

    [HttpPost]
    public async Task<MovieEnvelope> Create([FromBody] Create.Command command)
    {
      return await this._mediator.Send(command);
    }

    [HttpDelete("{id}")]
    public async Task<Unit> delete(int id) 
    {
      return await this._mediator.Send(new Delete.Command(id));
    }
  }
}