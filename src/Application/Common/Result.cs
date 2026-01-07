namespace Application.Common
{
    public class Result<T>
    {
        public bool IsSuccess { get; }
        public bool IsFailure => !IsSuccess;
        public T? Data { get; }
        public string? ErrorMessage { get; }
        public IEnumerable<string> Errors { get; set; } = new List<string>();

        protected Result(bool success, T? value, string? error)
        {
            IsSuccess = success;
            Data = value;
            ErrorMessage = error;
        }

        protected Result(bool success, T? value, IEnumerable<string> errors)
        {
            IsSuccess = success;
            Data = value;
            Errors = errors;
        }

        public static Result<T> Success(T value)
            => new(true, value, string.Empty);

        public static Result<T> Failure(string error)
            => new(false, default, error);

        public static Result<T> Failure(IEnumerable<string> errors)
          => new(false, default, errors);
    }

    public class Result
    {
        public bool IsSuccess { get; }
        public bool IsFailure => !IsSuccess;
        public string? ErrorMessage { get; }

        protected Result(bool success, string? error)
        {
            IsSuccess = success;
            ErrorMessage = error;
        }

        public static Result Success()
            => new(true, null);

        public static Result Failure(string error)
            => new(false, error);
    }
}