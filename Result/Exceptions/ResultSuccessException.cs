namespace ResultType.Exceptions;

/// <summary>
/// Класс исключения типа результата
/// </summary>
public class ResultSuccessException : Exception
{
  internal ResultSuccessException() : base(Result.Messages.ErrorIsInaccessibleForSuccess) { }
}