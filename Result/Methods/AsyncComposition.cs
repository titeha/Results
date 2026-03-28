namespace ResultType;

public readonly partial struct Result
{
  /// <summary>
  /// Асинхронно сопоставляет успешный и неуспешный результат с обычным значением.
  /// </summary>
  public async Task<TResult> MatchAsync<TResult>(
    Func<Task<TResult>> onSuccess,
    Func<string?, Task<TResult>> onFailure)
  {
    ArgumentNullException.ThrowIfNull(onSuccess);
    ArgumentNullException.ThrowIfNull(onFailure);

    return IsSuccess
      ? await onSuccess().ConfigureAwait(DefaultConfigureAwait)
      : await onFailure(Error).ConfigureAwait(DefaultConfigureAwait);
  }

  /// <summary>
  /// Асинхронно преобразует успешный результат без значения в успешный результат со значением.
  /// </summary>
  public async Task<Result<TOut>> MapAsync<TOut>(Func<Task<TOut>> mapper)
  {
    ArgumentNullException.ThrowIfNull(mapper);

    return IsSuccess
      ? Success(await mapper().ConfigureAwait(DefaultConfigureAwait))
      : Failure<TOut>(Error);
  }

  /// <summary>
  /// Асинхронно цепляет следующий шаг, который сам возвращает результат.
  /// </summary>
  public async Task<Result> BindAsync(Func<Task<Result>> binder)
  {
    ArgumentNullException.ThrowIfNull(binder);

    return IsSuccess
      ? await binder().ConfigureAwait(DefaultConfigureAwait)
      : Failure(Error);
  }

  /// <summary>
  /// Асинхронно цепляет следующий шаг, который сам возвращает результат.
  /// </summary>
  public async Task<Result<TOut>> BindAsync<TOut>(Func<Task<Result<TOut>>> binder)
  {
    ArgumentNullException.ThrowIfNull(binder);

    return IsSuccess
      ? await binder().ConfigureAwait(DefaultConfigureAwait)
      : Failure<TOut>(Error);
  }
}

public readonly partial struct Result<T>
{
  /// <summary>
  /// Асинхронно сопоставляет успешный и неуспешный результат с обычным значением.
  /// </summary>
  public async Task<TResult> MatchAsync<TResult>(
    Func<T?, Task<TResult>> onSuccess,
    Func<string?, Task<TResult>> onFailure)
  {
    ArgumentNullException.ThrowIfNull(onSuccess);
    ArgumentNullException.ThrowIfNull(onFailure);

    return IsSuccess
      ? await onSuccess(Value).ConfigureAwait(Result.DefaultConfigureAwait)
      : await onFailure(Error).ConfigureAwait(Result.DefaultConfigureAwait);
  }

  /// <summary>
  /// Асинхронно преобразует успешный результат без значения в успешный результат со значением.
  /// </summary>
  public async Task<Result<TOut>> MapAsync<TOut>(Func<T?, Task<TOut>> mapper)
  {
    ArgumentNullException.ThrowIfNull(mapper);

    return IsSuccess
      ? Result.Success(await mapper(Value).ConfigureAwait(Result.DefaultConfigureAwait))
      : Result.Failure<TOut>(Error);
  }

  /// <summary>
  /// Асинхронно цепляет следующий шаг, который сам возвращает результат.
  /// </summary>
  public async Task<Result> BindAsync(Func<T?, Task<Result>> binder)
  {
    ArgumentNullException.ThrowIfNull(binder);

    return IsSuccess
      ? await binder(Value).ConfigureAwait(Result.DefaultConfigureAwait)
      : Result.Failure(Error);
  }

  /// <summary>
  /// Асинхронно цепляет следующий шаг, который сам возвращает результат.
  /// </summary>
  public async Task<Result<TOut>> BindAsync<TOut>(Func<T?, Task<Result<TOut>>> binder)
  {
    ArgumentNullException.ThrowIfNull(binder);

    return IsSuccess
      ? await binder(Value).ConfigureAwait(Result.DefaultConfigureAwait)
      : Result.Failure<TOut>(Error);
  }
}

