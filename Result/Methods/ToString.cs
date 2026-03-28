namespace ResultType
{
  public partial struct Result
  {
    /// <summary>
    /// Преобразование признака результата в строку
    /// </summary>
    /// <returns>Строка с признаком результата выполнения операции</returns>
    public override string ToString() => IsSuccess ? "Успех" : $"Неудача({Error})";
  }

  public partial struct Result<T>
  {
    /// <summary>
    /// Преобразование признака результата в строку
    /// </summary>
    /// <returns>Строка с признаком результата выполнения операции</returns>
    public override string ToString() => IsSuccess ? $"Успех({Value})" : $"Неудача({Error})";
  }

  public partial struct Result<T, E>
  {
    /// <summary>
    /// Преобразование признака результата в строку
    /// </summary>
    /// <returns>Строка с признаком результата выполнения операции</returns>
    public override string ToString() => IsSuccess ? $"Успех({Value})" : $"Неудача({Error})";
  }

  public partial struct UnitResult<E>
  {
    /// <summary>
    /// Преобразование признака результата в строку
    /// </summary>
    /// <returns>Строка с признаком результата выполнения операции</returns>
    public override string ToString() => IsSuccess ? "Успех" : $"Неудача({Error})";
  }
}