

using System.Collections;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using System;

namespace hyouka_api
{
    public class FileData
    {
        public string Name { get; set; }
        public string Rights { get; set; }
        public string Size { get; set; }
        public string Date { get; set; }
        public string Type { get; set; }

    }

    public class ResultEnvelope
    {
        public List<FileData> Resutl { get; set; }

        public ResultEnvelope(List<FileData> data) {
            this.Resutl = data;
        }
    }

    public class ActionCommand
    {
        public string Action { get; set; }
    }

    [Route("api/file")]
    public class FileController : Controller
    {
        public List<FileData> result = new List<FileData>();
        private readonly IHostingEnvironment enviroment;
        private IMediator mediator;

        public FileController(IHostingEnvironment env, IMediator mediator)
        {
            this.mediator = mediator;
            this.enviroment = env;
            this.enviroment.WebRootPath = System.IO.Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\Files");
        }

        [HttpPost]
        public async Task<ResultEnvelope> HandleAction([FromBody] ActionCommand command)
        {

            return await this.mediator.Send(new ListActionCommand("/"));
        }
    }


    // listing
    public class ListActionCommand : IRequest<ResultEnvelope>
    {
        private string path;

        public ListActionCommand(string path)
        {
            this.path = path;
        }
    }

    class QueryHandler : IRequestHandler<ListActionCommand, ResultEnvelope>
    {

        private IHostingEnvironment enviroment;
        private List<FileData> files = new List<FileData>();

        public QueryHandler(IHostingEnvironment hostEnvironment) {
            enviroment = hostEnvironment;

            // Set WebRootPath to wwwroot\Files directiry
            enviroment.WebRootPath =
                System.IO.Path.Combine(
                    Directory.GetCurrentDirectory(),
                    @"wwwroot\Files");
        }

        public Task<ResultEnvelope> Handle(ListActionCommand request, CancellationToken cancellationToken)
        {
            if(Directory.Exists(this.enviroment.WebRootPath)){
                ProcessDirectory(enviroment.WebRootPath);
            }

            return Task.FromResult(new ResultEnvelope(this.files));
        }

        private void ProcessDirectory(string target)
        {
            string[] filesEntry = Directory.GetFiles(target);
            foreach(var filename in filesEntry) {
                ProcessFile(filename);
            }

            // string[] subdirectoryEntry = Directory.GetDirectories(target);
            // foreach(var subdirecotry in subdirectoryEntry) {
            //     this.ProcessDirectory(subdirecotry);
            // }
        }

        private void ProcessFile(string path)
        {
            FileInfo fileInfo = new FileInfo(path);
            this.files.Add(new FileData(){ Name = fileInfo.Name, Size = fileInfo.Length.ToString(), Type = fileInfo.GetType().ToString(), 
            Date = fileInfo.CreationTimeUtc.ToString(), Rights = fileInfo.IsReadOnly ? "-rw-r--r--":"drwxr-xr-x" });
        }
    }

}