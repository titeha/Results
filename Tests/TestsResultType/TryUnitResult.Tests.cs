using System.Threading.Tasks;

using FluentAssertions;

using ResultType;

using Xunit;

namespace TestsResultType;

public class TryUnitResultTests : TryTestBase
{
  [Fact]
  public void ResultTry_E_execute_action_success_result_expected()
  {
    UnitResult<E?> result = Result.Try(Action, ErrorHandlerE);

    result.IsSuccess.Should().BeTrue();
    result.IsFailure.Should().BeFalse();
    FuncExecuted.Should().BeTrue();
  }

  [Fact]
  public void ResultTry_E_execute_action_failed_failed_result_expected()
  {
    UnitResult<E?> result = Result.Try(Throwing_Action, ErrorHandlerE);

    result.IsFailure.Should().BeTrue();
    result.Error.Should().Be(E.Value);
  }

  [Fact]
  public async Task ResultTry_Async_E_execute_action_success_result_expected()
  {
    UnitResult<E?> result = await Result.Try(Func_Task, ErrorHandlerE);

    result.IsSuccess.Should().BeTrue();
    result.IsFailure.Should().BeFalse();
    FuncExecuted.Should().BeTrue();
  }

  [Fact]
  public async Task ResultTry_Async_E_execute_action_failed_failed_result_expected()
  {
    UnitResult<E?> result = await Result.Try(Throwing_Func_Task, ErrorHandlerE);

    result.IsFailure.Should().BeTrue();
    result.Error.Should().Be(E.Value);
  }
}
