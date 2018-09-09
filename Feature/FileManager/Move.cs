

using System.Threading;
using System.Threading.Tasks;
using System.IO;
using MediatR;
using System;

namespace hyouka_api.Feature.FileManger
{
  public class MoveCommand : IRequest<ActionResultEnvelope>
  {
    public string[] Items { get; set; }
    public string NewPath { get; set; }

    public MoveCommand(string[] items, string path)
    {
      this.Items = items;
      this.NewPath = path;
    }
  }

  public class MoveHandler : IRequestHandler<MoveCommand, ActionResultEnvelope>
  {
    private IPathMapper mapper;

    public MoveHandler(IPathMapper mapper)
    {
      this.mapper = mapper;
    }

    public Task<ActionResultEnvelope> Handle(MoveCommand request, CancellationToken cancellationToken)
    {
      var destination = mapper.MapPath(request.NewPath);
      try
      {
        FileInfo fileInfo = null;
        foreach (var path in request.Items)
        {
          var source = mapper.MapPath(path);
          fileInfo = new FileInfo(source);
          Directory.Move(source, $"{destination}/{fileInfo.Name}");
        }
      }
      catch (Exception)
      {
        throw new InvalideFileOperationException("Uanble to move. The same file is already exist in destionation");
      }

      return Task.FromResult<ActionResultEnvelope>(new ActionResultEnvelope(new FileActionResult
      {
        Success = true,
        Error = null
      }));
    }
  }
}