public readonly partial struct Result<T, E>
{
  /// <summary>
  /// Асинхронно сопоставляет успешный и неуспешный результат с обычным значением.
  /// </summary>
  public async Task<TResult> MatchAsync<TResult>(
    Func<T?, Task<TResult>> onSuccess,
    Func<E?, Task<TResult>> onFailure)
  {
    ArgumentNullException.ThrowIfNull(onSuccess);
    ArgumentNullException.ThrowIfNull(onFailure);

    return IsSuccess
      ? await onSuccess(Value).ConfigureAwait(Result.DefaultConfigureAwait)
      : await onFailure(Error).ConfigureAwait(Result.DefaultConfigureAwait);
  }

  /// <summary>
  /// Асинхронно преобразует успешный результат без значения в успешный результат со значением.
  /// </summary>
  public async Task<Result<TOut, E>> MapAsync<TOut>(Func<T?, Task<TOut>> mapper)
  {
    ArgumentNullException.ThrowIfNull(mapper);

    return IsSuccess
      ? Result.Success<TOut, E>(await mapper(Value).ConfigureAwait(Result.DefaultConfigureAwait))
      : Result.Failure<TOut, E>(Error);
  }

  /// <summary>
  /// Асинхронно цепляет следующий шаг, который сам возвращает результат.
  /// </summary>
  public async Task<UnitResult<E>> BindAsync(Func<T?, Task<UnitResult<E>>> binder)
  {
    ArgumentNullException.ThrowIfNull(binder);

    return IsSuccess
      ? await binder(Value).ConfigureAwait(Result.DefaultConfigureAwait)
      : UnitResult.Failure(Error)!;
  }

  /// <summary>
  /// Асинхронно цепляет следующий шаг, который сам возвращает результат.
  /// </summary>
  public async Task<Result<TOut, E>> BindAsync<TOut>(Func<T?, Task<Result<TOut, E>>> binder)
  {
    ArgumentNullException.ThrowIfNull(binder);

    return IsSuccess
      ? await binder(Value).ConfigureAwait(Result.DefaultConfigureAwait)
      : Result.Failure<TOut, E>(Error);
  }
}

public readonly partial struct UnitResult<E>
{
  /// <summary>
  /// Асинхронно сопоставляет успешный и неуспешный результат с обычным значением.
  /// </summary>
  public async Task<TResult> MatchAsync<TResult>(
    Func<Task<TResult>> onSuccess,
    Func<E?, Task<TResult>> onFailure)
  {
    ArgumentNullException.ThrowIfNull(onSuccess);
    ArgumentNullException.ThrowIfNull(onFailure);

    return IsSuccess
      ? await onSuccess().ConfigureAwait(Result.DefaultConfigureAwait)
      : await onFailure(Error).ConfigureAwait(Result.DefaultConfigureAwait);
  }

  /// <summary>
  /// Асинхронно преобразует успешный результат без значения в успешный результат со значением.
  /// </summary>
  public async Task<Result<TOut, E>> MapAsync<TOut>(Func<Task<TOut>> mapper)
  {
    ArgumentNullException.ThrowIfNull(mapper);

    return IsSuccess
      ? Result.Success<TOut, E>(await mapper().ConfigureAwait(Result.DefaultConfigureAwait))
      : Result.Failure<TOut, E>(Error);
  }

  /// <summary>
  /// Асинхронно цепляет следующий шаг, который сам возвращает результат.
  /// </summary>
  public async Task<UnitResult<E>> BindAsync(Func<Task<UnitResult<E>>> binder)
  {
    ArgumentNullException.ThrowIfNull(binder);

    return IsSuccess
      ? await binder().ConfigureAwait(Result.DefaultConfigureAwait)
      : UnitResult.Failure(Error)!;
  }
  /// <summary>
  /// Асинхронно цепляет следующий шаг, который сам возвращает результат.
  /// </summary>
  public async Task<Result<TOut, E>> BindAsync<TOut>(Func<Task<Result<TOut, E>>> binder)
  {
    ArgumentNullException.ThrowIfNull(binder);

    return IsSuccess
      ? await binder().ConfigureAwait(Result.DefaultConfigureAwait)
      : Result.Failure<TOut, E>(Error);
  }
}

/// <summary>
/// Класс расширения для операций результата в асинхронном представлении
/// </summary>
public static class ResultTaskExtensions
{
  /// <summary>
  /// Асинхронно сопоставляет успешный и неуспешный результат с обычным значением.
  /// </summary>
  public static async Task<TResult> MatchAsync<TResult>(
    this Task<Result> resultTask,
    Func<Task<TResult>> onSuccess,
    Func<string?, Task<TResult>> onFailure)
  {
    ArgumentNullException.ThrowIfNull(resultTask);
    return await (await resultTask.ConfigureAwait(Result.DefaultConfigureAwait))
      .MatchAsync(onSuccess, onFailure)
      .ConfigureAwait(Result.DefaultConfigureAwait);
  }

