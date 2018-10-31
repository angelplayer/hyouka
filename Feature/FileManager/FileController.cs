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
using NSwag.Annotations;

namespace hyouka_api.Feature.FileManger
{
    [Route("api/file")]
    public class FileController : Controller
    {
        private IMediator mediator;

        public FileController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        // GET: /api/file?action=download&path=/public/list.txt
        [HttpGet]
        public async Task<FileContentResult> Download([FromQuery]string action, [FromQuery] string path)
        {
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
            else if ("rename".Equals(command.Action))
            {
                request = new RenameCommand(command.Item, command.newItemPath);
            }
            else if ("move".Equals(command.Action))
            {
                request = new MoveCommand(command.Items, command.NewPath);
            }
            else if ("remove".Equals(command.Action))
            {
                request = new RemoveCommand(command.Items);
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
        public async Task<ActionResultEnvelope> Upload([SwaggerFile]FileUploadModel uploadModel)
        {
            return await this.mediator.Send(new UploadFileCommand(uploadModel));
        }
    }
}