using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApiAuthors.Filters
{
    public class ExceptionFilter : ExceptionFilterAttribute
    {
        private readonly ILogger<ExceptionFilter> _exeptionFilter;
        public ExceptionFilter(ILogger<ExceptionFilter> exceptionFilter)
        {
            this._exeptionFilter = exceptionFilter;
        }

        public override void OnException(ExceptionContext context)
        {
            _exeptionFilter.LogError(context.Exception, context.Exception.Message);
            base.OnException(context);
        }
    }
}
