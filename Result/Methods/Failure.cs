namespace ResultType
{
  public partial struct Result
  {
    /// <summary>
    /// Создаёт неуспешный результат без возвращаемого значения.
    /// </summary>
    /// <param name="error">Строковое описание ошибки.</param>
    public static Result Failure(string? error) => new(true, error);

    /// <summary>
    /// Создаёт неуспешный результат со строковой ошибкой.
    /// </summary>
    /// <typeparam name="T">Тип значения успешного результата.</typeparam>
    /// <param name="error">Строковое описание ошибки.</param>
    public static Result<T> Failure<T>(string? error) => new(true, error, default);

    /// <summary>
    /// Создаёт неуспешный результат со значением ошибки заданного типа.
    /// </summary>
    /// <typeparam name="T">Тип значения успешного результата.</typeparam>
    /// <typeparam name="E">Тип ошибки.</typeparam>
    /// <param name="error">Ошибка.</param>
    public static Result<T, E> Failure<T, E>(E? error) => new(true, error, default);
  }
}