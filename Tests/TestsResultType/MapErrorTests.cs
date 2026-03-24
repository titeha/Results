using FluentAssertions;
using ResultType;
using Xunit;

namespace TestsResultType;

public class MapErrorTests
{
  [Fact]
  public void Result_MapError_should_transform_string_error()
  {
    Result source = Result.Failure("boom");

    Result mapped = source.MapError(error => $"[{error}]");

    mapped.IsFailure.Should().BeTrue();
    mapped.Error.Should().Be("[boom]");
  }

  [Fact]
  public void Result_MapError_should_not_invoke_mapper_for_success()
  {
    Result source = Result.Success();
    bool called = false;

    Result mapped = source.MapError(error =>
    {
      called = true;
      return $"[{error}]";
    });

    mapped.IsSuccess.Should().BeTrue();
    called.Should().BeFalse();
  }

  [Fact]
  public void Result_MapErrorTo_should_convert_failure_to_typed_unit_result()
  {
    Result source = Result.Failure("boom");

    UnitResult<int> mapped = source.MapErrorTo(error => error?.Length ?? 0);

    mapped.IsFailure.Should().BeTrue();
    mapped.Error.Should().Be(4);
  }

  [Fact]
  public void Result_T_MapError_should_transform_string_error()
  {
    Result<int> source = Result.Failure<int>("boom");

    Result<int> mapped = source.MapError(error => error?.ToUpperInvariant());

    mapped.IsFailure.Should().BeTrue();
    mapped.Error.Should().Be("BOOM");
  }

  [Fact]
  public void Result_T_MapErrorTo_should_convert_error_type_and_preserve_value_on_success()
  {
    Result<int> source = Result.Success(42);

    Result<int, int> mapped = source.MapErrorTo(error => error?.Length ?? 0);

    mapped.IsSuccess.Should().BeTrue();
    mapped.Value.Should().Be(42);
  }

  [Fact]
  public void Result_T_E_MapError_should_transform_typed_error()
  {
    Result<int, string> source = Result.Failure<int, string>("boom");

    Result<int, string> mapped = source.MapError(error => $"[{error}]");

    mapped.IsFailure.Should().BeTrue();
    mapped.Error.Should().Be("[boom]");
  }

  [Fact]
  public void Result_T_E_MapErrorTo_should_convert_typed_error()
  {
    Result<int, string> source = Result.Failure<int, string>("boom");

    Result<int, int> mapped = source.MapErrorTo(error => error?.Length ?? 0);

    mapped.IsFailure.Should().BeTrue();
    mapped.Error.Should().Be(4);
  }

  [Fact]
  public void UnitResult_E_MapError_should_transform_typed_error()
  {
    UnitResult<string> source = UnitResult.Failure("boom")!;

    UnitResult<string> mapped = source.MapError(error => error?.ToUpperInvariant())!;

    mapped.IsFailure.Should().BeTrue();
    mapped.Error.Should().Be("BOOM");
  }

  [Fact]
  public void UnitResult_E_MapErrorTo_should_convert_typed_error()
  {
    UnitResult<string> source = UnitResult.Failure("boom")!;

    UnitResult<int> mapped = source.MapErrorTo(error => error?.Length ?? 0);

    mapped.IsFailure.Should().BeTrue();
    mapped.Error.Should().Be(4);
  }

  [Fact]
  public void UnitResult_E_MapErrorTo_should_not_invoke_mapper_for_success()
  {
    UnitResult<string> source = UnitResult.Success<string>()!;
    bool called = false;

    UnitResult<int> mapped = source.MapErrorTo(error =>
    {
      called = true;
      return error?.Length ?? 0;
    });

    mapped.IsSuccess.Should().BeTrue();
    called.Should().BeFalse();
  }
}
