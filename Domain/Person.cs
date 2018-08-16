using Newtonsoft.Json;

namespace hyouka_api.Domain
{
  public class Person
  {
    public int PersonId { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string Img { get; set; }

    [JsonIgnore] public byte[] Hash { get; set; }

    [JsonIgnore] public byte[] Salt { get; set; }
  }
}