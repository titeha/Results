namespace ResultType
{
  public partial struct Result
  {
    public static Result Failure(string? error) => new(true, error);

    public static Result<T> Failure<T>(string? error) => new(true, error, default);

    public static Result<T, E> Failure<T, E>(E? error) => new(true, error, default);
  }
}