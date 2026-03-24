using System;
using System.Threading.Tasks;
using FluentAssertions;
using ResultType;
using Xunit;

namespace TestsResultType;

public class AsyncCompositionTests
{
  [Fact]
  public async Task Result_MatchAsync_OnSuccess_ExecutesSuccessBranch()
  {
    var result = await Result.Success().MatchAsync(
      onSuccess: () => Task.FromResult("ok"),
      onFailure: error => Task.FromResult($"fail:{error}"));

    result.Should().Be("ok");
  }

  [Fact]
  public async Task ResultT_MatchAsync_OnFailure_ExecutesFailureBranch()
  {
    var result = await Result.Failure<int>("boom").MatchAsync(
      onSuccess: value => Task.FromResult($"ok:{value}"),
      onFailure: error => Task.FromResult($"fail:{error}"));

    result.Should().Be("fail:boom");
  }

  [Fact]
  public async Task ResultT_MapAsync_OnSuccess_MapsValue()
  {
    var result = await Result.Success(10).MapAsync(value => Task.FromResult(value * 2));

    result.IsSuccess.Should().BeTrue();
    result.Value.Should().Be(20);
  }

  [Fact]
  public async Task ResultT_MapAsync_OnFailure_DoesNotExecuteMapper()
  {
    bool executed = false;

    var result = await Result.Failure<int>("boom").MapAsync(value =>
    {
      executed = true;
      return Task.FromResult(value * 2);
    });

    result.IsFailure.Should().BeTrue();
    result.Error.Should().Be("boom");
    executed.Should().BeFalse();
  }

  [Fact]
  public async Task ResultT_BindAsync_OnSuccess_BindsNextResult()
  {
    var result = await Result.Success(10).BindAsync(value => Task.FromResult(Result.Success(value + 5)));

    result.IsSuccess.Should().BeTrue();
    result.Value.Should().Be(15);
  }

  [Fact]
  public async Task ResultT_BindAsync_OnFailure_DoesNotExecuteBinder()
  {
    bool executed = false;

    var result = await Result.Failure<int>("boom").BindAsync(value =>
    {
      executed = true;
      return Task.FromResult(Result.Success(value + 5));
    });

    result.IsFailure.Should().BeTrue();
    result.Error.Should().Be("boom");
    executed.Should().BeFalse();
  }

  [Fact]
  public async Task ResultTE_MapAsync_PreservesTypedError()
  {
    var result = await Result.Failure<int, int>(42).MapAsync(value => Task.FromResult(value + 1));

    result.IsFailure.Should().BeTrue();
    result.Error.Should().Be(42);
  }

  [Fact]
  public async Task UnitResultE_BindAsync_OnSuccess_ReturnsTypedResult()
  {
    var result = await UnitResult.Success<int>().BindAsync(() => Task.FromResult(Result.Success<string, int>("ok")));

    result.IsSuccess.Should().BeTrue();
    result.Value.Should().Be("ok");
  }

  [Fact]
  public async Task TaskResultT_MapAsync_AllowsFluentAsyncChain()
  {
    Task<Result<int>> source = Task.FromResult(Result.Success(5));

    var result = await source.MapAsync(value => Task.FromResult(value * 3));

    result.IsSuccess.Should().BeTrue();
    result.Value.Should().Be(15);
  }

  [Fact]
  public async Task TaskResultT_BindAsync_AllowsFluentAsyncChain()
  {
    Task<Result<int>> source = Task.FromResult(Result.Success(5));

    var result = await source.BindAsync(value => Task.FromResult(Result.Success(value * 3)));

    result.IsSuccess.Should().BeTrue();
    result.Value.Should().Be(15);
  }

  [Fact]
  public async Task TaskResultT_MatchAsync_AllowsFluentAsyncChain()
  {
    Task<Result<int>> source = Task.FromResult(Result.Success(5));

    var text = await source.MatchAsync(
      onSuccess: value => Task.FromResult($"ok:{value}"),
      onFailure: error => Task.FromResult($"fail:{error}"));

    text.Should().Be("ok:5");
  }

  [Fact]
  public async Task ResultTaskExtensions_PropagateFailureWithoutInvokingDelegate()
  {
    bool executed = false;
    Task<Result<int>> source = Task.FromResult(Result.Failure<int>("boom"));

    var result = await source.BindAsync(value =>
    {
      executed = true;
      return Task.FromResult(Result.Success(value * 2));
    });

    result.IsFailure.Should().BeTrue();
    result.Error.Should().Be("boom");
    executed.Should().BeFalse();
  }
}
