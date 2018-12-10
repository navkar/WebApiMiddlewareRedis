using CustomHeaderMiddleware.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomHeaderMiddleware.Middleware
{
    public class ParseHeadersMiddleware
    {
        private readonly RequestDelegate next;

        public ParseHeadersMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context, IHeaders headers, IOptions<MobileConfig> settings)
        {
            try
            {
                var bundleId = context.Request.Headers.FirstOrDefault(x => x.Key == "BundleId").Value.ToString();
                if (string.IsNullOrEmpty(bundleId))
                {
                    context.Response.StatusCode = 403;
                    await context.Response.WriteAsync("Required header - bundleId is missing");
                    return;
                }
                else
                {
                    MobileConfig config = settings.Value;
                    var bundleIds = config.BundleIds.Split(",");

                    if (!bundleIds.Contains(bundleId))
                    {
                        context.Response.StatusCode = 403;
                        await context.Response.WriteAsync("Invalid!");
                        return;
                    }
                }

                headers.BundleId = bundleId;
            }
            catch (Exception)
            {
            }

            await next(context);
        }
    }
}
