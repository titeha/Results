using FluentAssertions;

using ResultType;

using Xunit;

namespace TestsResultType
{
  public class TryTests : TryTestBase
  {
    [Fact]
    public void ResultTry_execute_action_success_without_error_handler_function_result_expected()
    {
      var result = Result.Try(Action);

      result.IsSuccess.Should().Be(true);
      FuncExecuted.Should().Be(true);
    }

    [Fact]
    public void ResultTry_execute_action_failed_without_error_handler_failed_result_expected()
    {
      var result = Result.Try(Throwing_Action);

      result.IsFailure.Should().Be(true);
      result.Error.Should().Be(ErrorMessage);
    }

    [Fact]
    public void ResultTry_execute_action_failed_with_error_handler_failed_result_expected()
    {
      var result = Result.Try(Throwing_Action, ErrorHandler);

      result.IsFailure.Should().Be(true);
      result.Error.Should().Be(ErrorHandlerMessage);
    }

    [Fact]
    public void ResultTry_T_execute_function_success_without_error_handler_function_result_expected()
    {
      var result = Result.Try(Func_T);

      result.IsSuccess.Should().BeTrue();
      result.IsFailure.Should().BeFalse();
      FuncExecuted.Should().BeTrue();
    }

    [Fact]
    public void ResultTry_T_executed_function_failed_without_error_handler_failed_result_expected()
    {
      var result = Result.Try(Throwing_Func_T);

      result.IsFailure.Should().Be(true);
      result.Error.Should().Be(ErrorMessage);
    }

    [Fact]
    public void ResultTry_T_execute_function_failed_with_error_handler_failed_result_expected()
    {
      var result = Result.Try(Throwing_Func_T, ErrorHandler);

      result.IsFailure.Should().Be(true);
      result.Error.Should().Be(ErrorHandlerMessage);
    }

    [Fact]
    public void ResultTry_T_E_execute_function_success_without_error_handler_function_result_expected()
    {
      var result = Result.Try(Func_T, ErrorHandlerE);

      result.IsSuccess.Should().Be(true);
      result.IsFailure.Should().Be(false);
      FuncExecuted.Should().Be(true);
    }

    [Fact]
    public void ResultTry_T_E_execute_function_failed_without_error_handler_failed_result_expected()
    {
      var result = Result.Try(Throwing_Func_T, ErrorHandlerE);

      result.IsFailure.Should().Be(true);
      result.Error.Should().Be(E.Value);
    }

    [Fact]
    public void ResultTry_T_E_execute_function_failed_with_error_handler_failed_resul_expected()
    {
      var result = Result.Try(Throwing_Func_T, ErrorHandlerE);

      result.IsFailure.Should().Be(true);
      result.Error.Should().Be(E.Value);
    }
  }
}