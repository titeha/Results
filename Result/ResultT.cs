using ResultType.Exceptions;

using static ResultType.Internals.ResultCommonLogic;

namespace ResultType
{
  /// <summary>
  /// Представляет результат выполнения операции со значением успеха и строковой ошибкой.
  /// </summary>
  /// <typeparam name="T">Тип значения успешного результата.</typeparam>
  public readonly partial struct Result<T> : IResult<T?>
  {
    /// <summary>
    /// Признак неуспешного выполнения операции
    /// </summary>
    public bool IsFailure { get; }

    /// <summary>
    /// Признак успешного выполнения операции
    /// </summary>
    public bool IsSuccess => !IsFailure;

    private readonly string? _error;

    /// <summary>
    /// Получает строковое описание ошибки.
    /// </summary>
    /// <remarks>
    /// Доступно только для неуспешного результата.
    /// </remarks>
    public string? Error => GetErrorWithSuccessGuard(IsFailure, _error);

    private readonly T? _value;

    /// <summary>
    /// Получает значение успешного результата.
    /// </summary>
    /// <remarks>
    /// Доступно только для успешного результата.
    /// </remarks>
    public T? Value => IsSuccess ? _value : throw new ResultFailureException(Error);

    internal Result(bool isFailure, string? error, T? value)
    {
      IsFailure = ErrorStateGuard(isFailure, error);
      _error = error;
      _value = value;
    }

    /// <summary>
    /// Неявное преобразование типа Result в признак успешного или нет выполнения операции
    /// </summary>
    /// <param name="result">Значение результата</param>
    public static implicit operator bool(Result<T> result) => result.IsSuccess;

    /// <summary>
    /// Явное преобразование типа Result в тип результата выполнения операции
    /// </summary>
    /// <param name="result"></param>
    public static explicit operator T?(Result<T> result) => result.IsSuccess ? result.Value : throw new ResultFailureException(result.Error);
  }
}