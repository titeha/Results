using System;
using System.Linq;

using FluentAssertions;

using ResultType;

using Xunit;

namespace TestsResultType;

public class CombineTests
{
  [Fact]
  public void Combine_non_generic_all_success_returns_success()
  {
    Result[] results =
    [
      Result.Success(),
      Result.Success(),
      Result.Success(),
    ];

    Result combined = Result.Combine(results);

    combined.IsSuccess.Should().BeTrue();
    combined.IsFailure.Should().BeFalse();
  }

  [Fact]
  public void Combine_non_generic_failures_returns_joined_error_message()
  {
    Result[] results =
    [
      Result.Success(),
      Result.Failure("first"),
      Result.Failure("second"),
    ];

    Result combined = Result.Combine(results, "; ");

    combined.IsFailure.Should().BeTrue();
    combined.Error.Should().Be("first; second");
  }

  [Fact]
  public void Combine_generic_string_error_all_success_returns_values_in_original_order()
  {
    Result<int>[] results =
    [
      Result.Success(1),
      Result.Success(2),
      Result.Success(3),
    ];

    Result<int[]> combined = Result.Combine(results);

    combined.IsSuccess.Should().BeTrue();
    combined.Value.Should().Equal([1, 2, 3]);
  }

  [Fact]
  public void Combine_generic_string_error_any_failure_returns_joined_error_message()
  {
    Result<int>[] results =
    [
      Result.Success(1),
      Result.Failure<int>("bad 2"),
      Result.Failure<int>("bad 3"),
    ];

    Result<int[]> combined = Result.Combine(results, " | ");

    combined.IsFailure.Should().BeTrue();
    combined.Error.Should().Be("bad 2 | bad 3");
  }

  [Fact]
  public void Combine_generic_typed_error_all_success_returns_values_in_original_order()
  {
    Result<int, MyError>[] results =
    [
      Result.Success<int, MyError>(10),
      Result.Success<int, MyError>(20),
      Result.Success<int, MyError>(30),
    ];

    Result<int[], MyError[]> combined = Result.Combine(results);

    combined.IsSuccess.Should().BeTrue();
    combined.Value.Should().Equal([10, 20, 30]);
  }

  [Fact]
  public void Combine_generic_typed_error_any_failure_returns_all_errors()
  {
    Result<int, MyError>[] results =
    [
      Result.Success<int, MyError>(10),
      Result.Failure<int, MyError>(new MyError("A")),
      Result.Failure<int, MyError>(new MyError("B")),
    ];

    Result<int[], MyError[]> combined = Result.Combine(results);

    combined.IsFailure.Should().BeTrue();
    combined.Error.Should().NotBeNull();
    combined.Error!.Select(x => x.Code).Should().Equal(["A", "B"]);
  }

  [Fact]
  public void Combine_unit_result_typed_error_all_success_returns_success()
  {
    UnitResult<MyError>[] results =
    [
      UnitResult.Success<MyError>()!,
      UnitResult.Success<MyError>()!,
    ];

    UnitResult<MyError[]> combined = Result.Combine(results)!;

    combined.IsSuccess.Should().BeTrue();
    combined.IsFailure.Should().BeFalse();
  }

  [Fact]
  public void Combine_unit_result_typed_error_any_failure_returns_all_errors()
  {
    UnitResult<MyError>[] results =
    [
      UnitResult.Success<MyError>()!,
      UnitResult.Failure(new MyError("A"))!,
      UnitResult.Failure(new MyError("B"))!,
    ];

    UnitResult<MyError[]> combined = Result.Combine(results)!;

    combined.IsFailure.Should().BeTrue();
    combined.Error.Should().NotBeNull();
    combined.Error!.Select(x => x.Code).Should().Equal(["A", "B"]);
  }

  [Fact]
  public void Combine_empty_non_generic_collection_returns_success()
  {
    Result combined = Result.Combine([]);

    combined.IsSuccess.Should().BeTrue();
  }

  [Fact]
  public void Combine_empty_generic_collection_returns_success_with_empty_value_array()
  {
    Result<int[]> combined = Result.Combine(Array.Empty<Result<int>>());

    combined.IsSuccess.Should().BeTrue();
    combined.Value.Should().BeEmpty();
  }

  private sealed class MyError(string code)
  {
    public string Code { get; } = code;
  }
}
