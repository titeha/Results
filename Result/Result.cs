using static ResultType.ResultCommonLogic;

namespace ResultType
{
  public partial struct Result : IResult
  {
    public bool IsFailure { get; }

    public bool IsSuccess => !IsFailure;

    private readonly string _error;

    public string Error => GetErrorWithSuccessGuard(IsFailure, _error);

    public Result(bool isFailure, string error)
    {
      IsFailure = ErrorStateGuard(isFailure, error);
      _error = error;
    }

    public static implicit operator UnitResult<string>(Result result)
    {
      if (result.IsSuccess)
        return UnitResult.Success<string>();
      else
        return UnitResult.Failure(result.Error);
    }

    public static implicit operator bool(Result result) => result.IsSuccess;
  }
}