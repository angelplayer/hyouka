using System.Threading;
using System.Threading.Tasks;
using hyouka_api.Infrastructure;
using MediatR;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Net;
using AutoMapper;
using hyouka_api.Infrastructure.security;

namespace hyouka_api.Feature.Users
{
  public class Login
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

    public class QueryHandler : IRequestHandler<Command, UserEnvelope>
    {

      private HyoukaContext context;
      private readonly IPasswordHasher hasher;
      private readonly IMapper mapper;
      private readonly IJwtTokenGenerator generator;

      public QueryHandler(HyoukaContext context, IPasswordHasher hasher, IMapper mapper, IJwtTokenGenerator tokengenerator)
      {
        this.hasher = hasher;
        this.context = context;
        this.mapper = mapper;
        this.generator = tokengenerator;
      }

      public async Task<UserEnvelope> Handle(Command message, CancellationToken cancellationToken)
      {
        var person = await this.context.Person.SingleOrDefaultAsync(x => x.Username == message.User.Username);
        if (person == null)
        {
          throw new RestException(HttpStatusCode.NotFound, Constants.NOT_FOUND);
        }

        if (!person.Hash.SequenceEqual(hasher.Hash(message.User.Password, person.Salt)))
        {
          throw new RestException(HttpStatusCode.Unauthorized, new { Errors = "Username / Password is invalid." });
        }

        var user = mapper.Map<Domain.Person, User>(person);
        user.Token = await this.generator.CreateToke(user.Username);

        return new UserEnvelope(user);
      }
    }
  }
}