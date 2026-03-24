namespace ResultType;

public readonly partial struct Result
{
  public readonly Result<T> Map<T>(Func<T> mapper)
  {
    ArgumentNullException.ThrowIfNull(mapper);

    return IsSuccess
      ? Result.Success(mapper())
      : Result.Failure<T>(Error);
  }
}

public readonly partial struct Result<T>
{
  public readonly Result<K> Map<K>(Func<T?, K> mapper)
  {
    ArgumentNullException.ThrowIfNull(mapper);

    return IsSuccess
      ? Result.Success(mapper(Value))
      : Result.Failure<K>(Error);
  }
}

public readonly partial struct Result<T, E>
{
  public readonly Result<K, E> Map<K>(Func<T?, K> mapper)
  {
    ArgumentNullException.ThrowIfNull(mapper);

    return IsSuccess
      ? Result.Success<K, E>(mapper(Value))
      : Result.Failure<K, E>(Error);
  }
}

public readonly partial struct UnitResult<E>
{
  public readonly Result<T, E> Map<T>(Func<T> mapper)
  {
    ArgumentNullException.ThrowIfNull(mapper);

    return IsSuccess
      ? Result.Success<T, E>(mapper())
      : Result.Failure<T, E>(Error);
  }
}