  /// <summary>
  /// Асинхронно сопоставляет успешный и неуспешный результат с обычным значением.
  /// </summary>
  public static async Task<TResult> MatchAsync<T, TResult>(
    this Task<Result<T>> resultTask,
    Func<T?, Task<TResult>> onSuccess,
    Func<string?, Task<TResult>> onFailure)
  {
    ArgumentNullException.ThrowIfNull(resultTask);
    return await (await resultTask.ConfigureAwait(Result.DefaultConfigureAwait))
      .MatchAsync(onSuccess, onFailure)
      .ConfigureAwait(Result.DefaultConfigureAwait);
  }

  /// <summary>
  /// Асинхронно сопоставляет успешный и неуспешный результат с обычным значением.
  /// </summary>
  public static async Task<TResult> MatchAsync<T, E, TResult>(
    this Task<Result<T, E>> resultTask,
    Func<T?, Task<TResult>> onSuccess,
    Func<E?, Task<TResult>> onFailure)
  {
    ArgumentNullException.ThrowIfNull(resultTask);
    return await (await resultTask.ConfigureAwait(Result.DefaultConfigureAwait))
      .MatchAsync(onSuccess, onFailure)
      .ConfigureAwait(Result.DefaultConfigureAwait);
  }

  /// <summary>
  /// Асинхронно сопоставляет успешный и неуспешный результат с обычным значением.
  /// </summary>
  public static async Task<TResult> MatchAsync<E, TResult>(
    this Task<UnitResult<E>> resultTask,
    Func<Task<TResult>> onSuccess,
    Func<E?, Task<TResult>> onFailure)
  {
    ArgumentNullException.ThrowIfNull(resultTask);
    return await (await resultTask.ConfigureAwait(Result.DefaultConfigureAwait))
      .MatchAsync(onSuccess, onFailure)
      .ConfigureAwait(Result.DefaultConfigureAwait);
  }

  /// <summary>
  /// Асинхронно преобразует успешный результат без значения в успешный результат со значением.
  /// </summary>
  public static async Task<Result<TOut>> MapAsync<TOut>(
    this Task<Result> resultTask,
    Func<Task<TOut>> mapper)
  {
    ArgumentNullException.ThrowIfNull(resultTask);
    return await (await resultTask.ConfigureAwait(Result.DefaultConfigureAwait))
      .MapAsync(mapper)
      .ConfigureAwait(Result.DefaultConfigureAwait);
  }

  /// <summary>
  /// Асинхронно преобразует успешный результат без значения в успешный результат со значением.
  /// </summary>
  public static async Task<Result<TOut>> MapAsync<T, TOut>(
    this Task<Result<T>> resultTask,
    Func<T?, Task<TOut>> mapper)
  {
    ArgumentNullException.ThrowIfNull(resultTask);
    return await (await resultTask.ConfigureAwait(Result.DefaultConfigureAwait))
      .MapAsync(mapper)
      .ConfigureAwait(Result.DefaultConfigureAwait);
  }

  /// <summary>
  /// Асинхронно преобразует успешный результат без значения в успешный результат со значением.
  /// </summary>
  public static async Task<Result<TOut, E>> MapAsync<T, E, TOut>(
    this Task<Result<T, E>> resultTask,
    Func<T?, Task<TOut>> mapper)
  {
    ArgumentNullException.ThrowIfNull(resultTask);
    return await (await resultTask.ConfigureAwait(Result.DefaultConfigureAwait))
      .MapAsync(mapper)
      .ConfigureAwait(Result.DefaultConfigureAwait);
  }

  /// <summary>
  /// Асинхронно преобразует успешный результат без значения в успешный результат со значением.
  /// </summary>
  public static async Task<Result<TOut, E>> MapAsync<E, TOut>(
    this Task<UnitResult<E>> resultTask,
    Func<Task<TOut>> mapper)
  {
    ArgumentNullException.ThrowIfNull(resultTask);
    return await (await resultTask.ConfigureAwait(Result.DefaultConfigureAwait))
      .MapAsync(mapper)
      .ConfigureAwait(Result.DefaultConfigureAwait);
  }

