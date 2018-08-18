

using System.Threading;
using System.Threading.Tasks;
using hyouka_api.Infrastructure;
using MediatR;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System;

namespace hyouka_api.Feature.Movies
{
  public class Edit
  {

    public class MovieData
    {
      public string Title { get; set; }
      public string Description { get; set; }
      public string Image { get; set; }
      public DateTime realeaseDate { get; set; }
    }

    public class Command : IRequest<MovieEnvelope>
    {
      public int Id { get; set; }
      public MovieData Movie { get; set; }

    }

    public class QueryHandler : IRequestHandler<Command, MovieEnvelope>
    {
      private HyoukaContext context;

      public QueryHandler(HyoukaContext context)
      {
        this.context = context;
      }

      public async Task<MovieEnvelope> Handle(Command request, CancellationToken cancellationToken)
      {
        var movie = await this.context.Movies.SingleOrDefaultAsync(x => x.MovieId == request.Id);
        if (movie == null)
        {
          throw new RestException(HttpStatusCode.NotFound, new { Errors = "Not found" });
        }

        movie.Title = request.Movie.Title ?? movie.Title;
        movie.Description = request.Movie.Description ?? movie.Description;
        movie.ReleaseDate = request.Movie.realeaseDate.CompareTo(movie.ReleaseDate) == 0 ? movie.ReleaseDate : request.Movie.realeaseDate;
        movie.Image = request.Movie.Image;

        if (context.ChangeTracker.Entries().First(x => x.Entity == movie).State == EntityState.Modified)
        {
          movie.UpdatedAt = DateTime.UtcNow;
        }

        await context.SaveChangesAsync(cancellationToken);

        return new MovieEnvelope(await context.Movies.
        Where(x => x.MovieId == movie.MovieId).FirstOrDefaultAsync(cancellationToken));
      }
    }
  }
}