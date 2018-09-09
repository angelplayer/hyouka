

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.FileProviders;

namespace hyouka_api.Feature.FileManger
{
  public class ListActionCommand : IRequest<FileResultEnvelope>
  {
    public string Path { get; set; }

    public ListActionCommand(string path)
    {
      this.Path = path;
    }
  }

  class QueryHandler : IRequestHandler<ListActionCommand, FileResultEnvelope>
  {

    private IFileProvider provider;
    private IPathMapper mapper;

    public QueryHandler(IFileProvider provider, IPathMapper mapper)
    {
      this.provider = provider;
      this.mapper = mapper;
    }

    public Task<FileResultEnvelope> Handle(ListActionCommand request, CancellationToken cancellationToken)
    {

      var directoryContent = this.provider.GetDirectoryContents($"{mapper.BasePath}/{request.Path}");
      List<FileData> files = new List<FileData>();

      foreach (var item in directoryContent)
      {
        if (!item.Exists)
        {
          throw new InvalideFileOperationException("File is not exist");
        }

        files.Add(
            new FileData(item.Name, item.Length.ToString(), item.LastModified.ToString("yyyy-MM-dd HH:mm:ss"),
            item.IsDirectory ? "dir" : "file", "----------"));
      }
      return Task.FromResult(new FileResultEnvelope(files));
    }
  }
}