namespace ResultType;

public partial struct Result
{
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
      ? Success<T[], E[]>(values.ToArray())
      : Failure<T[], E[]>(failures.ToArray());
  }

  public static UnitResult<E[]> Combine<E>(IEnumerable<UnitResult<E>> results)
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
