namespace ResultType
{
  public partial struct Result
  {
    /// <summary>
    /// Создаёт успешный результат без возвращаемого значения.
    /// </summary>
    public static Result Success() => new(false, null);

    /// <summary>
    /// Создаёт успешный результат со значением.
    /// </summary>
    /// <typeparam name="T">Тип значения.</typeparam>
    /// <param name="value">Значение успешного результата.</param>
    public static Result<T> Success<T>(T value) => new(false, null, value);

    /// <summary>
    /// Создаёт успешный результат со значением и типизированной ошибкой.
    /// </summary>
    /// <typeparam name="T">Тип значения.</typeparam>
    /// <typeparam name="E">Тип ошибки.</typeparam>
    /// <param name="value">Значение успешного результата.</param>
    public static Result<T, E> Success<T, E>(T value) => new(false, default, value);

    /// <summary>
    /// Создаёт успешный unit-результат.
    /// </summary>
    public static UnitResult<E> Success<E>() => new(false, default);
  }
}