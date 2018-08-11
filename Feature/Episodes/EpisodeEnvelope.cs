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
}