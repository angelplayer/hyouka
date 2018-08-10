using hyouka_api.Domain;

namespace hyouka_api.Feature
{
  public class MovieEnvelope
  {
    public MovieEnvelope(Movie movie)
    {
      this.Movie = movie;
    }

    public Movie Movie { get; }
  }
}