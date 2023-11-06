namespace ProductCatalog.Web.Configuration.Middleware
{
    public class ProductCatalogExceptionMiddleware : IMiddleware
    {
        private readonly ILogger<ProductCatalogExceptionMiddleware> _logger;

        public ProductCatalogExceptionMiddleware(ILogger<ProductCatalogExceptionMiddleware> logger)
        {
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occurred.");

                context.Response.Redirect($"/Home/Error?errorMessage={ex.Message}");
            }
        }
    }
}
