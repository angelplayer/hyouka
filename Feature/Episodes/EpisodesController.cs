using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using hyouka_api.Feature.Episodes;

namespace hyouka_api.Controllers
{
  [Route("api/movies")]
  public class EpisodesController : Controller
  {
    private readonly IMediator mediator;

    public EpisodesController(IMediator mediator)
    {
      this.mediator = mediator;
    }

    [HttpGet("{movieId}/episodes")]
    public async Task<EpisodesEvelope> Get(int movieId)
    {
      return await this.mediator.Send(new List.Query(movieId));
    }

    [HttpPost("{movieId}/episodes")]
    public async Task<EpisodeEnvelope> Create(int movieId, [FromBody] Create.Command command)
    {
      command.MovieId = movieId;
      return await this.mediator.Send(command);
    }

    [HttpDelete("{movieId}/episodes/{id}")]
    public async Task<Unit> Delete(int movieId, int id)
    {
      return await this.mediator.Send(new Delete.Command(id, movieId));
    }
  }
}