using JoygameProject.Application.Extensions;
using JoygameProject.Application.Wrappers;
using MediatR;
using MediatR.Pipeline;
using Microsoft.Extensions.Logging;
using System.Net;
namespace JoygameProject.Application.Handlers
{
    public class ExceptionHandler<TRequest, TResponse, TException>(
        ILogger<ExceptionHandler<TRequest, TResponse, TException>> logger) : IRequestExceptionHandler<TRequest, TResponse, TException>
      where TRequest : IRequest<TResponse>
      where TException : Exception
    {
        public Task Handle(TRequest request, TException exception, RequestExceptionHandlerState<TResponse> state, CancellationToken cancellationToken)
        {
            if (exception is FluentValidation.ValidationException validationException)
            {
                var validationErrors = validationException.Errors
                    .Select(e => e.ErrorMessage)
                    .ToList();

                var validationResponse = Activator.CreateInstance<TResponse>();
                var vstatusPropertyName = nameof(ServiceResponse<object>.Status);
                var vmessagePropertyName = nameof(ServiceResponse<object>.Message);
                var vresultPropertyName = nameof(ServiceResponse<object>.Message);

                var statusProp = typeof(TResponse).GetProperty(vstatusPropertyName);
                var messageProp = typeof(TResponse).GetProperty(vmessagePropertyName);
                var resultProp = typeof(TResponse).GetProperty(vresultPropertyName);

                statusProp?.SetValue(validationResponse, HttpStatusCode.BadRequest);
                messageProp?.SetValue(validationResponse, "Doğrulama hatası oluştu.");
                resultProp?.SetValue(validationResponse, validationErrors);

                state.SetHandled(validationResponse);

                return Task.CompletedTask;
            }

            var exceptionModel = new
            {
                methodName = exception.TargetSite?.DeclaringType?.DeclaringType?.FullName,
                requestParameters = request.ToJson(),
                message = exception.Message,
                innerException = exception.InnerException?.Message,
                stackTrace = exception.StackTrace,
            };
            logger.LogError(exception, exceptionModel.ToJson());
            var response = Activator.CreateInstance<TResponse>();
            var statusPropertyName = nameof(ServiceResponse<object>.Status);
            var messagePropertyName = nameof(ServiceResponse<object>.Message);
            var statusProperty = typeof(TResponse).GetProperty(statusPropertyName);
            var messageProperty = typeof(TResponse).GetProperty(messagePropertyName);
            statusProperty.SetValue(response, HttpStatusCode.InternalServerError);
            messageProperty.SetValue(response, "Sistemde bir hata meydana geldi.");
            state.SetHandled(response);

            return Task.CompletedTask;
        }


    }
}
