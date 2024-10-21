namespace MovieApi.Responses
{
    public class Result
    {
        public Result(bool isSuccess, string message, Error error = null)
        {
            IsSuccess = isSuccess;
            Error = error;
            Message = message;
        }

        public string Message { get; }

        public bool IsSuccess { get; }
        
        public Error Error { get; }

        public static Result Success() => new(true, Error.None);

        public static Result Failure(Error error) => new(false, error);

        public static Result<T> Success<T>(T data) => new(true, Error.None, data);

        public static Result<T> Failure<T>(Error error) => new(false, error, default);
    }
}