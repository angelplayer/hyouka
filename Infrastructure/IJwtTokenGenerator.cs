using System.Threading.Tasks;

namespace hyouka_api
{
    public interface IJwtTokenGenerator
    {
        Task<string> CreateToke(string username);
    }
}