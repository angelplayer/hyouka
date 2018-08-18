using System.Collections.Generic;
using hyouka_api.Domain;

namespace hyouka_api.Feature.Movies
{
  public class MovieEnvelope
  {
    public MovieEnvelope(Movie movie)
    {
      this.Movie = movie;
    }

    public Movie Movie { get; }
  }

  public class MoviesEnvelope
  {
    public List<Movie> Movies { get; set; }
    public int Count { get; set; }
  }
}