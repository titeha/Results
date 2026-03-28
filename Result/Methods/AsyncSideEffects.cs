namespace ResultType;

public readonly partial struct Result
{
  /// <summary>
  /// Асинхронно проверяет дополнительное условие над успешным результатом.
  /// </summary>
  public async Task<Result> EnsureAsync(Func<Task<bool>> predicate, string error)
  {
    ArgumentNullException.ThrowIfNull(predicate);

    if (IsFailure)
      return this;

    bool isValid = await predicate().ConfigureAwait(DefaultConfigureAwait);
    return isValid ? this : Failure(error);
  }

  /// <summary>
  /// Асинхронно выполняет побочный эффект только для успешного результата, не изменяя сам результат.
  /// </summary>
  public async Task<Result> TapAsync(Func<Task> action)
  {
    ArgumentNullException.ThrowIfNull(action);

    if (IsSuccess)
      await action().ConfigureAwait(DefaultConfigureAwait);

    return this;
  }

  /// <summary>
  /// Асинхронно выполняет побочный эффект только для неуспешного результата, не изменяя сам результат.
  /// </summary>
  public async Task<Result> TapErrorAsync(Func<string?, Task> action)
  {
    ArgumentNullException.ThrowIfNull(action);

    if (IsFailure)
      await action(Error).ConfigureAwait(DefaultConfigureAwait);

    return this;
  }
}

public readonly partial struct Result<T>
{
  /// <summary>
  /// Асинхронно проверяет дополнительное условие над успешным результатом.
  /// </summary>
  public async Task<Result<T>> EnsureAsync(Func<T?, Task<bool>> predicate, string error)
  {
    ArgumentNullException.ThrowIfNull(predicate);

    if (IsFailure)
      return this;

    bool isValid = await predicate(Value).ConfigureAwait(Result.DefaultConfigureAwait);
    return isValid ? this : Result.Failure<T>(error);
  }

  /// <summary>
  /// Асинхронно выполняет побочный эффект только для успешного результата, не изменяя сам результат.
  /// </summary>
  public async Task<Result<T>> TapAsync(Func<T?, Task> action)
  {
    ArgumentNullException.ThrowIfNull(action);

    if (IsSuccess)
      await action(Value).ConfigureAwait(Result.DefaultConfigureAwait);

    return this;
  }

  /// <summary>
  /// Асинхронно выполняет побочный эффект только для неуспешного результата, не изменяя сам результат.
  /// </summary>
  public async Task<Result<T>> TapErrorAsync(Func<string?, Task> action)
  {
    ArgumentNullException.ThrowIfNull(action);

    if (IsFailure)
      await action(Error).ConfigureAwait(Result.DefaultConfigureAwait);

    return this;
  }
}

public readonly partial struct Result<T, E>
{
  /// <summary>
  /// Асинхронно проверяет дополнительное условие над успешным результатом.
  /// </summary>
  public async Task<Result<T, E>> EnsureAsync(Func<T?, Task<bool>> predicate, E error)
  {
    ArgumentNullException.ThrowIfNull(predicate);

    if (IsFailure)
      return this;

    bool isValid = await predicate(Value).ConfigureAwait(Result.DefaultConfigureAwait);
    return isValid ? this : Result.Failure<T, E>(error);
  }

  /// <summary>
  /// Асинхронно выполняет побочный эффект только для успешного результата, не изменяя сам результат.
  /// </summary>
  public async Task<Result<T, E>> TapAsync(Func<T?, Task> action)
  {
    ArgumentNullException.ThrowIfNull(action);

    if (IsSuccess)
      await action(Value).ConfigureAwait(Result.DefaultConfigureAwait);

    return this;
  }

  /// <summary>
  /// Асинхронно выполняет побочный эффект только для неуспешного результата, не изменяя сам результат.
  /// </summary>
  public async Task<Result<T, E>> TapErrorAsync(Func<E?, Task> action)
  {
    ArgumentNullException.ThrowIfNull(action);

    if (IsFailure)
      await action(Error).ConfigureAwait(Result.DefaultConfigureAwait);

    return this;
  }
}

