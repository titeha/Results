using static ResultType.Internals.ResultCommonLogic;

namespace ResultType
{
  /// <summary>
  /// Представляет результат выполнения операции без возвращаемого значения
  /// со строковым описанием ошибки.
  /// </summary>
  /// <remarks>
  /// <para>
  /// Используйте <see cref="Result"/>, когда операция либо завершается успешно,
  /// либо возвращает строковое сообщение об ошибке, но не производит полезного значения.
  /// </para>
  /// <para>
  /// Для создания экземпляров рекомендуется использовать фабрики <c>Success</c>, <c>Failure</c>,
  /// <c>Try</c> и <c>Combine</c>.
  /// </para>
  /// </remarks>
  public readonly partial struct Result(bool isFailure, string? error) : IResult
  {
    /// <summary>
    /// Признак ошибки при выполнении операции
    /// </summary>
    public bool IsFailure { get; } = ErrorStateGuard(isFailure, error);

    /// <summary>
    /// Признак успешного выполнения операции
    /// </summary>
    public bool IsSuccess => !IsFailure;

    private readonly string? _error = error;

    /// <summary>
    /// Строковое описание ошибки.
    /// </summary>
    /// <remarks>
    /// Доступно только для неуспешного результата.
    /// </remarks>
    public string? Error => GetErrorWithSuccessGuard(IsFailure, _error);

    /// <summary>
    /// Неявное преобразование типа Result в признак успешного или нет выполнения операции
    /// </summary>
    /// <param name="result">Значение результата</param>
    public static implicit operator bool(Result result) => result.IsSuccess;
  }
}
