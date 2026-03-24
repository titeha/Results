namespace ResultType;

public partial struct Result
{
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
