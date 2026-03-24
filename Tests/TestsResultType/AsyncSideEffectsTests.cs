using System;
using System.Threading.Tasks;
using FluentAssertions;
using ResultType;
using Xunit;

namespace TestsResultType;

public class AsyncSideEffectsTests
{
    [Fact]
    public async Task Result_TapAsync_executes_action_on_success()
    {
        bool executed = false;

        Result result = await Result.Success()
            .TapAsync(() =>
            {
                executed = true;
                return Task.CompletedTask;
            });

        result.IsSuccess.Should().BeTrue();
        executed.Should().BeTrue();
    }

    [Fact]
    public async Task Result_TapAsync_does_not_execute_action_on_failure()
    {
        bool executed = false;

        Result result = await Result.Failure("error")
            .TapAsync(() =>
            {
                executed = true;
                return Task.CompletedTask;
            });

        result.IsFailure.Should().BeTrue();
        executed.Should().BeFalse();
    }

    [Fact]
    public async Task Result_TapErrorAsync_executes_action_on_failure()
    {
        string? captured = null;

        Result result = await Result.Failure("boom")
            .TapErrorAsync(error =>
            {
                captured = error;
                return Task.CompletedTask;
            });

        result.IsFailure.Should().BeTrue();
        captured.Should().Be("boom");
    }

    [Fact]
    public async Task Result_EnsureAsync_returns_failure_when_predicate_is_false()
    {
        Result result = await Result.Success()
            .EnsureAsync(() => Task.FromResult(false), "validation failed");

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be("validation failed");
    }

    [Fact]
    public async Task Result_T_task_extensions_support_fluent_chain()
    {
        int tapped = 0;
        string? errorTapped = null;

        Result<int> result = await Task.FromResult(Result.Success(10))
            .EnsureAsync(value => Task.FromResult(value > 0), "must be positive")
            .TapAsync(value =>
            {
                tapped = value;
                return Task.CompletedTask;
            })
            .TapErrorAsync(error =>
            {
                errorTapped = error;
                return Task.CompletedTask;
            });

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(10);
        tapped.Should().Be(10);
        errorTapped.Should().BeNull();
    }

    [Fact]
    public async Task Result_T_EnsureAsync_preserves_failure_without_running_predicate()
    {
        bool predicateExecuted = false;

        Result<int> result = await Result.Failure<int>("error")
            .EnsureAsync(value =>
            {
                predicateExecuted = true;
                return Task.FromResult(true);
            }, "should not happen");

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be("error");
        predicateExecuted.Should().BeFalse();
    }

    [Fact]
    public async Task Result_TE_TapErrorAsync_uses_typed_error()
    {
        int? captured = null;

        Result<string, int> result = await Result.Failure<string, int>(42)
            .TapErrorAsync(error =>
            {
                captured = error;
                return Task.CompletedTask;
            });

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(42);
        captured.Should().Be(42);
    }

    [Fact]
    public async Task Result_TE_EnsureAsync_returns_typed_failure()
    {
        Result<int, string> result = await Result.Success<int, string>(5)
            .EnsureAsync(value => Task.FromResult(value > 10), "too small");

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be("too small");
    }

    [Fact]
    public async Task UnitResult_TapAsync_executes_action_on_success()
    {
        bool executed = false;

        var result = await UnitResult.Success<string>()
            .TapAsync(() =>
            {
                executed = true;
                return Task.CompletedTask;
            });

        result.IsSuccess.Should().BeTrue();
        executed.Should().BeTrue();
    }

    [Fact]
    public async Task UnitResult_EnsureAsync_returns_failure_when_predicate_fails()
    {
        var result = await UnitResult.Success<string>()
            .EnsureAsync(() => Task.FromResult(false), "unit failed");

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be("unit failed");
    }

    [Fact]
    public async Task UnitResult_task_extensions_support_fluent_chain()
    {
        bool tapped = false;

        UnitResult<string> result = await Task.FromResult(UnitResult.Success<string>())
            .EnsureAsync(() => Task.FromResult(true), "nope")
            .TapAsync(() =>
            {
                tapped = true;
                return Task.CompletedTask;
            });

        result.IsSuccess.Should().BeTrue();
        tapped.Should().BeTrue();
    }

    [Fact]
    public async Task EnsureAsync_throws_on_null_predicate()
    {
        Func<Task<bool>>? predicate = null;

        Func<Task> action = async () => await Result.Success().EnsureAsync(predicate!, "error");

        await action.Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task TapAsync_throws_on_null_action()
    {
        Func<Task>? callback = null;

        Func<Task> action = async () => await Result.Success().TapAsync(callback!);

        await action.Should().ThrowAsync<ArgumentNullException>();
    }
}
