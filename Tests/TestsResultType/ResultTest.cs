using System;

using FluentAssertions;

using ResultType;

using Xunit;

namespace TestsResultType
{
  public class ResultTest
  {
    [Fact]
    public void Fail_argument_is_default_Fail_result_expected()
    {
      Result<string, int> result = Result.Failure<string, int>(0);

      result.IsFailure.Should().BeTrue();
    }

    [Fact]
    public void Fail_argument_is_not_deafult_Fail_result_expected()
    {
      Result<string, int> result = Result.Failure<string, int>(1);

      result.IsFailure.Should().BeTrue();
    }

    [Fact]
    public void Fail_argument_is_null_Exception_expected()
    {
      var exception = Record.Exception(() => Result.Failure<string, string>(null));

      Assert.IsType<ArgumentNullException>(exception);
    }

    [Fact]
    public void Can_work_with_nulable_structs()
    {
      Result<DateTime?> result = Result.Success((DateTime?)null);

      result.IsSuccess.Should().BeTrue();
      result.Value.Should().Be(null);
    }
  }
}