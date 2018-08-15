using Newtonsoft.Json;

namespace hyouka_api.Domain
{
  public class Person
  {
    public int PersonId { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }

    [JsonIgnore] public string Hash { get; set; }

    [JsonIgnore] public string Salt { get; set; }
  }
}