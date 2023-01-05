namespace ResultType
{
  public partial struct Result
  {
    public static Result Success() => new(false, null);

    public static Result<T> Success<T>(T value) => new(false, null, value);

    public static Result<T, E> Success<T, E>(T value) => new(false, default, value);

    public static UnitResult<E> Success<E>() => new(false, default);
  }
}