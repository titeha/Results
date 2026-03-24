namespace ResultType;

public readonly partial struct Result
{
  public readonly Result Tap(Action action)
  {
    ArgumentNullException.ThrowIfNull(action);

    if (IsSuccess)
      action();

    return this;
  }

  public readonly Result TapError(Action<string?> action)
  {
    ArgumentNullException.ThrowIfNull(action);

    if (IsFailure)
      action(Error);

    return this;
  }
}

public readonly partial struct Result<T>
{
  public readonly Result<T> Tap(Action<T?> action)
  {
    ArgumentNullException.ThrowIfNull(action);

    if (IsSuccess)
      action(Value);

    return this;
  }

  public readonly Result<T> TapError(Action<string?> action)
  {
    ArgumentNullException.ThrowIfNull(action);

    if (IsFailure)
      action(Error);

    return this;
  }
}

public readonly partial struct Result<T, E>
{
  public readonly Result<T, E> Tap(Action<T?> action)
  {
    ArgumentNullException.ThrowIfNull(action);

    if (IsSuccess)
      action(Value);

    return this;
  }

  public readonly Result<T, E> TapError(Action<E?> action)
  {
    ArgumentNullException.ThrowIfNull(action);

    if (IsFailure)
      action(Error);

    return this;
  }
}

public readonly partial struct UnitResult<E>
{
  public readonly UnitResult<E> Tap(Action action)
  {
    ArgumentNullException.ThrowIfNull(action);

    if (IsSuccess)
      action();

    return this;
  }

  public readonly UnitResult<E> TapError(Action<E?> action)
  {
    ArgumentNullException.ThrowIfNull(action);

    if (IsFailure)
      action(Error);

    return this;
  }
}
