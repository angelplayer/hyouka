using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
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

    [NotMapped]
    public List<int> genreList => (MovieGenre?.Select(x => x.GenreId) ?? Enumerable.Empty<int>()).ToList();
    [NotMapped] public int EpisodeCount => EpisodeList?.Count ?? 0;

    [JsonIgnore]
    public List<MovieGenre> MovieGenre { get; set; }

    [JsonIgnore]
    public List<Episode> EpisodeList { get; set; }


    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
  }
}