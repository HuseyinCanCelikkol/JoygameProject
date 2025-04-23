using JoygameProject.Application.Constants;
using System.Net;

namespace JoygameProject.Application.Wrappers
{
    public class ServiceResponse<T>()
    {
        public HttpStatusCode Status { get; set; }
        public string? Message { get; set; }
        public T? Result { get; set; }
        public ServiceResponse<T> Create()
        {
            return new ServiceResponse<T>();
        }
        public ServiceResponse<T> Success(T? result)
        {
            return new ServiceResponse<T>()
            {
                Status = HttpStatusCode.OK,
                Message = ResponseMessages.Success,
                Result = result
            };
        }
        public ServiceResponse<T> Forbidden(T? result)
        {
            return new ServiceResponse<T>()
            {
                Status = HttpStatusCode.Forbidden,
                Message = ResponseMessages.Forbidden,
                Result = result
            };
        }
        public ServiceResponse<T> NotFound(string? message = null)
        {
            return new ServiceResponse<T>()
            {
                Status = HttpStatusCode.NotFound,
                Message = ResponseMessages.NotFound,
                Result = default
            };
        }
        public ServiceResponse<T> BadRequest(string? message = null)
        {
            return new ServiceResponse<T>()
            {
                Status = HttpStatusCode.BadRequest,
                Message = ResponseMessages.BadRequest, 
                Result = default
            };
        }
        public ServiceResponse<T> Error(string message, HttpStatusCode status = HttpStatusCode.InternalServerError)
        {
            return new ServiceResponse<T>()
            {
                Status = status,
                Message = message,
                Result = default
            };
        }
    }
}
