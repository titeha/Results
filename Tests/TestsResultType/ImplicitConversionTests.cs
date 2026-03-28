using System;

using FluentAssertions;

using Microsoft.CSharp.RuntimeBinder;

using ResultType;

using Xunit;

namespace TestsResultType;

public class ImplicitConversionTests
{
  [Fact]
  public void Result_of_dynamic_cannot_be_cast_to_other_generic_result()
  {
    dynamic source = Result.Success("test");

    Action act = () =>
    {
      Result<object> _ = source;
    };

    act.Should().Throw<RuntimeBinderException>();
  }

  [Fact]
  public void Result_TE_value_of_dynamic_cannot_be_cast_to_other_generic_result()
  {
    dynamic source = Result.Success<string, MyError?>("test");

    Action act = () =>
    {
      Result<object, MyError?> _ = source;
    };

    act.Should().Throw<RuntimeBinderException>();
  }

  [Fact]
  public void Result_TE_error_of_dynamic_cannot_be_cast_to_other_generic_result()
  {
    dynamic source = Result.Failure<string, MyError?>(new MyError());

    Action act = () =>
    {
      Result<string, object> _ = source;
    };

    act.Should().Throw<RuntimeBinderException>();
  }

  [Fact]
  public void Result_T_value_can_be_upcast_explicitly_via_map()
  {
    Result<string> source = Result.Success("test");

    Result<object> actual = source.Map(value => (object)value!);

    actual.IsSuccess.Should().BeTrue();
    actual.Value.Should().Be("test");
  }

  [Fact]
  public void Result_TE_value_can_be_upcast_explicitly_via_map()
  {
    Result<string, MyError> source = Result.Success<string, MyError>("test");

    Result<object, MyError> actual = source.Map(value => (object)value!);

    actual.IsSuccess.Should().BeTrue();
    actual.Value.Should().Be("test");
  }

  [Fact]
  public void Result_TE_error_can_be_upcast_explicitly_via_map_error_to()
  {
    var error = new MyError();
    Result<string, MyError> source = Result.Failure<string, MyError>(error);

    Result<string, object> actual = source.MapErrorTo<object>(e => e!);

    actual.IsFailure.Should().BeTrue();
    actual.Error.Should().Be(error);
  }

  [Fact]
  public void IResult_T_can_be_used_covariantly()
  {
    IResult<ICovariantResult> covariantResult = Result.Success(new CovariantResult());

    covariantResult.IsSuccess.Should().BeTrue();
    covariantResult.Value.Should().BeOfType<CovariantResult>();
  }

  [Fact]
  public void IResult_TE_value_can_be_used_covariantly()
  {
    IResult<ICovariantResult, string> covariantResult =
        Result.Success<ICovariantResult, string>(new CovariantResult());

    covariantResult.IsSuccess.Should().BeTrue();
    covariantResult.Value.Should().BeOfType<CovariantResult>();
  }

  [Fact]
  public void IResult_TE_error_can_be_used_covariantly()
  {
    IResult<string, IMyError> covariantResult =
        Result.Failure<string, MyError?>(new MyError());

    covariantResult.IsFailure.Should().BeTrue();
    covariantResult.Error.Should().BeOfType<MyError?>();
  }

  [Fact]
  public void Implicit_conversion_Result_success_to_bool_returns_true()
  {
    bool actual = Result.Success();

    actual.Should().BeTrue();
  }

  [Fact]
  public void Implicit_conversion_Result_failure_to_bool_returns_false()
  {
    bool actual = Result.Failure("boom");

    actual.Should().BeFalse();
  }

  [Fact]
  public void Implicit_conversion_Result_T_success_to_bool_returns_true()
  {
    Result<int> result = Result.Success(42);

    bool actual = result;

    actual.Should().BeTrue();
    result.Value.Should().Be(42);
  }

  [Fact]
  public void Implicit_conversion_Result_T_failure_to_bool_returns_false()
  {
    Result<int> result = Result.Failure<int>("boom");

    bool actual = result;

    actual.Should().BeFalse();
    result.Error.Should().Be("boom");
  }

  [Fact]
  public void Implicit_conversion_Result_TE_success_to_bool_returns_true()
  {
    Result<int, MyError?> result = Result.Success<int, MyError?>(42);

    bool actual = result;

    actual.Should().BeTrue();
    result.Value.Should().Be(42);
  }

  [Fact]
  public void Implicit_conversion_Result_TE_failure_to_bool_returns_false()
  {
    var error = new MyError();
    Result<int, MyError?> result = Result.Failure<int, MyError?>(error);

    bool actual = result;

    actual.Should().BeFalse();
    result.Error.Should().Be(error);
  }

  [Fact]
  public void Implicit_conversion_UnitResult_success_to_bool_returns_true()
  {
    bool actual = UnitResult.Success<MyError?>();

    actual.Should().BeTrue();
  }

  [Fact]
  public void Implicit_conversion_UnitResult_failure_to_bool_returns_false()
  {
    var error = new MyError();
    UnitResult<MyError?> result = UnitResult.Failure(error);

    bool actual = result;

    actual.Should().BeFalse();
    result.Error.Should().Be(error);
  }

  [Fact]
  public void Result_of_bool_success_false_still_converts_to_true_in_if_semantics()
  {
    Result<bool> result = Result.Success(false);

    bool actual = result;

    actual.Should().BeTrue();
    result.Value.Should().BeFalse();
  }

  private interface ICovariantResult;

  private interface IMyError;

  private class MyError : IMyError;

  private class CovariantResult : ICovariantResult;
}
