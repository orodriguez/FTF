using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;
using FTF.Api.Exceptions;

namespace FTF.Web.Filters
{
    public class ValidationExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            if (context.Exception is ValidationException)
                context.Response = context.Request.CreateResponse(
                    HttpStatusCode.BadRequest, 
                    context.Exception.Message);
        }
    }
}