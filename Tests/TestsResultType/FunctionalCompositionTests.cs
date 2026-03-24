using FluentAssertions;
using ResultType;
using Xunit;

namespace TestsResultType;

public class FunctionalCompositionTests
{
  [Fact]
  public void Match_for_non_generic_result_should_choose_success_branch()
  {
    Result result = Result.Success();

    string actual = result.Match(
      onSuccess: () => "ok",
      onFailure: error => error ?? "error");

    actual.Should().Be("ok");
  }

  [Fact]
  public void Match_for_generic_result_should_choose_failure_branch()
  {
    Result<int> result = Result.Failure<int>("boom");

    string actual = result.Match(
      onSuccess: value => value.ToString() ?? "null",
      onFailure: error => error ?? "error");

    actual.Should().Be("boom");
  }

  [Fact]
  public void Map_should_transform_success_value()
  {
    Result<int> source = Result.Success(21);

    Result<int> mapped = source.Map(x => x * 2);

    mapped.IsSuccess.Should().BeTrue();
    mapped.Value.Should().Be(42);
  }

  [Fact]
  public void Map_should_preserve_failure_for_generic_result()
  {
    Result<int> source = Result.Failure<int>("boom");

    Result<string> mapped = source.Map(x => $"{x}");

    mapped.IsFailure.Should().BeTrue();
    mapped.Error.Should().Be("boom");
  }

  [Fact]
  public void Map_should_transform_typed_success_value()
  {
    Result<int, string> source = Result.Success<int, string>(10);

    Result<string, string> mapped = source.Map(x => $"#{x}");

    mapped.IsSuccess.Should().BeTrue();
    mapped.Value.Should().Be("#10");
  }

  [Fact]
  public void Bind_should_chain_generic_success_results()
  {
    Result<int> source = Result.Success(5);

    Result<int> bound = source.Bind(x => Result.Success(x + 10));

    bound.IsSuccess.Should().BeTrue();
    bound.Value.Should().Be(15);
  }

  [Fact]
  public void Bind_should_not_invoke_binder_on_failure()
  {
    Result<int> source = Result.Failure<int>("boom");
    bool called = false;

    Result<int> bound = source.Bind(x =>
    {
      called = true;
      return Result.Success(x + 10);
    });

    called.Should().BeFalse();
    bound.IsFailure.Should().BeTrue();
    bound.Error.Should().Be("boom");
  }

  [Fact]
  public void Bind_should_chain_typed_results()
  {
    Result<int, string> source = Result.Success<int, string>(3);

    Result<int, string> bound = source.Bind(x => Result.Success<int, string>(x * 7));

    bound.IsSuccess.Should().BeTrue();
    bound.Value.Should().Be(21);
  }

  [Fact]
  public void Ensure_should_keep_success_when_predicate_is_true()
  {
    Result<int> source = Result.Success(42);

    Result<int> ensured = source.Ensure(x => x > 0, "must be positive");

    ensured.IsSuccess.Should().BeTrue();
    ensured.Value.Should().Be(42);
  }

  [Fact]
  public void Ensure_should_convert_success_to_failure_when_predicate_is_false()
  {
    Result<int> source = Result.Success(-1);

    Result<int> ensured = source.Ensure(x => x >= 0, "must be non-negative");

    ensured.IsFailure.Should().BeTrue();
    ensured.Error.Should().Be("must be non-negative");
  }

  [Fact]
  public void Ensure_should_preserve_existing_failure()
  {
    Result<int> source = Result.Failure<int>("boom");

    Result<int> ensured = source.Ensure(x => x > 0, "must be positive");

    ensured.IsFailure.Should().BeTrue();
    ensured.Error.Should().Be("boom");
  }

  [Fact]
  public void Tap_should_execute_action_for_success_only()
  {
    Result<int> source = Result.Success(10);
    int captured = 0;

    Result<int> tapped = source.Tap(x => captured = x);

    tapped.IsSuccess.Should().BeTrue();
    captured.Should().Be(10);
  }

  [Fact]
  public void Tap_should_not_execute_action_for_failure()
  {
    Result<int> source = Result.Failure<int>("boom");
    int captured = 0;

    Result<int> tapped = source.Tap(x => captured = 1);

    tapped.IsFailure.Should().BeTrue();
    captured.Should().Be(0);
  }

  [Fact]
  public void TapError_should_execute_action_for_failure_only()
  {
    Result<int> source = Result.Failure<int>("boom");
    string? captured = null;

    Result<int> tapped = source.TapError(error => captured = error);

    tapped.IsFailure.Should().BeTrue();
    captured.Should().Be("boom");
  }

  [Fact]
  public void TapError_should_not_execute_action_for_success()
  {
    Result<int> source = Result.Success(5);
    string? captured = null;

    Result<int> tapped = source.TapError(error => captured = error);

    tapped.IsSuccess.Should().BeTrue();
    captured.Should().BeNull();
  }

  [Fact]
  public void UnitResult_map_should_create_typed_success_result()
  {
    UnitResult<string> source = UnitResult.Success<string>()!;

    Result<int, string> mapped = source.Map(() => 123);

    mapped.IsSuccess.Should().BeTrue();
    mapped.Value.Should().Be(123);
  }

  [Fact]
  public void UnitResult_ensure_should_convert_success_to_failure()
  {
    UnitResult<string> source = UnitResult.Success<string>()!;

    UnitResult<string> ensured = source.Ensure(() => false, "boom")!;

    ensured.IsFailure.Should().BeTrue();
    ensured.Error.Should().Be("boom");
  }
}
