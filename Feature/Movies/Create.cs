

using System;
using System.Threading;
using System.Threading.Tasks;
using hyouka_api.Controllers;
using hyouka_api.Domain;
using hyouka_api.Infrastructure;
using MediatR;
using System.Linq;

namespace hyouka_api.Feature.Movies
{
  namespace Create
  {
    public class MovieData
    {
      public string Title { get; set; }
      public string Description { get; set; }
      public DateTime RealeaseDate { get; set; }
      public int[] GenreList { get; set; }
      public string Image { get; set; }
    }

    public class Command : IRequest<MovieEnvelope>
    {
      public MovieData Movie { get; set; }
    }

    public class Handler : IRequestHandler<Command, MovieEnvelope>
    {
      private readonly HyoukaContext context;

      public Handler(HyoukaContext context)
      {
        this.context = context;
      }

      public async Task<MovieEnvelope> Handle(Command message, CancellationToken cancellationToken)
      {
        var movie = new Movie()
        {
          Title = message.Movie.Title,
          Description = message.Movie.Description,
          CreatedAt = DateTime.UtcNow,
          UpdatedAt = DateTime.UtcNow,
          Image = message.Movie.Image,
          ReleaseDate = message.Movie.RealeaseDate
        };

        await this.context.Movies.AddAsync(movie, cancellationToken);
        await this.context.MovieGenre.AddRangeAsync(
            message.Movie.GenreList.Select(x => new MovieGenre
            {
              Movie = movie,
              GenreId = x
            })
         , cancellationToken);
        await this.context.SaveChangesAsync(cancellationToken);
        movie.MovieGenre = null;
        return new MovieEnvelope(movie);
      }
    }
  }
}

