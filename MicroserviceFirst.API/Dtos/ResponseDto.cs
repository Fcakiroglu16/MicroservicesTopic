using System.Net;
using System.Text.Json.Serialization;

namespace MicroserviceFirst.API.DTOs
{
    public struct NoData;

    public record ResponseDto<T>
    {
        private ResponseDto()
        {
        }

        public T? Data { get; init; }

        [JsonIgnore] public HttpStatusCode StatusCode { get; init; }


        public List<string>? Errors { get; init; }

        public static ResponseDto<T> Success(T data, HttpStatusCode statusCode)
        {
            return new ResponseDto<T> { Data = data, StatusCode = statusCode };
        }

        public static ResponseDto<T> Success(HttpStatusCode statusCode)
        {
            return new ResponseDto<T> { StatusCode = statusCode };
        }

        public static ResponseDto<T> SuccessWithOk(T data)
        {
            return new ResponseDto<T> { Data = data, StatusCode = HttpStatusCode.OK };
        }


        public static ResponseDto<T> SuccessWithNoContent()
        {
            return new ResponseDto<T> { StatusCode = HttpStatusCode.NoContent };
        }


        public static ResponseDto<T> Fail(List<string> errors, HttpStatusCode statusCode)
        {
            return new ResponseDto<T> { StatusCode = statusCode, Errors = errors };
        }

        public static ResponseDto<T> FailWithBadRequest(List<string> errors)
        {
            return new ResponseDto<T> { StatusCode = HttpStatusCode.BadRequest, Errors = errors };
        }

        public static ResponseDto<T> Fail(string error, HttpStatusCode statusCode)
        {
            return new ResponseDto<T> { StatusCode = statusCode, Errors = [error] };
        }

        public static ResponseDto<T> FailWithBadRequest(string error)
        {
            return new ResponseDto<T> { StatusCode = HttpStatusCode.BadRequest, Errors = [error] };
        }
    }
}