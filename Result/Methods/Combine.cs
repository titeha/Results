namespace ResultType;

public partial struct Result
{
  /// <summary>
  /// Объединяет несколько результатов без значений в один итоговый результат.
  /// </summary>
  /// <param name="results">Последовательность результатов.</param>
  /// <param name="errorMessageSeparator">Разделитель между сообщениями ошибок.</param>
  /// <returns>Успех, если все результаты успешны; иначе — неуспех с агрегированным сообщением.</returns>
  public static Result Combine(IEnumerable<Result> results, string errorMessageSeparator = "\n")
  {
    ArgumentNullException.ThrowIfNull(results);

    List<string>? failures = null;

    foreach (Result result in results)
    {
      if (result.IsFailure)
      {
        failures ??= [];
        failures.Add(result.Error!);
      }
    }

    return failures is null
      ? Success()
      : Failure(string.Join(errorMessageSeparator, failures));
  }

  /// <summary>
  /// Объединяет несколько типизированных результатов со строковыми ошибками,
  /// возвращая массив успешных значений.
  /// </summary>
  public static Result<T[]> Combine<T>(IEnumerable<Result<T>> results, string errorMessageSeparator = "\n")
  {
    ArgumentNullException.ThrowIfNull(results);

    List<T> values = [];
    List<string>? failures = null;

    foreach (Result<T> result in results)
    {
      if (result.IsFailure)
      {
        failures ??= [];
        failures.Add(result.Error!);
        continue;
      }

      values.Add(result.Value!);
    }

    return failures is null
      ? Success(values.ToArray())
      : Failure<T[]>(string.Join(errorMessageSeparator, failures));
  }

  /// <summary>
  /// Объединяет несколько типизированных результатов с типизированными ошибками,
  /// возвращая массив успешных значений или массив ошибок.
  /// </summary>
  public static Result<T[], E[]> Combine<T, E>(IEnumerable<Result<T, E>> results)
  {
    ArgumentNullException.ThrowIfNull(results);

    List<T> values = [];
    List<E>? failures = null;

    foreach (Result<T, E> result in results)
    {
      if (result.IsFailure)
      {
        failures ??= [];
        failures.Add(result.Error!);
        continue;
      }

      values.Add(result.Value!);
    }

    return failures is null
      ? Success<T[], E[]>([.. values])
      : Failure<T[], E[]>([.. failures]);
  }

  /// <summary>
  /// Объединяет несколько unit-результатов с типизированными ошибками,
  /// возвращая общий успех или массив ошибок.
  /// </summary>
  public static UnitResult<E[]?> Combine<E>(IEnumerable<UnitResult<E>> results)
  {
    ArgumentNullException.ThrowIfNull(results);

    List<E>? failures = null;

    foreach (UnitResult<E> result in results)
    {
      if (result.IsFailure)
      {
        failures ??= [];
        failures.Add(result.Error!);
      }
    }

    return failures is null
      ? UnitResult.Success<E[]>()
      : UnitResult.Failure(failures.ToArray());
  }
}
