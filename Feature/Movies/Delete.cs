using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.EntityFrameworkCore;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using hyouka_api.Infrastructure;
using System.Net;

namespace hyouka_api.Feature.Movies
{
    public class Delete 
    {
        public class Command : IRequest 
        {
            public int Id { get; set; }

            public Command(int id)
            {
                this.Id = id;
            }
        }

        public class QueryHandler : IRequestHandler<Command, Unit>
        {
            private HyoukaContext context;

            public QueryHandler(HyoukaContext context)
            {
                this.context = context;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var movie = await this.context.Movies.Include(x => x.Episodes).SingleOrDefaultAsync(x => x.MovieId == request.Id);
                if(movie == null) 
                {
                    throw new RestException(HttpStatusCode.NotFound, new { Errros = "No content found." });
                }

                this.context.Movies.Remove(movie);
                await this.context.SaveChangesAsync();
                return Unit.Value;
            }
        }
    }
}