using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace hyouka_api.Domain
{
  public class Movie
  {
    [JsonIgnore]
    public int MovieId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Image { get; set; }
    public DateTime ReleaseDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    [NotMapped]
    public List<string> GenreList => (MovieGenre?.Select(x => x.Genre.Name) ?? Enumerable.Empty<string>()).ToList();

    [NotMapped] public int EpisodeCount => Episodes?.Count ?? 0;

    [JsonIgnore]
    public List<MovieGenre> MovieGenre { get; set; }

    [JsonIgnore]
    public List<Episode> Episodes { get; set; }
  }
}