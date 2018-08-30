
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using hyouka_api.Infrastructure;
using MediatR;
using System.Linq;
using System.Net;
using System;
using AutoMapper;
using hyouka_api.Infrastructure.security;

namespace hyouka_api.Feature.Users
{
  public class Create
  {
    public class UserData
    {
      public string Username { get; set; }
      public string Email { get; set; }
      public string Password { get; set; }
    }

    public class Command : IRequest<UserEnvelope>
    {
      public UserData User { get; set; }
    }

    public class QueryHandler : IRequestHandler<Command, UserEnvelope>
    {
      private HyoukaContext context;
      private IPasswordHasher hasher;
      private IMapper mapper;
      private IJwtTokenGenerator jwtGenerator;

      public QueryHandler(HyoukaContext context, IPasswordHasher hasher, IMapper mapper, IJwtTokenGenerator jwt)
      {
        this.jwtGenerator = jwt;
        this.context = context;
        this.hasher = hasher;
        this.mapper = mapper;
      }

      public async Task<UserEnvelope> Handle(Command request, CancellationToken cancellationToken)
      {
        if (await this.context.Person.Where(x => x.Username == request.User.Username).AnyAsync(cancellationToken))
        {
          throw new RestException(HttpStatusCode.BadRequest, Constants.IN_USE);
        }

        if (await this.context.Person.Where(x => x.Email == request.User.Email).AnyAsync())
        {
          throw new RestException(HttpStatusCode.BadRequest, Constants.IN_USE);
        }

        var salt = Guid.NewGuid().ToByteArray();
        var person = new Domain.Person()
        {
          Username = request.User.Username,
          Email = request.User.Email,
          Hash = this.hasher.Hash(request.User.Password, salt),
          Salt = salt
        };

        this.context.Person.Add(person);
        await this.context.SaveChangesAsync(cancellationToken);
        var user = mapper.Map<Domain.Person, User>(person);
        user.Token = await this.jwtGenerator.CreateToke(user.Username);

        return new UserEnvelope(user);
      }
    }
  }
}