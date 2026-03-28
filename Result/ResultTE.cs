using ResultType.Exceptions;

using static ResultType.Internals.ResultCommonLogic;

namespace ResultType
{
  /// <summary>
  /// Представляет результат выполнения операции со значением успеха и типизированной ошибкой.
  /// </summary>
  /// <typeparam name="T">Тип значения успешного результата.</typeparam>
  /// <typeparam name="E">Тип ошибки.</typeparam>
  public readonly partial struct Result<T, E> : IResult<T?, E?>
  {
    /// <summary>
    /// Признак получения ошибки выполнения операции
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

    private readonly T? _value;

    /// <summary>
    /// Получает значение успешного результата.
    /// </summary>
    /// <remarks>
    /// Доступно только для успешного результата.
    /// </remarks>
    public T? Value => IsSuccess ? _value : throw new ResultFailureException<E?>(_error);

    internal Result(bool isFailure, E? error, T? value)
    {
      IsFailure = ErrorStateGuard(isFailure, error);
      _error = error;
      _value = value;
    }
    /// <summary>
    /// Неявное преобразование типа Result в признак успешного или нет выполнения операции
    /// </summary>
    /// <param name="result"></param>
    public static implicit operator bool(Result<T, E?> result) => result.IsSuccess;

    /// <summary>
    /// Явное преобразование типа Result в тип результата выполнения операции
    /// </summary>
    /// <param name="result"></param>
    public static explicit operator T?(Result<T, E?> result) => result.IsSuccess ? result.Value : throw new ResultFailureException<E?>(result.Error);
  }
}