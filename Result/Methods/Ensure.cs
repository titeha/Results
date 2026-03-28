namespace ResultType;

public readonly partial struct Result
{
  /// <summary>
  /// Проверяет дополнительное условие над успешным результатом.
  /// </summary>
  /// <param name="predicate">Проверяемое условие.</param>
  /// <param name="error">Сообщение ошибки, если условие не выполнено.</param>
  public readonly Result Ensure(Func<bool> predicate, string? error)
  {
    ArgumentNullException.ThrowIfNull(predicate);

    if (IsFailure)
      return this;

    return predicate() ? this : Failure(error);
  }
}

public readonly partial struct Result<T>
{
  /// <summary>
  /// Проверяет дополнительное условие над успешным результатом.
  /// </summary>
  /// <param name="predicate">Проверяемое условие.</param>
  /// <param name="error">Сообщение ошибки, если условие не выполнено.</param>
  public readonly Result<T> Ensure(Func<T?, bool> predicate, string? error)
  {
    ArgumentNullException.ThrowIfNull(predicate);

    if (IsFailure)
      return this;

    return predicate(Value) ? this : Result.Failure<T>(error);
  }
}

public readonly partial struct Result<T, E>
{
  /// <summary>
  /// Проверяет дополнительное условие над успешным результатом.
  /// </summary>
  /// <param name="predicate">Проверяемое условие.</param>
  /// <param name="error">Сообщение ошибки, если условие не выполнено.</param>
  public readonly Result<T, E> Ensure(Func<T?, bool> predicate, E? error)
  {
    ArgumentNullException.ThrowIfNull(predicate);

    if (IsFailure)
      return this;

    return predicate(Value) ? this : Result.Failure<T, E>(error);
  }
}

public readonly partial struct UnitResult<E>
{
  /// <summary>
  /// Проверяет дополнительное условие над успешным результатом.
  /// </summary>
  /// <param name="predicate">Проверяемое условие.</param>
  /// <param name="error">Сообщение ошибки, если условие не выполнено.</param>
  public readonly UnitResult<E?> Ensure(Func<bool> predicate, E? error)
  {
    ArgumentNullException.ThrowIfNull(predicate);

    if (IsFailure)
      return this;

    return predicate() ? this : UnitResult.Failure(error);
  }
}
