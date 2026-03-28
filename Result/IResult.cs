namespace ResultType
{
  /// <summary>
  /// Представляет общий контракт результата выполнения операции.
  /// </summary>
  /// <remarks>
  /// <para>
  /// Любой результат находится только в одном из двух состояний:
  /// успешном или неуспешном.
  /// </para>
  /// <para>
  /// Свойства <see cref="IsSuccess"/> и <see cref="IsFailure"/> взаимно исключают друг друга.
  /// </para>
  /// </remarks>
  public interface IResult
  {
    /// <summary>
    /// Возвращает признак ошибки.
    /// </summary>
    bool IsFailure { get; }

    /// <summary>
    /// Возвращает признак успешного выполнения операции.
    /// </summary>
    bool IsSuccess { get; }
  }

  /// <summary>
  /// Представляет результат, содержащий значение успешного выполнения.
  /// </summary>
  /// <typeparam name="T">Тип значения успешного результата.</typeparam>
  public interface IValue<out T> : IResult
  {
    /// <summary>
    /// Получает значение успешного результата.
    /// </summary>
    /// <remarks>
    /// Доступ к этому свойству допустим только для успешного результата.
    /// Попытка чтения у неуспешного результата приводит к исключению библиотеки.
    /// </remarks>
    T Value { get; }
  }

  /// <summary>
  /// Представляет результат, содержащий информацию об ошибке.
  /// </summary>
  /// <typeparam name="E">Тип ошибки.</typeparam>
  public interface IError<out E> : IResult
  {
    /// <summary>
    /// Получает ошибку неуспешного результата.
    /// </summary>
    /// <remarks>
    /// Доступ к этому свойству допустим только для неуспешного результата.
    /// Попытка чтения у успешного результата приводит к исключению библиотеки.
    /// </remarks>
    E? Error { get; }
  }

  /// <summary>
  /// Представляет результат, содержащий и значение успеха, и типизированную ошибку.
  /// </summary>
  /// <typeparam name="T">Тип значения успешного результата.</typeparam>
  /// <typeparam name="E">Тип ошибки.</typeparam>
  public interface IResult<out T, out E> : IResult, IValue<T>, IError<E>;

  /// <summary>
  /// Представляет результат со значением успешного выполнения и строковой ошибкой.
  /// </summary>
  /// <typeparam name="T">Тип значения успешного результата.</typeparam>
  public interface IResult<out T> : IResult<T, string>;

  /// <summary>
  /// Представляет результат без значения, но с типизированной ошибкой.
  /// </summary>
  /// <typeparam name="E">Тип ошибки.</typeparam>
  public interface IUnitResult<out E> : IResult, IError<E>;
}