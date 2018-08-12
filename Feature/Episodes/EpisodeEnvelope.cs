using System.Collections.Generic;
using hyouka_api.Domain;

namespace hyouka_api.Feature.Episodes
{
  public class EpisodeEnvelope
  {
    public Episode Episode { get; set; }

    public EpisodeEnvelope(Episode newEpi)
    {
      this.Episode = newEpi;
    }
  }

  public class EpisodesEvelope
  {
    public List<Episode> Episodes { get; set; }
    public int Count => Episodes.Count;
  }
}