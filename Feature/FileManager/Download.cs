
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.FileProviders;

namespace hyouka_api.Feature.FileManger
{
  public class SingleDownloadCommnad : IRequest<FileContentResult>
  {
    public string Path;

    public SingleDownloadCommnad(string path)
    {
      this.Path = path;
    }
  }

  public class SingleDownloadHandler : IRequestHandler<SingleDownloadCommnad, FileContentResult>
  {
    private IPathMapper mapper;

    public SingleDownloadHandler(IPathMapper mapper)
    {
      this.mapper = mapper;
    }

    public async Task<FileContentResult> Handle(SingleDownloadCommnad request, CancellationToken cancellationToken)
    {
      var path = mapper.MapPath(request.Path);
      FileInfo fileInfo = new FileInfo(path);
      var memory = new MemoryStream();
      using (var stream = fileInfo.OpenRead())
      {
        await stream.CopyToAsync(memory);
      }
      memory.Position = 0;
      FileExtensionContentTypeProvider guest = new FileExtensionContentTypeProvider();
      string contentType = "";
      guest.TryGetContentType(path, out contentType);
      return new FileContentResult(memory.ToArray(), contentType);
    }
  }
}