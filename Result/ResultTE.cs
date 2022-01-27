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
 }
}