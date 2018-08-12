

using System.Threading;
using System.Threading.Tasks;
using hyouka_api.Feature.Movies;
using hyouka_api.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Net;

namespace hyouka_api.Feature.Episodes
{
  public class Delete
  {
    public class Command : IRequest
    {
      public int id { get; set; }
      public int movieId { get; set; }

      public Command(int id, int movieId)
      {
        this.id = id;
        this.movieId = movieId;
      }
    }

    public class QueryHandler : IRequestHandler<Command>
    {

      private HyoukaContext context;

      public QueryHandler(HyoukaContext context)
      {
        this.context = context;
      }

      public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
      {
        var movie = await this.context.Movies.Include(x => x.Episodes)
        .SingleOrDefaultAsync(x => x.MovieId == request.movieId, cancellationToken);

        if (movie == null)
        {
          throw new RestException(HttpStatusCode.NotFound, new { Movie = Constants.NOT_FOUND });
        }

        var episode = movie.Episodes.SingleOrDefault(x => x.EpisodeId == request.id);
        this.context.Episodes.Remove(episode);
        await this.context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
      }
    }
  }
}