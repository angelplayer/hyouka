using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using hyouka_api.Feature;
using hyouka_api.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace hyouka_api.Feature.Genres
{
  public class List
  {
    public class Query : IRequest<GenresEnvelope>
    {

    }

    public class QueryHandler : IRequestHandler<Query, GenresEnvelope>
    {
      private readonly HyoukaContext context;

      public QueryHandler(HyoukaContext context)
      {
        this.context = context;
      }

      public async Task<GenresEnvelope> Handle(Query message, CancellationToken cancellationToken)
      {
        var genres = await this.context.Genres.OrderBy(x => x.GenreId).AsNoTracking().ToListAsync(cancellationToken);
        return new GenresEnvelope()
        {
          Genre = genres.ToList()
        };
      }
    }
  }
}