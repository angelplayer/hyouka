using hyouka_api.Domain;

namespace hyouka_api.Feature.Users
{
    public class UserEnvelope
    {
        public User User { get; set; }

        public UserEnvelope(User user)
        {
            this.User = user;
        }
    }

    public class User 
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Img { get; set; }
        public string Token { get; set; }


    }
}