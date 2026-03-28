namespace ResultType;

public partial struct Result
{
  private static readonly Func<Exception, string> _defaultTryErrorHandler = exc => exc.Message;

  /// <summary>
  /// Пытается выполнить действие и преобразовать выброшенное исключение в результат.
  /// </summary>
  /// <param name="action">Действие для выполнения.</param>
  /// <param name="errorHandler">Обработчик ошибки</param>
  /// <returns>Успешный результат, если исключение не было выброшено; иначе — неуспешный результат с ошибкой.</returns>
  public static Result Try(Action action, Func<Exception, string>? errorHandler = null)
  {
    errorHandler ??= _defaultTryErrorHandler;

    try
    {
      action();
      return Success();
    }
    catch (Exception e)
    {
      string _message = errorHandler(e);
      return Failure(_message);
    }
  }

  /// <summary>
  /// Пытается асинхронно выполнить действие и преобразовать выброшенное исключение в результат.
  /// </summary>
  /// <param name="action">Асинхронное действие для выполнения.</param>
  /// <param name="errorHandler">Обработчик ошибки</param>
  public static async Task<Result> Try(Func<Task> action, Func<Exception, string>? errorHandler = null)
  {
    errorHandler ??= _defaultTryErrorHandler;

    try
    {
      await action().DefaultAwait();
      return Success();
    }
    catch (Exception e)
    {
      string _message = errorHandler(e);
      return Failure(_message);
    }
  }

  /// <summary>
  /// Пытается выполнить действие и преобразовать выброшенное исключение в результат.
  /// </summary>
  /// <param name="func">Действие для выполнения.</param>
  /// <param name="errorHandler">Обработчик ошибки</param>
  /// <returns>Успешный результат, если исключение не было выброшено; иначе — неуспешный результат с ошибкой.</returns>
  public static Result<T> Try<T>(Func<T> func, Func<Exception, string>? errorHandler = null)
  {
    errorHandler ??= _defaultTryErrorHandler;

    try
    {
      return Success(func());
    }
    catch (Exception e)
    {
      string _message = errorHandler(e);
      return Failure<T>(_message);
    }
  }

  /// <summary>
  /// Пытается асинхронно выполнить действие и преобразовать выброшенное исключение в результат.
  /// </summary>
  /// <param name="func">Асинхронное действие для выполнения.</param>
  /// <param name="errorHandler">Обработчик ошибки</param>
  public static async Task<Result<T>> Try<T>(Func<Task<T>> func, Func<Exception, string>? errorHandler = null)
  {
    errorHandler ??= _defaultTryErrorHandler;

    try
    {
      var result = await func().DefaultAwait();
      return Success(result);
    }
    catch (Exception e)
    {
      string _message = errorHandler(e);
      return Failure<T>(_message);
    }
  }

  /// <summary>
  /// Пытается выполнить функцию и преобразовать выброшенное исключение в типизированный результат.
  /// </summary>
  /// <typeparam name="T">Тип возвращаемого значения.</typeparam>
  /// <typeparam name="E">Тип возвращаемой ошибки</typeparam>
  /// <param name="func">Функция для выполнения.</param>
  /// <param name="errorHandler">Обработчик ошибки</param>
  public static Result<T, E> Try<T, E>(Func<T> func, Func<Exception, E> errorHandler)
  {
    try
    {
      return Success<T, E>(func());
    }
    catch (Exception e)
    {
      E _error = errorHandler(e);
      return Failure<T, E>(_error);
    }
  }

  /// <summary>
  /// Пытается асинхронно выполнить функцию и преобразовать выброшенное исключение в типизированный результат.
  /// </summary>
  /// <typeparam name="T">Тип возвращаемого значения.</typeparam>
  /// <typeparam name="E">Тип возвращаемой ошибки</typeparam>
  /// <param name="func">Асинхронная функция для выполнения.</param>
  /// <param name="errorHandler">Обработчик ошибки</param>
  public static async Task<Result<T, E>> Try<T, E>(Func<Task<T>> func, Func<Exception, E> errorHandler)
  {
    try
    {
      var result = await func().DefaultAwait();
      return Success<T, E>(result);
    }
    catch (Exception e)
    {
      E _error = errorHandler(e);
      return Failure<T, E>(_error);
    }
  }
}