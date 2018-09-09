

using System.Net;
using hyouka_api.Infrastructure;

namespace hyouka_api
{
  public class InvalideFileOperationException : RestException
  {
    public InvalideFileOperationException(string message) : base(HttpStatusCode.InternalServerError, message)
    {
    }
  }
}