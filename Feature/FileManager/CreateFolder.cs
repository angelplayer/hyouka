using System.IO;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.FileProviders;

namespace hyouka_api.Feature.FileManger
{
  public class CreateFolderCommand : IRequest<ActionResultEnvelope>
  {
    public string NewPath { get; set; }

    public CreateFolderCommand(string newPath)
    {
      this.NewPath = newPath;
    }
  }

  class CreateFolderHandler : IRequestHandler<CreateFolderCommand, ActionResultEnvelope>
  {
    private IPathMapper mapper;

    public CreateFolderHandler(IPathMapper mapper)
    {
      this.mapper = mapper;
    }

    public Task<ActionResultEnvelope> Handle(CreateFolderCommand request, CancellationToken cancellationToken)
    {
      var path = mapper.MapPath(request.NewPath);
      if (Directory.Exists(path))
      {
        throw new InvalideFileOperationException("Directory is already existed");
      }

      Directory.CreateDirectory(path);
      var result = new FileActionResult()
      {
        Success = true,
        Error = null
      };
      return Task.FromResult(new ActionResultEnvelope(result));
    }
  }
}