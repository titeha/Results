using static ResultType.ResultCommonLogic;

namespace ResultType
{
  public partial struct UnitResult<E> : IUnitResult<E?>
  {
    public bool IsFailure { get; }

    public readonly bool IsSuccess => !IsFailure;

    private readonly E? _error;

    public readonly E? Error => GetErrorWithSuccessGuard(IsFailure, _error);

    internal UnitResult(bool isFailure, E? error)
    {
      IsFailure = ErrorStateGuard(isFailure, error);
      _error = error;
    }

    public static implicit operator UnitResult<E?>(E error)
    {
      if (error is IUnitResult<E?> result)
      {
        E? resultError = result.IsFailure ? result.Error : default;
        return new UnitResult<E?>(result.IsFailure, resultError);
      }

      return UnitResult.Failure(error);
    }

    public static implicit operator bool(UnitResult<E?> result) => result.IsSuccess;
  }

  public struct UnitResult
  {
    public static UnitResult<E?> Failure<E>(E error) => new(true, error);

    public static UnitResult<E?> Success<E>() => Result.Success<E?>();
  }
}