
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.FileProviders;

namespace hyouka_api.Feature.FileManger
{
  public class GetContentActionCommand : IRequest<ContentEnvelope>
  {
    public string Item { get; set; }

    public GetContentActionCommand(string item)
    {
      this.Item = item;
    }
  }

  public class GetContentHandler : IRequestHandler<GetContentActionCommand, ContentEnvelope>
  {
    private IPathMapper mapper;

    public GetContentHandler(IPathMapper mapper)
    {
      this.mapper = mapper;
    }

    public async Task<ContentEnvelope> Handle(GetContentActionCommand request, CancellationToken cancellationToken)
    {
      var path = mapper.MapPath(request.Item);
      var content = await File.ReadAllTextAsync(path);
      var envelope = new ContentEnvelope();
      envelope.Result = content;

      return envelope;
    }
  }
}