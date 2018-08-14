
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using hyouka_api.Feature;
using hyouka_api.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace hyouka_api.Feature.Movies
{
    public class List
    {
        public class Query : IRequest<MoviesEnvelope>
        {
            public Query()
            {

            }

            public Query(string tag)
            {
                this.Tag = tag;
            }

            public string Tag { get; set; }
        }

        public class QueryHandler : IRequestHandler<Query, MoviesEnvelope>
        {
            private HyoukaContext context;

            public QueryHandler(HyoukaContext context)
            {
                this.context = context;
            }

            public async Task<MoviesEnvelope> Handle(Query message, CancellationToken cancellationToken)
            {
                var query = this.context.Movies.GetAllData();
                if (!string.IsNullOrEmpty(message.Tag))
                {
                    var genre = await this.context.MovieGenre.FirstOrDefaultAsync(x => x.Genre.Name == message.Tag);
                    if (genre != null)
                    {
                        query = query.Where(x => x.MovieGenre.Select(y => y.Genre.GenreId).Contains(genre.GenreId));
                    }
                }

                var movies = await query.ToListAsync();
                return new MoviesEnvelope()
                {
                    Movies = movies,
                    Count = movies.Count()
                };
            }
        }
    }
}