namespace MovieApi.Responses
{
    public class Result<T> : Result
    {
        public T Data { get; set; }

        public Result(bool isSuccess, Error error, T? data) : base(isSuccess, error)
        {
            Data = data;
        }
    }
}