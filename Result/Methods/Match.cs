namespace ResultType;

public readonly partial struct Result
{
  /// <summary>
  /// Сопоставляет успешный и неуспешный результат с обычным значением.
  /// </summary>
  public readonly T Match<T>(Func<T> onSuccess, Func<string?, T> onFailure) =>
    IsSuccess ? onSuccess() : onFailure(Error);
}

public readonly partial struct Result<T>
{
  /// <summary>
  /// Сопоставляет успешный и неуспешный результат с обычным значением.
  /// </summary>
  public readonly TResult Match<TResult>(Func<T?, TResult> onSuccess, Func<string?, TResult> onFailure) =>
    IsSuccess ? onSuccess(Value) : onFailure(Error);
}

public readonly partial struct Result<T, E>
{
  /// <summary>
  /// Сопоставляет успешный и неуспешный результат с обычным значением.
  /// </summary>
  public readonly TResult Match<TResult>(Func<T?, TResult> onSuccess, Func<E?, TResult> onFailure) =>
    IsSuccess ? onSuccess(Value) : onFailure(Error);
}

public readonly partial struct UnitResult<E>
{
  /// <summary>
  /// Сопоставляет успешный и неуспешный результат с обычным значением.
  /// </summary>
  public readonly TResult Match<TResult>(Func<TResult> onSuccess, Func<E?, TResult> onFailure) =>
    IsSuccess ? onSuccess() : onFailure(Error);
}
