namespace ResultType
{
  public partial struct Result
  {
    public static Result Failure(string error) => new Result(true, error);

    public static Result<T> Failure<T>(string error) => new Result<T>(true, error, default);

    public static Result<T, E> Failure<T, E>(E error) => new Result<T, E>(true, error, default);
  }
}