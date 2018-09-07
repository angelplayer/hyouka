

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
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.StaticFiles;

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
    public Object Error { get; set; }
  }

  public class FileResultEnvelope
  {
    public List<FileData> Result { get; set; }

    public FileResultEnvelope(List<FileData> data)
    {
      this.Result = data;
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

  public class ContentEnvelope
  {
    public string Result { get; set; }
  }


  public class ActionCommand
  {
    public string Action { get; set; }
    public string Path { get; set; }
    public string NewPath { get; set; }
    public string Item { get; set; }
  }


  [Route("api/file")]
  public class FileController : Controller
  {
    private IMediator mediator;

    public FileController(IMediator mediator)
    {
      this.mediator = mediator;
    }

    [HttpGet]
    public async Task<FileContentResult> download([FromQuery]string action, [FromQuery] string path)
    {
      //download/ preview a file
      return await this.mediator.Send(new SingleDownloadCommnad(path));
    }

    // POST: /api/file/list
    [HttpPost("list")]
    public async Task<FileResultEnvelope> Navigate([FromBody] ActionCommand command)
    {
      return await this.mediator.Send(new ListActionCommand(command.Path));
    }

    // POST: /api/file/command
    [HttpPost("command")]
    public async Task<ActionResultEnvelope> FileAction([FromBody] ActionCommand command)
    {
      IRequest<ActionResultEnvelope> request = null;

      if ("createFolder".Equals(command.Action))
      {
        request = new CreateFolderCommand(command.NewPath);
      }
      else
      {
        throw new InvalideFileOperationException("command is not found");
      }
      return await this.mediator.Send(request);
    }

    // PostL /api/file/content
    [HttpPost("content")]
    public async Task<ContentEnvelope> GetContent([FromBody]ActionCommand command)
    {
      return await this.mediator.Send(new GetContentActionCommand(command.Item));
    }


    // POST: /api/file/upload
    [HttpPost("upload")]
    public async Task<ActionResultEnvelope> Upload([FromForm]FileUploadModel uploadModel)
    {
      return await this.mediator.Send(new UploadFileCommand(uploadModel));
    }
  }

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
    private IHostingEnvironment env;
    private IFileProvider provider;

    public SingleDownloadHandler(IHostingEnvironment env, IFileProvider provider)
    {
      this.env = env;
      this.provider = provider;
    }

    public async Task<FileContentResult> Handle(SingleDownloadCommnad request, CancellationToken cancellationToken)
    {
      var path = Path.Combine(this.env.WebRootPath, "Files", request.Path.TrimStart(new char[] { ' ', '/' }));
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

    private string GetContentType(string path)
    {
      var types = GetMimeTypes();
      var ext = Path.GetExtension(path).ToLowerInvariant();
      return types[ext];
    }

    private Dictionary<string, string> GetMimeTypes()
    {
      return new Dictionary<string, string>
            {
                {".txt", "text/plain"},
                {".pdf", "application/pdf"},
                {".doc", "application/vnd.ms-word"},
                {".docx", "application/vnd.ms-word"},
                {".xls", "application/vnd.ms-excel"},
                {".xlsx", "application/vnd.openxmlformatsofficedocument.spreadsheetml.sheet"},
                {".png", "image/png"},
                {".jpg", "image/jpeg"},
                {".jpeg", "image/jpeg"},
                {".gif", "image/gif"},
                {".csv", "text/csv"}
            };
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
        if (!item.Exists)
        {
          throw new InvalideFileOperationException("File is not exist");
        }

        files.Add(
            new FileData(item.Name, item.Length.ToString(), item.LastModified.ToString(),
            item.IsDirectory ? "dir" : "file", "drwxr-xr-x"));
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

  #region Read content 
  public class GetContentActionCommand : IRequest<ContentEnvelope>
  {
    public string Item { get; set; }

    public GetContentActionCommand(String item)
    {
      this.Item = item;
    }
  }

  public class GetContentHandler : IRequestHandler<GetContentActionCommand, ContentEnvelope>
  {
    private IFileProvider provider;
    private IHostingEnvironment env;

    public GetContentHandler(IFileProvider provider, IHostingEnvironment env)
    {
      this.env = env;
      this.provider = provider;
    }

    public async Task<ContentEnvelope> Handle(GetContentActionCommand request, CancellationToken cancellationToken)
    {
      var path = Path.Combine(this.env.WebRootPath, "Files", request.Item.TrimStart(new char[] { ' ', '/' }));
      var content = await File.ReadAllTextAsync(path);
      var envelope = new ContentEnvelope();
      envelope.Result = content;

      return envelope;
    }
  }
  #endregion

  #region Upload file
  public class FileUploadModel
  {
    public string Destination { get; set; }
    public IFormCollection Files { get; set; }
  }


  public class UploadFileCommand : IRequest<ActionResultEnvelope>
  {
    public FileUploadModel UploadModel { get; set; }

    public UploadFileCommand(FileUploadModel uploadModel)
    {
      this.UploadModel = uploadModel;
    }
  }



  public class UploadActionHandler : IRequestHandler<UploadFileCommand, ActionResultEnvelope>
  {

    private IFileProvider provider;
    private IHostingEnvironment env;

    public UploadActionHandler(IFileProvider provider, IHostingEnvironment env)
    {
      this.provider = provider;
      this.env = env;
    }

    public async Task<ActionResultEnvelope> Handle(UploadFileCommand request, CancellationToken cancellationToken)
    {
      var path = Path.Combine(this.env.WebRootPath, "Files", request.UploadModel.Destination.TrimStart(new char[] { ' ', '/' }));
      if (request.UploadModel.Files.Count > 0)
      {
        try
        {
          foreach (var item in request.UploadModel.Files.Files)
          {
            using (var stream = new FileStream(Path.Combine(path, item.FileName), FileMode.Create))
            {
              await item.CopyToAsync(stream);
            }
          }
        }
        catch (Exception ex)
        {
          throw new InvalideFileOperationException(ex.Message);
        }
      }
      //move it to webroot content folder
      var envelope = new ActionResultEnvelope(new FileActionResult()
      {
        Success = true,
        Error = null
      });

      return envelope;
    }
  }
  #endregion
}