public readonly partial struct UnitResult<E>
{
  /// <summary>
  /// Асинхронно проверяет дополнительное условие над успешным результатом.
  /// </summary>
  public async Task<UnitResult<E?>> EnsureAsync(Func<Task<bool>> predicate, E error)
  {
    ArgumentNullException.ThrowIfNull(predicate);

    if (IsFailure)
      return this;

    bool isValid = await predicate().ConfigureAwait(Result.DefaultConfigureAwait);
    return isValid ? this : UnitResult.Failure(error);
  }

  /// <summary>
  /// Асинхронно выполняет побочный эффект только для успешного результата, не изменяя сам результат.
  /// </summary>
  public async Task<UnitResult<E>> TapAsync(Func<Task> action)
  {
    ArgumentNullException.ThrowIfNull(action);

    if (IsSuccess)
      await action().ConfigureAwait(Result.DefaultConfigureAwait);

    return this;
  }

  /// <summary>
  /// Асинхронно выполняет побочный эффект только для неуспешного результата, не изменяя сам результат.
  /// </summary>
  public async Task<UnitResult<E>> TapErrorAsync(Func<E?, Task> action)
  {
    ArgumentNullException.ThrowIfNull(action);

    if (IsFailure)
      await action(Error).ConfigureAwait(Result.DefaultConfigureAwait);

    return this;
  }
}

/// <summary>
/// Класс расширения эффектов результата в асинхронном представлении
/// </summary>
public static class ResultTaskSideEffectsExtensions
{
  /// <summary>
  /// Асинхронно проверяет дополнительное условие над успешным результатом.
  /// </summary>
  public static async Task<Result> EnsureAsync(this Task<Result> resultTask, Func<Task<bool>> predicate, string error)
  {
    ArgumentNullException.ThrowIfNull(resultTask);
    ArgumentNullException.ThrowIfNull(predicate);

    Result result = await resultTask.ConfigureAwait(Result.DefaultConfigureAwait);
    return await result.EnsureAsync(predicate, error).ConfigureAwait(Result.DefaultConfigureAwait);
  }

  /// <summary>
  /// Асинхронно проверяет дополнительное условие над успешным результатом.
  /// </summary>
  public static async Task<Result<T>> EnsureAsync<T>(this Task<Result<T>> resultTask, Func<T?, Task<bool>> predicate, string error)
  {
    ArgumentNullException.ThrowIfNull(resultTask);
    ArgumentNullException.ThrowIfNull(predicate);

    Result<T> result = await resultTask.ConfigureAwait(Result.DefaultConfigureAwait);
    return await result.EnsureAsync(predicate, error).ConfigureAwait(Result.DefaultConfigureAwait);
  }

  /// <summary>
  /// Асинхронно проверяет дополнительное условие над успешным результатом.
  /// </summary>
  public static async Task<Result<T, E>> EnsureAsync<T, E>(this Task<Result<T, E>> resultTask, Func<T?, Task<bool>> predicate, E error)
  {
    ArgumentNullException.ThrowIfNull(resultTask);
    ArgumentNullException.ThrowIfNull(predicate);

    Result<T, E> result = await resultTask.ConfigureAwait(Result.DefaultConfigureAwait);
    return await result.EnsureAsync(predicate, error).ConfigureAwait(Result.DefaultConfigureAwait);
  }

  /// <summary>
  /// Асинхронно проверяет дополнительное условие над успешным результатом.
  /// </summary>
  public static async Task<UnitResult<E?>> EnsureAsync<E>(this Task<UnitResult<E>> resultTask, Func<Task<bool>> predicate, E error)
  {
    ArgumentNullException.ThrowIfNull(resultTask);
    ArgumentNullException.ThrowIfNull(predicate);

    UnitResult<E> result = await resultTask.ConfigureAwait(Result.DefaultConfigureAwait);
    return await result.EnsureAsync(predicate, error).ConfigureAwait(Result.DefaultConfigureAwait);
  }

  /// <summary>
  /// Асинхронно выполняет побочный эффект только для успешного результата, не изменяя сам результат.
  /// </summary>
  public static async Task<Result> TapAsync(this Task<Result> resultTask, Func<Task> action)
  {
    ArgumentNullException.ThrowIfNull(resultTask);
    ArgumentNullException.ThrowIfNull(action);

    Result result = await resultTask.ConfigureAwait(Result.DefaultConfigureAwait);
    return await result.TapAsync(action).ConfigureAwait(Result.DefaultConfigureAwait);
  }

