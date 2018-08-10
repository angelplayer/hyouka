
namespace hyouka_api.Domain
{
    public class Genre
    {
        public int GenreId { get; set; }
        public string Name { get; set; }

        public Genre() { }

        public Genre(string genre) 
        {
            this.Name = genre;
        }
    }
}