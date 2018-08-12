
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using hyouka_api.Feature;
using hyouka_api.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace hyouka_api.Feature.Movies
{
  public class List
  {
    public class Query : IRequest<MoviesEnvelope>
    {
      public Query()
      {

      }

      public Query(string tag)
      {
        this.Tag = tag;
      }

      public string Tag { get; set; }
    }

    public class QueryHandler : IRequestHandler<Query, MoviesEnvelope>
    {
      private HyoukaContext context;

      public QueryHandler(HyoukaContext context)
      {
        this.context = context;
      }

      public async Task<MoviesEnvelope> Handle(Query message, CancellationToken cancellationToken)
      {
        var query = this.context.Movies.GetAllData();
        if (!string.IsNullOrEmpty(message.Tag))
        {
          query = this.context.MovieGenre.Where(x => x.Genre.Name == message.Tag).Select(mg => mg.Movie).AsNoTracking();
        }

        var movies = await query.ToListAsync();
        return new MoviesEnvelope()
        {
          Movies = movies,
          Count = movies.Count()
        };
      }
    }
  }
}