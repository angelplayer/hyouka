using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace hyouka_api.Feature.Genres
{
    [Route("api/[Controller]")]
    public class GenresController : Controller
    {
        private readonly IMediator mediator;

        public GenresController(IMediator mediator) {
            this.mediator = mediator;
        }

        public async Task<GenresEnvelope> Get() 
        {
            return await this.mediator.Send(new List.Query());
        }

        [HttpPost]
        public async Task<GenreEnvelope> Post([FromBody] Create.Command command) 
        {
            return await this.mediator.Send(command);
        }
    }
}