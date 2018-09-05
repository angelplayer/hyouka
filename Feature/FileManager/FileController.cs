

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

  public class FileActionResult
  {
    public bool Success { get; set; }
    public String Error { get; set; }
  }

  public class FileResultEnvelope
  {
    public List<FileData> Resutl { get; set; }

    public FileResultEnvelope(List<FileData> data)
    {
      this.Resutl = data;
    }
  }

  public class ActionResultEnvelope
  {
    public FileActionResult Result { get; set; }

    public ActionResultEnvelope(FileActionResult result)
    {
      this.Result = result;
    }
  }

  public class ActionCommand
  {
    public string Action { get; set; }
    public string Path { get; set; }
    public string NewPath { get; set; }
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
    public async Task<ActionResultEnvelope> HandleAction([FromBody] ActionCommand command)
    {
      return await this.mediator.Send(new CreateFolderCommand(command.NewPath));
    }
  }


  #region List file
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
    IHostingEnvironment env;

    public QueryHandler(IFileProvider provider, IHostingEnvironment env)
    {
      this.provider = provider;
      this.env = env;
    }

    public Task<FileResultEnvelope> Handle(ListActionCommand request, CancellationToken cancellationToken)
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
      return Task.FromResult(new FileResultEnvelope(files));
    }
  }
  #endregion

  #region Create folder

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
    private IFileProvider provider;
    private IHostingEnvironment env;

    public CreateFolderHandler(IFileProvider provider, IHostingEnvironment env)
    {
      this.env = env;
      this.provider = provider;
    }

    public Task<ActionResultEnvelope> Handle(CreateFolderCommand request, CancellationToken cancellationToken)
    {
      var path = Path.Combine(this.env.WebRootPath, "Files", request.NewPath.TrimStart(new char[] { ' ', '/' }));

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

  #endregion


}