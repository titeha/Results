using static ResultType.ResultCommonLogic;

namespace ResultType
{
 public partial struct Result<T, E> : IResult<T, E>
 {
  public bool IsFailure { get; }

  public bool IsSuccess => !IsFailure;

  private readonly E _error;

  public E Error => GetErrorWithSuccessGuard(IsFailure, _error);

  private readonly T _value;

  public T Value => IsSuccess ? _value : throw new ResultFailureException<E>(_error);

  internal Result(bool isFailure, E error, T value)
  {
   IsFailure = ErrorStateGuard(isFailure, error);
   _error = error;
   _value = value;
  }

  public static implicit operator Result<T, E>(T value)
  {
   if (value is IResult<T, E> result)
   {
	E resultError = result.IsFailure ? result.Error : default;
	T resultValue = result.IsSuccess ? result.Value : default;

	return new Result<T, E>(result.IsFailure, resultError, resultValue);
   }

   return Result.Success<T, E>(value);
  }

  public static implicit operator Result<T, E>(E error)
  {
   if (error is IResult<T, E> result)
   {
	E resultError = result.IsFailure ? result.Error : default;
	T resultValue = result.IsSuccess ? result.Value : default;

	return new Result<T, E>(result.IsFailure, resultError, resultValue);
   }

   return Result.Failure<T, E>(error);
  }

  public static implicit operator UnitResult<E>(Result<T, E> result) => result.IsSuccess ? UnitResult.Success<E>() : UnitResult.Failure(result.Error);

  public static implicit operator bool(Result<T, E> result) => result.IsSuccess;

  public static explicit operator T(Result<T, E> result) => result.IsSuccess ? result.Value : throw new ResultFailureException<E>(result.Error);
 }
}