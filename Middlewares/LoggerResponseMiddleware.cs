namespace WebApiAuthors.Middlewares
{
    public class LoggerResponseMiddleware
    {
        private readonly RequestDelegate _delegate;
        private readonly ILogger<LoggerResponseMiddleware> _logger;

        public LoggerResponseMiddleware(RequestDelegate next, ILogger<LoggerResponseMiddleware> log)
        {
            this._delegate = next;
            this._logger = log;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            using (var ms = new MemoryStream())
            {
                var bodyResponse = httpContext.Response.Body;
                httpContext.Response.Body = ms;

                await _delegate(httpContext);

                ms.Seek(0, SeekOrigin.Begin);
                string resp = new StreamReader(ms).ReadToEnd();
                ms.Seek(0, SeekOrigin.Begin);

                await ms.CopyToAsync(bodyResponse);
                httpContext.Response.Body = bodyResponse;

                _logger.LogInformation(resp);
            }
        }
    }
}
