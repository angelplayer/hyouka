

using System.IO;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace hyouka_api.Feature.FileManger
{
  public class RemoveCommand : IRequest<ActionResultEnvelope>
  {
    public string[] Items { get; set; }

    public RemoveCommand(string[] items)
    {
      this.Items = items;
    }
  }

  public class RemoveHandler : IRequestHandler<RemoveCommand, ActionResultEnvelope>
  {

    private IPathMapper mapper;

    public RemoveHandler(IPathMapper mapper)
    {
      this.mapper = mapper;
    }

    public Task<ActionResultEnvelope> Handle(RemoveCommand request, CancellationToken cancellationToken)
    {
      foreach (var path in request.Items)
      {
        var target = mapper.MapPath(path);
        if (File.Exists(target))
        {
          File.Delete(target);
        }
        else if (Directory.Exists(target))
        {
          Directory.Delete(target, true);
        }
      }

      return Task.FromResult(new ActionResultEnvelope(new FileActionResult() { Success = true, Error = null }));
    }
  }
}