namespace ResultType.Exceptions;

/// <summary>
/// Исключение, выбрасываемое при попытке получить доступ к значению
/// у неуспешного результата.
/// </summary>
/// <remarks>
/// Используется библиотекой для защиты инвариантов типов <c>Result</c>.
/// Например, при обращении к <c>Value</c> у результата, находящегося в состоянии failure.
/// </remarks>
public class ResultFailureException : Exception
{
  /// <summary>
  /// Описание ошибки в исключении
  /// </summary>
  public string? Error { get; }

  internal ResultFailureException(string? error) : base(Result.Messages.ValueIsInaccessibleForFailure(error)) => Error = error;
}

/// <summary>
/// Исключение, выбрасываемое при попытке получить доступ к значению
/// у неуспешного типизированного результата.
/// </summary>
/// <typeparam name="E">Тип ошибки, связанный с результатом.</typeparam>
/// <remarks>
/// Используется библиотекой для защиты инвариантов типов <c>Result&lt;T, E&gt;</c>
/// и <c>UnitResult&lt;E&gt;</c>.
/// </remarks>
public class ResultFailureException<E> : ResultFailureException
{
  /// <summary>
  /// Объект ошибки в исключении
  /// </summary>
  public new E? Error { get; }

  internal ResultFailureException(E? error) : base(Result.Messages.ValueIsInaccessibleForFailure(error?.ToString())) => Error = error;
}