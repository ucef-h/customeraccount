using Customer.Application;
using Customer.Domain.Exceptions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Net;

namespace Customer.Api
{
    public class HttpGlobalExceptionFilter : IExceptionFilter
    {
        private readonly IWebHostEnvironment env;
        private readonly ILogger<HttpGlobalExceptionFilter> logger;

        public HttpGlobalExceptionFilter(IWebHostEnvironment env, ILogger<HttpGlobalExceptionFilter> logger)
        {
            this.env = env;
            this.logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            logger.LogError(new EventId(context.Exception.HResult),
                context.Exception,
                context.Exception.Message);

            var response = new ResponseBase<object>(false);
            switch (context.Exception)
            {
                case CustomerDomainException e:
                    {
                        response.Errors = new[] { e.Message };
                        context.HttpContext.Response.StatusCode = (int)HttpStatusCode.OK;
                        context.Result = new ResponseServerErrorObjectResult(response);
                        break;
                    }
                default:
                    {
                        response.Errors = new[] { "Internal Error Occurred" };
                        if (env.IsDevelopment() || env.IsStaging())
                        {
                            response.Response = new { context.Exception.Message, context.Exception.StackTrace };
                        }

                        context.Result = new InternalServerErrorObjectResult(response);
                        break;
                    }
            }

            context.ExceptionHandled = true;
        }
    }


    public class ResponseServerErrorObjectResult : ObjectResult
    {
        public ResponseServerErrorObjectResult(object error)
            : base(error)
        {
            StatusCode = StatusCodes.Status200OK;
        }
    }

    public class InternalServerErrorObjectResult : ObjectResult
    {
        public InternalServerErrorObjectResult(object error)
            : base(error)
        {
            StatusCode = StatusCodes.Status500InternalServerError;
        }
    }
}