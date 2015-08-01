using System.Web.Http.Filters;

namespace TinyFeed.Filters
{
    public class ExceptionLoggingFilter : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            UnhandledExceptionLogger.Log.Error(actionExecutedContext.Exception.Message, actionExecutedContext.Exception);
        }
    }
}