using System.Threading.Tasks;

using FluentAssertions;

using ResultType;

using Xunit;

namespace TestsResultType
{
 public class TryAsyncTests : TryTestBase
 {
  [Fact]
  public async Task ResultTry_Async_execute_action_failed_without_error_handler_function_result_expected()
  {
   var result = await Result.Try(Func_Task);

   result.IsSuccess.Should().Be(true);
   FuncExecuted.Should().Be(true);
  }

  [Fact]
  public async Task ResultTry_Async_execute_action_failed_without_error_handler_failed_result_expected()
  {
   var result = await Result.Try(Throwing_Func_Task);

   result.IsFailure.Should().Be(true);
   result.Error.Should().Be(ErrorMessage);
  }

  [Fact]
  public async Task ResultTry_Async_execute_action_failed_with_error_handler_failed_result_expected()
  {
   var result = await Result.Try(Throwing_Func_Task, ErrorHandler);

   result.IsFailure.Should().Be(true);
   result.Error.Should().Be(ErrorHandlerMessage);
  }

  [Fact]
  public async Task ResultTry_Async_T_execute_function_success_without_error_handler_function_result_expected()
  {
   var result = await Result.Try(Func_Task_T);

   result.IsSuccess.Should().Be(true);
   result.Value.Should().Be(T.Value);
   FuncExecuted.Should().Be(true);
  }

  [Fact]
  public async Task ResultTry_Async_T_E_execute_function_failed_without_error_handler_failed_result_expected()
  {
   var result = await Result.Try(Throwing_Func_Task_T, ErrorHandlerE);

   result.IsFailure.Should().Be(true);
   result.Error.Should().Be(E.Value);
  }

  [Fact]
  public async Task ResultTry_Async_T_E_execute_function_failed_with_error_handler_failed_result_expected()
  {
   var result = await Result.Try(Throwing_Func_Task_T, ErrorHandlerE);

   result.IsFailure.Should().Be(true);
   result.Error.Should().Be(E.Value);
  }
 }
}