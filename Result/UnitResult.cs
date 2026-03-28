using static ResultType.Internals.ResultCommonLogic;

namespace ResultType
{
  /// <summary>
  /// Представляет результат выполнения операции без значения,
  /// но с типизированной ошибкой.
  /// </summary>
  /// <typeparam name="E">Тип ошибки.</typeparam>
  public readonly partial struct UnitResult<E> : IUnitResult<E?>
  {
    /// <summary>
    /// Признак получения ошибки при выполнении операции
    /// </summary>
    public bool IsFailure { get; }

    /// <summary>
    /// Признак успешного выполнения операции
    /// </summary>
    public bool IsSuccess => !IsFailure;

    private readonly E? _error;

    /// <summary>
    /// Получает типизированную ошибку неуспешного результата.
    /// </summary>
    /// <remarks>
    /// Доступно только для неуспешного результата.
    /// </remarks>
    public E? Error => GetErrorWithSuccessGuard(IsFailure, _error);

    internal UnitResult(bool isFailure, E? error)
    {
      IsFailure = ErrorStateGuard(isFailure, error);
      _error = error;
    }

    /// <summary>
    /// Неявное преобразование признака успешности в булево значение
    /// </summary>
    /// <param name="result"></param>
    public static implicit operator bool(UnitResult<E?> result) => result.IsSuccess;
  }

  /// <summary>
  /// Представляет результат выполнения операции без значения
  /// </summary>
  public struct UnitResult
  {
    /// <summary>
    /// Создаёт неуспешный unit-результат с типизированной ошибкой.
    /// </summary>
    /// <param name="error">Ошибка.</param>
    public static UnitResult<E?> Failure<E>(E error) => new(true, error);

    /// <summary>
    /// Создаёт успешный unit-результат.
    /// </summary>
    public static UnitResult<E?> Success<E>() => Result.Success<E?>();
  }
}
