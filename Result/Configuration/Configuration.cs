namespace ResultType
{
  public partial struct Result
  {
    /// <summary>
    /// Определяет значение <c>ConfigureAwait</c>, которое по умолчанию используется
    /// во встроенных асинхронных методах библиотеки.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Если установлено значение <see langword="true"/>, библиотека будет использовать
    /// <c>ConfigureAwait(true)</c>.
    /// </para>
    /// <para>
    /// Если установлено значение <see langword="false"/>, библиотека будет использовать
    /// <c>ConfigureAwait(false)</c>.
    /// </para>
    /// <para>
    /// Значение применяется только к асинхронным методам, реализованным внутри библиотеки,
    /// и не влияет на пользовательский код напрямую.
    /// </para>
    /// </remarks>
    public static bool DefaultConfigureAwait;
  }
}