using System.Text.Json;

namespace socialmedia.Middleware
{
    public class globalExceptionHandler
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<globalExceptionHandler> _logger;
        private readonly IHostEnvironment _env;

        public globalExceptionHandler(RequestDelegate next, ILogger<globalExceptionHandler> logger, IHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Response.ContentType = "application/json";

                var response = _env.IsDevelopment()
                    ? new ApiException
                    {
                        StatusCode = context.Response.StatusCode,
                        Message = ex.Message,
                        Details = ex.StackTrace?.ToString()
                    }
                    : new ApiException
                    {
                        StatusCode = context.Response.StatusCode,
                        Message = "Internal Server Error",
                        Details = null
                    };

                var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                var json = JsonSerializer.Serialize(response, options);
                await context.Response.WriteAsync(json);
            }
        }

        public class ApiException
        {
            public int StatusCode { get; set; }
            public string Message { get; set; }
            public string? Details { get; set; }
        }
    }
}