  /// <summary>
  /// Асинхронно выполняет побочный эффект только для успешного результата, не изменяя сам результат.
  /// </summary>
  public static async Task<Result<T>> TapAsync<T>(this Task<Result<T>> resultTask, Func<T?, Task> action)
  {
    ArgumentNullException.ThrowIfNull(resultTask);
    ArgumentNullException.ThrowIfNull(action);

    Result<T> result = await resultTask.ConfigureAwait(Result.DefaultConfigureAwait);
    return await result.TapAsync(action).ConfigureAwait(Result.DefaultConfigureAwait);
  }

  /// <summary>
  /// Асинхронно выполняет побочный эффект только для успешного результата, не изменяя сам результат.
  /// </summary>
  public static async Task<Result<T, E>> TapAsync<T, E>(this Task<Result<T, E>> resultTask, Func<T?, Task> action)
  {
    ArgumentNullException.ThrowIfNull(resultTask);
    ArgumentNullException.ThrowIfNull(action);

    Result<T, E> result = await resultTask.ConfigureAwait(Result.DefaultConfigureAwait);
    return await result.TapAsync(action).ConfigureAwait(Result.DefaultConfigureAwait);
  }

  /// <summary>
  /// Асинхронно выполняет побочный эффект только для успешного результата, не изменяя сам результат.
  /// </summary>
  public static async Task<UnitResult<E>> TapAsync<E>(this Task<UnitResult<E>> resultTask, Func<Task> action)
  {
    ArgumentNullException.ThrowIfNull(resultTask);
    ArgumentNullException.ThrowIfNull(action);

    UnitResult<E> result = await resultTask.ConfigureAwait(Result.DefaultConfigureAwait);
    return await result.TapAsync(action).ConfigureAwait(Result.DefaultConfigureAwait);
  }

  /// <summary>
  /// Асинхронно выполняет побочный эффект только для неуспешного результата, не изменяя сам результат.
  /// </summary>
  public static async Task<Result> TapErrorAsync(this Task<Result> resultTask, Func<string?, Task> action)
  {
    ArgumentNullException.ThrowIfNull(resultTask);
    ArgumentNullException.ThrowIfNull(action);

    Result result = await resultTask.ConfigureAwait(Result.DefaultConfigureAwait);
    return await result.TapErrorAsync(action).ConfigureAwait(Result.DefaultConfigureAwait);
  }

  /// <summary>
  /// Асинхронно выполняет побочный эффект только для неуспешного результата, не изменяя сам результат.
  /// </summary>
  public static async Task<Result<T>> TapErrorAsync<T>(this Task<Result<T>> resultTask, Func<string?, Task> action)
  {
    ArgumentNullException.ThrowIfNull(resultTask);
    ArgumentNullException.ThrowIfNull(action);

    Result<T> result = await resultTask.ConfigureAwait(Result.DefaultConfigureAwait);
    return await result.TapErrorAsync(action).ConfigureAwait(Result.DefaultConfigureAwait);
  }

  /// <summary>
  /// Асинхронно выполняет побочный эффект только для неуспешного результата, не изменяя сам результат.
  /// </summary>
  public static async Task<Result<T, E>> TapErrorAsync<T, E>(this Task<Result<T, E>> resultTask, Func<E?, Task> action)
  {
    ArgumentNullException.ThrowIfNull(resultTask);
    ArgumentNullException.ThrowIfNull(action);

    Result<T, E> result = await resultTask.ConfigureAwait(Result.DefaultConfigureAwait);
    return await result.TapErrorAsync(action).ConfigureAwait(Result.DefaultConfigureAwait);
  }

  /// <summary>
  /// Асинхронно выполняет побочный эффект только для неуспешного результата, не изменяя сам результат.
  /// </summary>
  public static async Task<UnitResult<E>> TapErrorAsync<E>(this Task<UnitResult<E>> resultTask, Func<E?, Task> action)
  {
    ArgumentNullException.ThrowIfNull(resultTask);
    ArgumentNullException.ThrowIfNull(action);

    UnitResult<E> result = await resultTask.ConfigureAwait(Result.DefaultConfigureAwait);
    return await result.TapErrorAsync(action).ConfigureAwait(Result.DefaultConfigureAwait);
  }
}
