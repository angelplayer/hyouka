using System.Collections.Generic;
using hyouka_api.Domain;

namespace hyouka_api.Feature.Genres
{
  public class GenresEnvelope
  {
    public List<string> Genre { get; set; }
  }

  public class GenreEnvelope
  {
    public Genre Genre { get; set; }

    public GenreEnvelope(Genre gerne)
    {
      this.Genre = gerne;
    }
  }
}