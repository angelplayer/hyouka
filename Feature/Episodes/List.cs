
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using hyouka_api.Feature.Movies;
using hyouka_api.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace hyouka_api.Feature.Episodes
{
  public class List
  {
    public class Query : IRequest<EpisodesEvelope>
    {
      public int MovieId { get; set; }

      public Query(int id)
      {
        this.MovieId = id;
      }
    }

    public class QueryHandler : IRequestHandler<Query, EpisodesEvelope>
    {
      private HyoukaContext hyoukaContext;

      public QueryHandler(HyoukaContext context)
      {
        this.hyoukaContext = context;
      }

      public async Task<EpisodesEvelope> Handle(Query request, CancellationToken cancellationToken)
      {
        var episodes = await this.hyoukaContext.Episodes.Where(x => x.MovieId == request.MovieId).ToListAsync();
        EpisodesEvelope envelope = new EpisodesEvelope();
        envelope.Episodes = episodes;

        return envelope;
      }
    }
  }
}