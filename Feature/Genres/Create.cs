using System.Threading;
using System.Threading.Tasks;
using hyouka_api.Domain;
using hyouka_api.Infrastructure;
using MediatR;

namespace hyouka_api.Feature.Genres
{

  public class Create
  {
    public class Command : IRequest<GenreEnvelope>
    {
      public string Name { get; set; }
    }

    public class Handler : IRequestHandler<Command, GenreEnvelope>
    {
      private HyoukaContext context;

      public Handler(HyoukaContext context)
      {
        this.context = context;
      }

      public async Task<GenreEnvelope> Handle(Command message, CancellationToken cancellationToken)
      {
        var genre = new Genre(message.Name);
        await this.context.Genres.AddAsync(genre, cancellationToken);
        await this.context.SaveChangesAsync(cancellationToken);
        return new GenreEnvelope(genre);
      }
    }
  }
}