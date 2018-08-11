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

    [HttpPost("{movieId}/episodes")]
    public async Task<EpisodeEnvelope> Create(int movieId,[FromBody] Create.Command command)
    {
      command.MovieId = movieId;
      return await this.mediator.Send(command);
    }
  }
}