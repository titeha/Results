namespace ResultType;

public readonly partial struct Result
{
  /// <summary>
  /// Выполняет побочный эффект только для успешного результата, не изменяя сам результат.
  /// </summary>
  public readonly Result Tap(Action action)
  {
    ArgumentNullException.ThrowIfNull(action);

    if (IsSuccess)
      action();

    return this;
  }

  /// <summary>
  /// Выполняет побочный эффект только для неуспешного результата, не изменяя сам результат.
  /// </summary>
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
  /// <summary>
  /// Выполняет побочный эффект только для успешного результата, не изменяя сам результат.
  /// </summary>
  public readonly Result<T> Tap(Action<T?> action)
  {
    ArgumentNullException.ThrowIfNull(action);

    if (IsSuccess)
      action(Value);

    return this;
  }

  /// <summary>
  /// Выполняет побочный эффект только для неуспешного результата, не изменяя сам результат.
  /// </summary>
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
  /// <summary>
  /// Выполняет побочный эффект только для успешного результата, не изменяя сам результат.
  /// </summary>
  public readonly Result<T, E> Tap(Action<T?> action)
  {
    ArgumentNullException.ThrowIfNull(action);

    if (IsSuccess)
      action(Value);

    return this;
  }

  /// <summary>
  /// Выполняет побочный эффект только для неуспешного результата, не изменяя сам результат.
  /// </summary>
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
  /// <summary>
  /// Выполняет побочный эффект только для успешного результата, не изменяя сам результат.
  /// </summary>
  public readonly UnitResult<E> Tap(Action action)
  {
    ArgumentNullException.ThrowIfNull(action);

    if (IsSuccess)
      action();

    return this;
  }

  /// <summary>
  /// Выполняет побочный эффект только для неуспешного результата, не изменяя сам результат.
  /// </summary>
  public readonly UnitResult<E> TapError(Action<E?> action)
  {
    ArgumentNullException.ThrowIfNull(action);

    if (IsFailure)
      action(Error);

    return this;
  }
}
