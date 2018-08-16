using System.Threading;
using System.Threading.Tasks;
using hyouka_api.Infrastructure;
using MediatR;

namespace hyouka_api.Feature.Users
{

    public class LoginData
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class Command : IRequest<UserEnvelope>
    {
        public LoginData User { get; set; }
    }

    // public class QueryHandler : IRequestHandler<Command, UserEnvelope>
    // {

    //     private HyoukaContext context;

    //     public QueryHandler(HyoukaContext context)
    //     {
    //         this.context = context;
    //     }

    //     public Task<UserEnvelope> Handle(Command message, CancellationToken cancellationToken)
    //     {
    //         return Task.CompletedTask;
    //     }
    // }
}