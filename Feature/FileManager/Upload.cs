using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using hyouka_api;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;
using Newtonsoft.Json;

namespace hyouka_api.Feature.FileManger
{

    public class FileUploadModel
    {
        public string Destination { get; set; }
        public IFormFileCollection Files { get; set; }
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
        private IPathMapper mapper;

        public UploadActionHandler(IPathMapper mapper)
        {
            this.mapper = mapper;
        }

        public async Task<ActionResultEnvelope> Handle(UploadFileCommand request, CancellationToken cancellationToken)
        {
            var path = mapper.MapPath(request.UploadModel.Destination);
            if (request.UploadModel.Files != null)
            {
                try
                {
                    foreach (var item in request.UploadModel.Files)
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
            var envelope = new ActionResultEnvelope(new FileActionResult()
            {
                Success = true,
                Error = null
            });

            return envelope;
        }
    }
}
