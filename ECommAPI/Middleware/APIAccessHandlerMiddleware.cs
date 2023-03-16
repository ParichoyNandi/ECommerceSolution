using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace ECommAPI.Middleware
{
    public class APIAccessHandlerMiddleware
    {
        private RequestDelegate _next;
        private string _apikey;
        //private readonly string _apikey = "pq78dndj#jfmsm$gyys";
        public APIAccessHandlerMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            _apikey = configuration.GetSection("APIKey").Value.ToString();
        }

        public async Task Invoke(HttpContext context)
        {
            bool validkey = false;

            if (context.Request.Headers.ContainsKey("X-API-KEY"))
            {
                if (context.Request.Headers["X-API-KEY"].Equals(_apikey))
                {
                    validkey = true;
                }
            }
            if (!validkey)
            {
                context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                await context.Response.WriteAsync("Access Denied!").ConfigureAwait(false);
            }
            else
            {
                await _next.Invoke(context).ConfigureAwait(false);
            }
        }
    }

    public static class APIKeyHandlerExtension
    {
        public static IApplicationBuilder APIKeyHandler(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<APIAccessHandlerMiddleware>();
        }
    }
}
