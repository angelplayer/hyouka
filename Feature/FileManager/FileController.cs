

using System.Collections;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using System;
using Microsoft.Extensions.FileProviders;

namespace hyouka_api
{
  public class FileData
  {
    public string Name { get; set; }
    public string Rights { get; set; }
    public string Size { get; set; }
    public string Date { get; set; }
    public string Type { get; set; }

    public FileData(string name, string size, string date, string type, string right)
    {
      this.Name = name;
      this.Size = size;
      this.Date = date;
      this.Type = type;
      this.Rights = right;
    }
  }

  public class ResultEnvelope
  {
    public List<FileData> Resutl { get; set; }

    public ResultEnvelope(List<FileData> data)
    {
      this.Resutl = data;
    }
  }

  public class ActionCommand
  {
    public string Action { get; set; }
    public string Path { get; set; }
  }

  [Route("api/file")]
  public class FileController : Controller
  {
    private IMediator mediator;

    public FileController(IMediator mediator)
    {
      this.mediator = mediator;


    }

    [HttpPost]
    public async Task<ResultEnvelope> HandleAction([FromBody] ActionCommand command)
    {

      return await this.mediator.Send(new ListActionCommand(command.Path));
    }
  }


  // listing
  public class ListActionCommand : IRequest<ResultEnvelope>
  {
    public string Path { get; set; }

    public ListActionCommand(string path)
    {
      this.Path = path;
    }
  }

  class QueryHandler : IRequestHandler<ListActionCommand, ResultEnvelope>
  {

    private IFileProvider provider;
    IHostingEnvironment env;

    public QueryHandler(IFileProvider provider, IHostingEnvironment env)
    {
      this.provider = provider;
      this.env = env;
    }

    public Task<ResultEnvelope> Handle(ListActionCommand request, CancellationToken cancellationToken)
    {

      var directoryContent = this.provider.GetDirectoryContents($"/Files/{request.Path}");
      List<FileData> files = new List<FileData>();

      foreach (var item in directoryContent)
      {
        if (item.Exists)
        {
          files.Add(
            new FileData(item.Name, item.Length.ToString(), item.LastModified.ToString(),
            item.IsDirectory ? "dir" : "file", "drwxr-xr-x"));
        }
      }
      return Task.FromResult(new ResultEnvelope(files));
    }
  }

}