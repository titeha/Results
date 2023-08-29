namespace ResultType
{
  public partial struct Result
  {
    internal static class Messages
    {
      public static readonly string ErrorIsInaccessibleForSuccess = "Вы попытались получить доступ к свойству ошибки для успешного результата. Успешный результат не содержит ошибок.";

      public static readonly string ErrorObjectIsNotProvidedForFailure = "Вы попытались создать результат сбоя, в котором должна быть ошибка, но конструктору был передан нулевой объект ошибки (или пустая строка).";

      public static readonly string ErrorObjectIsProvidedForSuccess = "Вы попытались создать результат успеха, в котором не может быть ошибки, но конструктору был передан объект ошибки, отличный от нуля.";

      public static string ValueIsInaccessibleForFailure(string? error) => $"Вы попытались получить доступ к свойству Value с неудачным результатом. Неудачный результат не имеет никакого значения. Ошибка заключалась в: {error ?? string.Empty}";
    }
  }
}