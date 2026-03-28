namespace ResultType;

public partial struct Result
{
  /// <summary>
    /// Пытается выполнить действие и преобразовать выброшенное исключение в типизированную ошибку.
    /// </summary>
    /// <typeparam name="E">Тип ошибки.</typeparam>
    /// <param name="action">Действие для выполнения.</param>
    /// <param name="errorHandler">Преобразователь исключения в тип ошибки.</param>
  public static UnitResult<E?> Try<E>(Action action, Func<Exception, E> errorHandler)
  {
    ArgumentNullException.ThrowIfNull(action);
    ArgumentNullException.ThrowIfNull(errorHandler);

    try
    {
      action();
      return UnitResult.Success<E>();
    }
    catch (Exception exception)
    {
      E error = errorHandler(exception);
      return UnitResult.Failure<E?>(error);
    }
  }

  /// <summary>
  /// Пытается асинхронно выполнить действие и преобразовать выброшенное исключение в типизированную ошибку.
  /// </summary>
  /// <typeparam name="E">Тип ошибки.</typeparam>
  /// <param name="action">Асинхронное действие для выполнения.</param>
  /// <param name="errorHandler">Преобразователь исключения в тип ошибки.</param>
  public static async Task<UnitResult<E?>> Try<E>(Func<Task> action, Func<Exception, E> errorHandler)
  {
    ArgumentNullException.ThrowIfNull(action);
    ArgumentNullException.ThrowIfNull(errorHandler);

    try
    {
      await action().DefaultAwait();
      return UnitResult.Success<E>();
    }
    catch (Exception exception)
    {
      E error = errorHandler(exception);
      return UnitResult.Failure<E?>(error);
    }
  }
}
