using System;
using System.Net;
using System.Threading.Tasks;
using hyouka_api.Feature.FileManger;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace hyouka_api.Infrastructure
{
  public class ErrorHandlingMiddleware
  {
    private readonly RequestDelegate next;
    // private readonly ILogger<ErrorHandlingMiddleware> _logger;
    // private readonly IStringLocalizer<ErrorHandlingMiddleware> _localizer;

    public ErrorHandlingMiddleware(
        RequestDelegate next
        /* ,IStringLocalizer<ErrorHandlingMiddleware> localizer,
        ILogger<ErrorHandlingMiddleware> logger */
        )
    {
      this.next = next;
      // this._logger = logger;
      // this._localizer = localizer;
    }

    public async Task Invoke(HttpContext context)
    {
      try
      {
        await next(context);
      }
      catch (Exception ex)
      {
        await HandleExceptionAsync(context, ex);
      }
    }

    private static async Task HandleExceptionAsync(
        HttpContext context,
        Exception exception
        /*,ILogger<ErrorHandlingMiddleware> logger,
        IStringLocalizer<ErrorHandlingMiddleware> localizer*/)
    {
      object errorsEnvelope = null;
      context.Response.ContentType = "application/json";

      switch (exception)
      {
        case InvalideFileOperationException fe:
          context.Response.StatusCode = (int)fe.Code;
          var result = new FileActionResult() { Error = fe.Errors, Success = false };
          errorsEnvelope = new { result };
          break;
        case RestException re:
          errorsEnvelope = new { errors = re.Errors };
          context.Response.StatusCode = (int)re.Code;
          break;
        case Exception e:
          errorsEnvelope = new { errors = string.IsNullOrWhiteSpace(e.Message) ? "Error" : e.Message };
          context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
          break;
      }

      var response = JsonConvert.SerializeObject(errorsEnvelope).ToLower();
      await context.Response.WriteAsync(response);
    }
  }
}