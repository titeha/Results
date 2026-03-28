namespace ResultType;

public readonly partial struct Result
{
  /// <summary>
  /// Цепляет следующий шаг, который сам возвращает результат.
  /// </summary>
  public readonly Result Bind(Func<Result> binder)
  {
    ArgumentNullException.ThrowIfNull(binder);

    return IsSuccess
      ? binder()
      : Failure(Error);
  }

  /// <summary>
  /// Цепляет следующий шаг, который сам возвращает результат.
  /// </summary>
  public readonly Result<T> Bind<T>(Func<Result<T>> binder)
  {
    ArgumentNullException.ThrowIfNull(binder);

    return IsSuccess
      ? binder()
      : Failure<T>(Error);
  }
}

public readonly partial struct Result<T>
{
  /// <summary>
  /// Цепляет следующий шаг, который сам возвращает результат.
  /// </summary>
  public readonly Result Bind(Func<T?, Result> binder)
  {
    ArgumentNullException.ThrowIfNull(binder);

    return IsSuccess
      ? binder(Value)
      : Result.Failure(Error);
  }

  /// <summary>
  /// Цепляет следующий шаг, который сам возвращает результат.
  /// </summary>
  public readonly Result<K> Bind<K>(Func<T?, Result<K>> binder)
  {
    ArgumentNullException.ThrowIfNull(binder);

    return IsSuccess
      ? binder(Value)
      : Result.Failure<K>(Error);
  }
}

public readonly partial struct Result<T, E>
{
  /// <summary>
  /// Цепляет следующий шаг, который сам возвращает результат.
  /// </summary>
  public readonly UnitResult<E> Bind(Func<T?, UnitResult<E>> binder)
  {
    ArgumentNullException.ThrowIfNull(binder);

    return IsSuccess
      ? binder(Value)
      : UnitResult.Failure(Error)!;
  }

  /// <summary>
  /// Цепляет следующий шаг, который сам возвращает результат.
  /// </summary>
  public readonly Result<K, E> Bind<K>(Func<T?, Result<K, E>> binder)
  {
    ArgumentNullException.ThrowIfNull(binder);

    return IsSuccess
      ? binder(Value)
      : Result.Failure<K, E>(Error);
  }
}

public readonly partial struct UnitResult<E>
{
  /// <summary>
  /// Цепляет следующий шаг, который сам возвращает результат.
  /// </summary>
  public readonly UnitResult<E> Bind(Func<UnitResult<E>> binder)
  {
    ArgumentNullException.ThrowIfNull(binder);

    return IsSuccess
      ? binder()
      : UnitResult.Failure(Error)!;
  }

  /// <summary>
  /// Цепляет следующий шаг, который сам возвращает результат.
  /// </summary>
  public readonly Result<T, E> Bind<T>(Func<Result<T, E>> binder)
  {
    ArgumentNullException.ThrowIfNull(binder);

    return IsSuccess
      ? binder()
      : Result.Failure<T, E>(Error);
  }
}
