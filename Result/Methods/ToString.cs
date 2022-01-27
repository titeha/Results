namespace ResultType
{
 public partial struct Result
 {
  public override string ToString() => IsSuccess ? "Успех" : $"Неудача({Error})";
 }

 public partial struct Result<T>
 {
  public override string ToString() => IsSuccess ? $"Успех({Value})" : $"Неудача({Error})";
 }

 public partial struct Result<T, E>
 {
  public override string ToString() => IsSuccess ? $"Успех({Value})" : $"Неудача({Error})";
 }

 public partial struct UnitResult<E>
 {
  public override string ToString() => IsSuccess ? "Успех" : $"Неудача({Error})";
 }
}