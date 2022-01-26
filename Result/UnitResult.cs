using static Result.Internals.ResultCommonLogic;

namespace Result
{
 public partial struct UnitResult<E> : IUnitResult<E>
 {
  public bool IsFailure { get; }

  public bool IsSuccess => !IsFailure;

  private readonly E _error;

  public E Error => GetErrorWithSuccessGuard(IsFailure, _error);

  internal UnitResult(bool isFailure, E error)
  {
   IsFailure = ErrorStateGuard(isFailure, _error);
   _error = error;
  }
 }
}