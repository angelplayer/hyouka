
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using hyouka_api.Feature.FileManger;
using MediatR;
using System;

namespace hyouka_api.Feature.FileManger
{
  public class RenameCommand : IRequest<ActionResultEnvelope>
  {
    public string Item { get; set; }
    public string newItemPath { get; set; }

    public RenameCommand(string item, string newPath)
    {
      this.Item = item;
      this.newItemPath = newPath;
    }
  }

  public class RenameHandler : IRequestHandler<RenameCommand, ActionResultEnvelope>
  {
    private IPathMapper mapper;

    public RenameHandler(IPathMapper mapper)
    {
      this.mapper = mapper;
    }

    public Task<ActionResultEnvelope> Handle(RenameCommand request, CancellationToken cancellationToken)
    {
      var path = mapper.MapPath(request.Item);
      var newPath = mapper.MapPath(request.newItemPath);

      try
      {
        if (File.Exists(path))
        {
          File.Move(path, newPath);
        }
        else if (Directory.Exists(path))
        {
          Directory.Move(path, newPath);
        }
      }
      catch (Exception)
      {
        throw new InvalideFileOperationException("Unable to rename file");
      }

      return Task.FromResult(new ActionResultEnvelope(new FileActionResult() { Success = true, Error = null }));
    }
  }
}