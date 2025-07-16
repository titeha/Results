namespace ResultType;

public partial struct Result
{
  public static Result Combine(IEnumerable<Result> results, string errorMessageSeparator = "\n")
  {
    List<string> failedResult = [];

    foreach (var result in results.ToList())
      if (result.IsFailure)
        failedResult.Add(result.Error!);

    return failedResult.Count == 0
      ? Success()
      : Failure(string.Join(errorMessageSeparator, failedResult));
  }
}