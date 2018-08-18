using System.Net;
using System.Threading;
using System.Threading.Tasks;
using hyouka_api.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace hyouka_api.Feature.Movies
{
  public class Detail
  {
    public class Query : IRequest<MovieEnvelope>
    {
      public int Id { get; set; }
      public Query(int id)
      {
        this.Id = id;
      }
    }

    public class QueryHandler : IRequestHandler<Query, MovieEnvelope>
    {
      private HyoukaContext context;

      public QueryHandler(HyoukaContext context)
      {
        this.context = context;
      }

      public async Task<MovieEnvelope> Handle(Query message, CancellationToken cancellationToken)
      {
        var movie = await this.context.Movies.GetAllData()
        .SingleOrDefaultAsync(x => x.MovieId == message.Id);
        if (movie == null)
        {
          throw new RestException(HttpStatusCode.NotFound, new { Movie = Constants.NOT_FOUND });
        }

        return new MovieEnvelope(movie);
      }
    }
  }
}