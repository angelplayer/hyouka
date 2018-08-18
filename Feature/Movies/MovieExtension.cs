
using System.Linq;
using hyouka_api.Domain;
using Microsoft.EntityFrameworkCore;

namespace hyouka_api.Feature.Movies
{
  public static class MovieExtension
  {
    public static IQueryable<Movie> GetAllData(this DbSet<Movie> movie)
    {
      return movie.Include(x => x.MovieGenre).ThenInclude(x => x.Genre)
        .Include(x => x.Episodes)
        .AsNoTracking();
    }
  }
}