using CSBI.Test.Application.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CSBI.Test.API.Filters
{
    public class ExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<ExceptionFilter> _logger;

        public ExceptionFilter(ILogger<ExceptionFilter> logger)
            => _logger = logger;

        public void OnException(ExceptionContext context)
        {
            var ex = context.Exception;

            var response = new ExceptionResponse();

            var result = new ObjectResult(response);

            switch (ex)
            {
                case DomainException:
                    response.Type = ExceptionType.DomainError;
                    response.Message = ex.Message;
                    result.StatusCode = StatusCodes.Status400BadRequest;
                    _logger.LogWarning(ex.Message);
                    break;
                default:
                    response.Type = ExceptionType.InternalError;
                    response.Message = "Внутренняя ошибка системы";
                    result.StatusCode = StatusCodes.Status500InternalServerError;
                    _logger.LogError(ex, ex.Message);
                    break;

            }
            response.Message = context.Exception.Message;

            context.Result = result;
        }

        private class ExceptionResponse
        {
            [JsonConverter(typeof(StringEnumConverter))]
            public ExceptionType Type { get; set; }

            public string Message { get; set; }
        }

        private enum ExceptionType
        {
            InternalError,
            DomainError
        }
    }
}
