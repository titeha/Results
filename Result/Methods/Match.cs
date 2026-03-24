namespace ResultType;

public readonly partial struct Result
{
  public readonly TResult Match<TResult>(Func<TResult> onSuccess, Func<string?, TResult> onFailure) =>
    IsSuccess ? onSuccess() : onFailure(Error);
}

public readonly partial struct Result<T>
{
  public readonly TResult Match<TResult>(Func<T?, TResult> onSuccess, Func<string?, TResult> onFailure) =>
    IsSuccess ? onSuccess(Value) : onFailure(Error);
}

public readonly partial struct Result<T, E>
{
  public readonly TResult Match<TResult>(Func<T?, TResult> onSuccess, Func<E?, TResult> onFailure) =>
    IsSuccess ? onSuccess(Value) : onFailure(Error);
}

public readonly partial struct UnitResult<E>
{
  public readonly TResult Match<TResult>(Func<TResult> onSuccess, Func<E?, TResult> onFailure) =>
    IsSuccess ? onSuccess() : onFailure(Error);
}
