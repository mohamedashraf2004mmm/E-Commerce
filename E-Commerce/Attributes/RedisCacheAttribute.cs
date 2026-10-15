using E_Commerce.Application.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text;

namespace E_Commerce.Attributes
{
    public class RedisCacheAttribute : ActionFilterAttribute
    {
        private readonly int durationInSeconds;

        public RedisCacheAttribute(int durationInSeconds = 60)
        {
            this.durationInSeconds = durationInSeconds;
        }
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            //Get cache service from container
            var cacheService = context.HttpContext.RequestServices.GetRequiredService<ICacheService>();

            var cacheKey = CreateCacheKey(context.HttpContext.Request);
            var data = await cacheService.GetDataAsync(cacheKey);

            //if data exist in cache => get data from cache => skip endpoint

            if (!string.IsNullOrEmpty(data))
            {
                context.Result = new ContentResult()
                {
                    Content = data,
                    ContentType = "application/json",
                    StatusCode = StatusCodes.Status200OK
                };
                return;
            }

            //if data not exist in cache => hit the endpoint + store the result in cache if result is 200OK + data
           var executedContext = await next.Invoke();

           if(executedContext.Result is OkObjectResult { Value : not null} ok)
            {
               await cacheService.SetDataAsync(cacheKey, ok.Value , TimeSpan.FromSeconds(durationInSeconds));
            }

               
        }

        private static string CreateCacheKey(HttpRequest request)
        {
            var key = new StringBuilder();
            key.Append(request.Path);

            if (request.Query.Any())
            {
                key.Append('?');
                foreach(var(k,v) in request.Query.OrderBy(x => x.Key))
                {
                    key.Append(k).Append('=').Append(v).Append('&');
                }
            }
            return key.ToString();
        }
    }
}