  /// <summary>
  /// Асинхронно цепляет следующий шаг, который сам возвращает результат.
  /// </summary>
  public static async Task<Result> BindAsync(
    this Task<Result> resultTask,
    Func<Task<Result>> binder)
  {
    ArgumentNullException.ThrowIfNull(resultTask);
    return await (await resultTask.ConfigureAwait(Result.DefaultConfigureAwait))
      .BindAsync(binder)
      .ConfigureAwait(Result.DefaultConfigureAwait);
  }

  /// <summary>
  /// Асинхронно цепляет следующий шаг, который сам возвращает результат.
  /// </summary>
  public static async Task<Result<TOut>> BindAsync<TOut>(
    this Task<Result> resultTask,
    Func<Task<Result<TOut>>> binder)
  {
    ArgumentNullException.ThrowIfNull(resultTask);
    return await (await resultTask.ConfigureAwait(Result.DefaultConfigureAwait))
      .BindAsync(binder)
      .ConfigureAwait(Result.DefaultConfigureAwait);
  }

  /// <summary>
  /// Асинхронно цепляет следующий шаг, который сам возвращает результат.
  /// </summary>
  public static async Task<Result> BindAsync<T>(
    this Task<Result<T>> resultTask,
    Func<T?, Task<Result>> binder)
  {
    ArgumentNullException.ThrowIfNull(resultTask);
    return await (await resultTask.ConfigureAwait(Result.DefaultConfigureAwait))
      .BindAsync(binder)
      .ConfigureAwait(Result.DefaultConfigureAwait);
  }

  /// <summary>
  /// Асинхронно цепляет следующий шаг, который сам возвращает результат.
  /// </summary>
  public static async Task<Result<TOut>> BindAsync<T, TOut>(
    this Task<Result<T>> resultTask,
    Func<T?, Task<Result<TOut>>> binder)
  {
    ArgumentNullException.ThrowIfNull(resultTask);
    return await (await resultTask.ConfigureAwait(Result.DefaultConfigureAwait))
      .BindAsync(binder)
      .ConfigureAwait(Result.DefaultConfigureAwait);
  }

  /// <summary>
  /// Асинхронно цепляет следующий шаг, который сам возвращает результат.
  /// </summary>
  public static async Task<UnitResult<E>> BindAsync<T, E>(
    this Task<Result<T, E>> resultTask,
    Func<T?, Task<UnitResult<E>>> binder)
  {
    ArgumentNullException.ThrowIfNull(resultTask);
    return await (await resultTask.ConfigureAwait(Result.DefaultConfigureAwait))
      .BindAsync(binder)
      .ConfigureAwait(Result.DefaultConfigureAwait);
  }

  /// <summary>
  /// Асинхронно цепляет следующий шаг, который сам возвращает результат.
  /// </summary>
  public static async Task<Result<TOut, E>> BindAsync<T, E, TOut>(
    this Task<Result<T, E>> resultTask,
    Func<T?, Task<Result<TOut, E>>> binder)
  {
    ArgumentNullException.ThrowIfNull(resultTask);
    return await (await resultTask.ConfigureAwait(Result.DefaultConfigureAwait))
      .BindAsync(binder)
      .ConfigureAwait(Result.DefaultConfigureAwait);
  }

  /// <summary>
  /// Асинхронно цепляет следующий шаг, который сам возвращает результат.
  /// </summary>
  public static async Task<UnitResult<E>> BindAsync<E>(
    this Task<UnitResult<E>> resultTask,
    Func<Task<UnitResult<E>>> binder)
  {
    ArgumentNullException.ThrowIfNull(resultTask);
    return await (await resultTask.ConfigureAwait(Result.DefaultConfigureAwait))
      .BindAsync(binder)
      .ConfigureAwait(Result.DefaultConfigureAwait);
  }

  /// <summary>
  /// Асинхронно цепляет следующий шаг, который сам возвращает результат.
  /// </summary>
  public static async Task<Result<TOut, E>> BindAsync<E, TOut>(
    this Task<UnitResult<E>> resultTask,
    Func<Task<Result<TOut, E>>> binder)
  {
    ArgumentNullException.ThrowIfNull(resultTask);
    return await (await resultTask.ConfigureAwait(Result.DefaultConfigureAwait))
      .BindAsync(binder)
      .ConfigureAwait(Result.DefaultConfigureAwait);
  }
}
