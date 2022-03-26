namespace TestsResultType
{
 public abstract class TestBase
 {
  protected const string ErrorMessage = "Error message";
  protected const string ErrorMessage2 = "Error message2";

  protected class T
  {
   public static readonly T Value = new T();

   public static readonly T Value2 = new T();
  }

  protected class K
  {
   public static readonly K Value = new K();
  }

  protected class E
  {
   public static readonly E Value = new E();

   public static readonly E Value2 = new E();
  }
 }
}