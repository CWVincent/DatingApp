namespace DatingApp.Api.Errors
{
    public class ApiException
    {
        public int StatusCode { get; set; }

        public string Message { get; set; }

        public string Details { get; set; }

        public ApiException(int statusCode = 500, string message = null, string details = null)
        {
            StatusCode = statusCode;
            Message = message;
            Details = details;
        }
    }
}
