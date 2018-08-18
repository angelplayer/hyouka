namespace hyouka_api.Domain
{
  public class Episode
  {
    public int EpisodeId { get; set; }
    public string Name { get; set; }
    public string Number { get; set; }
    public string File { get; set; }
    public int MovieId {get; set;}

    public Movie Movie {get; set;}

  }
}