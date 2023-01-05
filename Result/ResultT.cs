using static ResultType.ResultCommonLogic;

namespace ResultType
{
  public partial struct Result<T> : IResult<T>
  {
    public bool IsFailure { get; }

    public bool IsSuccess => !IsFailure;

    private readonly string _error;

    public string Error => GetErrorWithSuccessGuard(IsFailure, _error);

    private readonly T _value;

    public T Value => IsSuccess ? _value : throw new ResultFailureException(Error);

    internal Result(bool isFailure, string error, T value)
    {
      IsFailure = ErrorStateGuard(isFailure, error);
      _error = error;
      _value = value;
    }

    public static implicit operator Result<T>(T value)
    {
      if (value is IResult<T> result)
      {
        string resultError = result.IsFailure ? result.Error : null;
        T resultValue = result.IsSuccess ? result.Value : default;

        return new Result<T>(result.IsFailure, resultError, resultValue);
      }

      return Result.Success(value);
    }

    public static implicit operator Result(Result<T> result) => result.IsSuccess ? Result.Success() : Result.Failure(result.Error);

    public static implicit operator UnitResult<string>(Result<T> result) => result.IsSuccess ? UnitResult.Success<string>() : UnitResult.Failure(result.Error);

    public static implicit operator bool(Result<T> result) => result.IsSuccess;

    public static explicit operator T(Result<T> result) => result.IsSuccess ? result.Value : throw new ResultFailureException(result.Error);
  }
}