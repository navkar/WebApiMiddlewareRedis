using CustomHeaderMiddleware.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace CustomHeaderMiddleware.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context /* other dependencies */)
        {
            try
            {
                await next(context);
            }
            catch (Exception exceptionObj)
            {
                await HandleExceptionAsync(context, exceptionObj);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, HttpStatusCodeException exception)
        {
            string result = null;
            context.Response.ContentType = "application/json";
            if (exception is HttpStatusCodeException)
            {
                result = new ApiResponse<string>() { Message = exception.Message, StatusCode = (int) exception.StatusCode }.ToString();
                context.Response.StatusCode = (int)exception.StatusCode;
            }
            else
            {
                result = new ApiResponse<string>() { Message = "Runtime Error", StatusCode = (int) HttpStatusCode.BadRequest }.ToString();
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            }
            return context.Response.WriteAsync(result);
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            var result = new ApiResponse<string>() { Message = exception.Message, StatusCode = (int) HttpStatusCode.InternalServerError }.ToString();
            return context.Response.WriteAsync(result);
        }
    }
}
