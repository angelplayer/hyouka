using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using hyouka_api.Infrastructure;
using MediatR;
using hyouka_api.Domain;

namespace hyouka_api.Feature.Episodes
{
  public class Create
  {
    public class EpisodeData
    {
      public string Name { get; set; }
      public string Number { get; set; }
      public string File { get; set; }
    }

    public class Command : IRequest<EpisodeEnvelope>
    {
      public EpisodeData Episode { get; set; }
      public int MovieId { get; set; }
    }

    public class Handler : IRequestHandler<Command, EpisodeEnvelope>
    {
      private HyoukaContext context;

      public Handler(HyoukaContext context)
      {
        this.context = context;
      }

      public async Task<EpisodeEnvelope> Handle(Command message, CancellationToken cancellationToken)
      {
        var movie = await this.context.Movies.SingleOrDefaultAsync(x => x.MovieId == message.MovieId);

        if (movie == null)
        {
          throw new RestException(HttpStatusCode.NotFound, new { Movie = Constants.NOT_FOUND });
        }

        var episode = new Episode()
        {
          Movie = movie,
          Name = message.Episode.Name,
          File = message.Episode.File,
          Number = message.Episode.Number
        };

        await this.context.Episodes.AddAsync(episode);
        movie.Episodes.Add(episode);
        await this.context.SaveChangesAsync();

        return new EpisodeEnvelope(episode);
      }
    }
  }
}