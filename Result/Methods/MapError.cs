namespace ResultType;

public readonly partial struct Result
{
  public readonly Result MapError(Func<string?, string?> mapper)
  {
    ArgumentNullException.ThrowIfNull(mapper);

    return IsFailure
      ? Result.Failure(mapper(Error))
      : this;
  }

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
  public readonly Result<T> MapError(Func<string?, string?> mapper)
  {
    ArgumentNullException.ThrowIfNull(mapper);

    return IsFailure
      ? Result.Failure<T>(mapper(Error))
      : this;
  }

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
  public readonly Result<T, E> MapError(Func<E?, E?> mapper)
  {
    ArgumentNullException.ThrowIfNull(mapper);

    return IsFailure
      ? Result.Failure<T, E>(mapper(Error))
      : this;
  }

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
  public readonly UnitResult<E?> MapError(Func<E?, E?> mapper)
  {
    ArgumentNullException.ThrowIfNull(mapper);

    return IsFailure
      ? UnitResult.Failure(mapper(Error))
      : this;
  }

  public readonly UnitResult<EOut?> MapErrorTo<EOut>(Func<E?, EOut?> mapper)
  {
    ArgumentNullException.ThrowIfNull(mapper);

    return IsFailure
      ? UnitResult.Failure(mapper(Error))
      : UnitResult.Success<EOut>();
  }
}
