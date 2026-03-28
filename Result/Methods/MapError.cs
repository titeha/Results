namespace ResultType;

public readonly partial struct Result
{
  /// <summary>
  /// Преобразует ошибку неуспешного результата, не изменяя успешный результат.
  /// </summary>
  public readonly Result MapError(Func<string?, string?> mapper)
  {
    ArgumentNullException.ThrowIfNull(mapper);

    return IsFailure
      ? Failure(mapper(Error))
      : this;
  }

  /// <summary>
  /// Преобразует ошибку неуспешного результата в ошибку другого типа.
  /// </summary>
  public readonly UnitResult<EOut?> MapErrorTo<EOut>(Func<string?, EOut?> mapper)
  {
    ArgumentNullException.ThrowIfNull(mapper);

    return IsFailure
      ? UnitResult.Failure(mapper(Error))
      : UnitResult.Success<EOut>();
  }
}

public readonly partial struct Result<T>
{
  /// <summary>
  /// Преобразует ошибку неуспешного результата, не изменяя успешный результат.
  /// </summary>
  public readonly Result<T> MapError(Func<string?, string?> mapper)
  {
    ArgumentNullException.ThrowIfNull(mapper);

    return IsFailure
      ? Result.Failure<T>(mapper(Error))
      : this;
  }

  /// <summary>
  /// Преобразует ошибку неуспешного результата в ошибку другого типа.
  /// </summary>
  public readonly Result<T, EOut> MapErrorTo<EOut>(Func<string?, EOut?> mapper)
  {
    ArgumentNullException.ThrowIfNull(mapper);

    return IsFailure
      ? Result.Failure<T, EOut>(mapper(Error))
      : Result.Success<T, EOut>(Value!);
  }
}

public readonly partial struct Result<T, E>
{
  /// <summary>
  /// Преобразует ошибку неуспешного результата, не изменяя успешный результат.
  /// </summary>
  public readonly Result<T, E> MapError(Func<E?, E?> mapper)
  {
    ArgumentNullException.ThrowIfNull(mapper);

    return IsFailure
      ? Result.Failure<T, E>(mapper(Error))
      : this;
  }

  /// <summary>
  /// Преобразует ошибку неуспешного результата в ошибку другого типа.
  /// </summary>
  public readonly Result<T, EOut> MapErrorTo<EOut>(Func<E?, EOut?> mapper)
  {
    ArgumentNullException.ThrowIfNull(mapper);

    return IsFailure
      ? Result.Failure<T, EOut>(mapper(Error))
      : Result.Success<T, EOut>(Value!);
  }
}

public readonly partial struct UnitResult<E>
{
  /// <summary>
  /// Преобразует ошибку неуспешного результата, не изменяя успешный результат.
  /// </summary>
  public readonly UnitResult<E?> MapError(Func<E?, E?> mapper)
  {
    ArgumentNullException.ThrowIfNull(mapper);

    return IsFailure
      ? UnitResult.Failure(mapper(Error))
      : this;
  }

  /// <summary>
  /// Преобразует ошибку неуспешного результата в ошибку другого типа.
  /// </summary>
  public readonly UnitResult<EOut?> MapErrorTo<EOut>(Func<E?, EOut?> mapper)
  {
    ArgumentNullException.ThrowIfNull(mapper);

    return IsFailure
      ? UnitResult.Failure(mapper(Error))
      : UnitResult.Success<EOut>();
  }
}
