using System;
using System.Threading.Tasks;

namespace TestsResultType
{
  public abstract class TryTestBase : TestBase
  {
    protected TryTestBase()
    {
      FuncExecuted = false;
    }

    protected static readonly Exception Exception = new(ErrorMessage);
    protected const string ErrorHandlerMessage = "Error message from error handler";
    protected static readonly Func<Exception, string> ErrorHandler = exc => ErrorHandlerMessage;
    protected static readonly Func<Exception, E> ErrorHandlerE = exc => E.Value;

    protected bool FuncExecuted;

    protected void Action() => FuncExecuted = true;

    protected void Action_T(T _) => FuncExecuted = true;

    protected static T Throwing_Func_T() => throw Exception;

    protected static K Throwing_Func_T_K(T _) => throw Exception;

    protected static void Throwing_Action() => throw Exception;

    protected static void Throwing_Action_T(T _) => throw Exception;

    protected T Func_T()
    {
      FuncExecuted = true;
      return T.Value;
    }

    protected Task Func_Task()
    {
      FuncExecuted = true;
      return Task.CompletedTask;
    }
    protected static Task Throwing_Func_Task() => Exception.AsTask();

    protected Task<T> Func_Task_T()
    {
      FuncExecuted = true;
      return T.Value.AsTask();
    }

    protected static Task<T> Throwing_Func_Task_T() => Exception.AsTask<T>();
  }
}