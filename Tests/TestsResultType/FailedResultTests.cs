using FluentAssertions;

using ResultType;

using System;

using Xunit;

namespace TestsResultType
{
  public class FailedResultTests
  {
    private const string _errorMessage = "Error message";

    [Fact]
    public void Can_create_a_non_generic_vareion()
    {
      Result result = Result.Failure(_errorMessage);

      result.Error.Should().Be(_errorMessage);
      result.IsFailure.Should().BeTrue();
      result.IsSuccess.Should().BeFalse();
    }

    [Fact]
    public void Can_create_a_generic_version()
    {
      Result<MyClass> result = Result.Failure<MyClass>(_errorMessage);

      result.Error.Should().Be(_errorMessage);
      result.IsFailure.Should().Be(true);
      result.IsSuccess.Should().Be(false);
    }

    [Fact]
    public void Can_create_a_unit_version()
    {
      var _error = new MyErrorClass();

      UnitResult<MyErrorClass> result = UnitResult.Failure(_error);

      result.Error.Should().Be(_error);
      result.IsFailure.Should().Be(true);
      result.IsSuccess.Should().Be(false);
    }

    [Fact]
    public void Cannot_access_Value_property()
    {
      Result<MyClass> result = Result.Failure<MyClass>(_errorMessage);

      Action action = () => { MyClass myClass = result.Value; };

      action.Should().Throw<ResultFailureException>().WithMessage("Вы попытались получить доступ к свойству Value с неудачным результатом. Неудачный результат не имеет никакого значения. Ошибка заключалась в: Error message");
    }

    [Fact]
    public void Cannot_access_Value_property_with_a_generic_error()
    {
      Result<MyClass, MyErrorClass> result = Result.Failure<MyClass, MyErrorClass>(new MyErrorClass());

      Action action = () => { MyClass myClass = result.Value; };

      action.Should().Throw<ResultFailureException<MyErrorClass>>();
    }

    [Fact]
    public void Cannot_create_without_error_message()
    {
      Action action1 = () => { Result.Failure(null); };
      Action action2 = () => { Result.Failure(string.Empty); };
      Action action3 = () => { Result.Failure<MyClass>(null); };
      Action action4 = () => { Result.Failure<MyClass>(string.Empty); };
      Action action5 = () => { UnitResult.Failure<MyClass>(null); };

      action1.Should().Throw<ArgumentNullException>();
      action2.Should().Throw<ArgumentNullException>();
      action3.Should().Throw<ArgumentNullException>();
      action4.Should().Throw<ArgumentNullException>();
      action5.Should().Throw<ArgumentNullException>();
    }

    private class MyClass { }

    private class MyErrorClass { }
  }
}