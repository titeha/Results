# Results

Лёгкая библиотека для представления успешного и неуспешного результата без использования исключений в ожидаемых сценариях.

Библиотека предоставляет типы `Result`, `Result<T>`, `Result<T, E>` и `UnitResult<E>`, а также фабрики `Success`/`Failure`, методы `Try`, `Combine` и compositional API (`Match`, `Map`, `Bind`, `Ensure`, `Tap`, `TapError`, `MapError`) в синхронном и асинхронном вариантах.

## Для чего нужна библиотека

`Results` позволяет:

- явно выражать успех/неуспех операции;
- избегать исключений там, где ошибка является ожидаемой частью доменной логики;
- строить линейные pipeline-цепочки без множества `if` и ручного проброса ошибок;
- удобно работать как со строковыми ошибками, так и с типизированными ошибками.

## Поддерживаемые платформы

Библиотека мультитаргетится на:

- `net6.0`
- `net7.0`
- `net8.0`
- `net9.0`
- `net10.0`

> Хотя `.NET 6` и `.NET 7` уже вышли из официальной поддержки Microsoft, они намеренно сохранены в библиотеке ради совместимости с существующими проектами.

## Основные типы

### `Result`

Неуспех/успех **без значения**, с ошибкой типа `string`.

```csharp
using ResultType;

Result result = Result.Success();
Result failed = Result.Failure("Something went wrong");
```

### `Result<T>`

Неуспех/успех **со значением**, с ошибкой типа `string`.

```csharp
Result<int> success = Result.Success(42);
Result<int> failure = Result.Failure<int>("Value was not produced");
```

### `Result<T, E>`

Неуспех/успех **со значением** и **типизированной ошибкой**.

```csharp
Result<int, string> success = Result.Success<int, string>(42);
Result<int, string> failure = Result.Failure<int, string>("Domain error");
```

### `UnitResult<E>`

Неуспех/успех **без значения**, но с **типизированной ошибкой**.

```csharp
UnitResult<string> success = UnitResult.Success<string>();
UnitResult<string> failure = UnitResult.Failure("Domain error");
```

## Базовые правила

У каждого результата есть два свойства состояния:

- `IsSuccess`
- `IsFailure`

Они взаимоисключающие.

### Доступ к `Value` и `Error`

В библиотеке намеренно защищаются инварианты объекта:

- `Value` доступно только для успешного результата;
- `Error` доступно только для неуспешного результата.

Нарушение этих правил приводит к специализированным исключениям библиотеки.

Это сделано специально: ошибка доступа к неправильной ветке результата считается ошибкой использования API, а не нормальным рабочим сценарием.

## Фабрики `Success` / `Failure`

```csharp
Result ok = Result.Success();
Result fail = Result.Failure("error");

Result<string> okValue = Result.Success("hello");
Result<string> failValue = Result.Failure<string>("error");

Result<int, string> typedOk = Result.Success<int, string>(10);
Result<int, string> typedFail = Result.Failure<int, string>("typed error");

UnitResult<string> unitOk = UnitResult.Success<string>();
UnitResult<string> unitFail = UnitResult.Failure("unit error");
```

## `Try`

`Try` позволяет обернуть код, который может выбросить исключение, в `Result`.

### Без возвращаемого значения

```csharp
Result result = Result.Try(() =>
{
    DoWork();
});
```

### С возвращаемым значением

```csharp
Result<int> result = Result.Try(() =>
{
    return int.Parse("42");
});
```

### С типизированной ошибкой

```csharp
Result<int, string> result = Result.Try(
    () => int.Parse("42"),
    ex => $"Parsing failed: {ex.Message}");
```

### Для `Action` / `Task` с типизированной ошибкой

```csharp
UnitResult<string> result = Result.Try(
    () => DoWork(),
    ex => $"Action failed: {ex.Message}");
```

```csharp
UnitResult<string> result = await Result.Try(
    async () => await DoWorkAsync(),
    ex => $"Async action failed: {ex.Message}");
```

## `Combine`

`Combine` нужен, когда есть несколько независимых результатов, и нужно:

- вернуть успех, если успешны все;
- собрать ошибки, если есть хотя бы один failure.

### Нетипизированный вариант

```csharp
var results = new[]
{
    Result.Success(),
    Result.Failure("Name is empty"),
    Result.Failure("Email is invalid")
};

Result combined = Result.Combine(results);
```

### `Result<T>`

```csharp
var results = new[]
{
    Result.Success(1),
    Result.Success(2),
    Result.Success(3)
};

Result<int[]> combined = Result.Combine(results);
```

### `Result<T, E>`

```csharp
var results = new[]
{
    Result.Success<int, string>(1),
    Result.Failure<int, string>("Invalid item")
};

Result<int[], string[]> combined = Result.Combine(results);
```

### `UnitResult<E>`

```csharp
var results = new[]
{
    UnitResult.Success<string>(),
    UnitResult.Failure("Validation failed")
};

UnitResult<string[]> combined = Result.Combine(results);
```

## Composition API

### `Match`

Используется как финальный выход из мира `Result`.

```csharp
string text = result.Match(
    onSuccess: value => $"OK: {value}",
    onFailure: error => $"ERROR: {error}");
```

### `Map`

Преобразует успешное значение, не затрагивая failure.

```csharp
Result<UserDto> dtoResult = userResult.Map(user => new UserDto(user));
```

### `Bind`

Используется, когда следующий шаг уже возвращает `Result`.

```csharp
Result<OrderDto> result = GetUser(id)
    .Bind(CheckAccess)
    .Map(user => new OrderDto(user));
```

### `Ensure`

Проверяет дополнительное условие над успехом.

```csharp
Result<string> result = GetName()
    .Ensure(name => !string.IsNullOrWhiteSpace(name), "Name is empty");
```

### `Tap`

Выполняет побочный эффект на успехе, не меняя результат.

```csharp
var result = CreateOrder(command)
    .Tap(order => logger.LogInformation("Created order {Id}", order.Id));
```

### `TapError`

Выполняет побочный эффект на ошибке.

```csharp
var result = CreateOrder(command)
    .TapError(error => logger.LogWarning("Order creation failed: {Error}", error));
```

### `MapError`

Преобразует ошибку, не меняя успешный результат.

```csharp
Result<User, string> domainResult = CreateUser(command);

Result<User, ApiError> apiResult = domainResult.MapErrorTo(error => new ApiError(error));
```

## Асинхронные версии

Библиотека поддерживает async-composition:

- `MatchAsync`
- `MapAsync`
- `BindAsync`
- `EnsureAsync`
- `TapAsync`
- `TapErrorAsync`

Они доступны:
- для самих `Result` / `Result<T>` / `Result<T,E>` / `UnitResult<E>`;
- и для `Task<Result...>` через extension methods.

### Пример async-pipeline

```csharp
var result = await LoadUserAsync(id)
    .BindAsync(CheckAccessAsync)
    .EnsureAsync(user => repository.IsActiveAsync(user.Id), "User is not active")
    .MapAsync(user => BuildDtoAsync(user))
    .TapAsync(dto => audit.WriteAsync(dto));
```

### Пример async-finalization

```csharp
var httpResult = await result.MatchAsync(
    onSuccess: value => Task.FromResult($"OK: {value}"),
    onFailure: error => Task.FromResult($"ERROR: {error}"));
```

## `DefaultConfigureAwait`

Для async-методов используется глобальная настройка `Result.DefaultConfigureAwait`.

Если она равна `true`, библиотека будет использовать `ConfigureAwait(true)`, если `false` — `ConfigureAwait(false)`.

```csharp
Result.DefaultConfigureAwait = false;
```

Это позволяет централизованно настроить поведение async-цепочек в зависимости от характера приложения.

## Когда использовать `Map`, а когда `Bind`

Очень короткое правило:

- `Map`: если шаг имеет форму `T -> K`
- `Bind`: если шаг имеет форму `T -> Result<K>`

Пример:

```csharp
// Map: обычное преобразование
Result<string> text = Result.Success(10)
    .Map(x => x.ToString());

// Bind: шаг сам может завершиться ошибкой
Result<int> parsed = Result.Success("42")
    .Bind(text => int.TryParse(text, out var value)
        ? Result.Success(value)
        : Result.Failure<int>("Parse failed"));
```

## Когда использовать `Result<T>`, а когда `Result<T, E>`

### `Result<T>`

Используй, если строковой ошибки достаточно.

Подходит для:
- небольших библиотек;
- validation layer;
- простых сервисов;
- пользовательских сообщений.

### `Result<T, E>`

Используй, если ошибка является полноценной частью модели.

Подходит для:
- domain errors;
- API error models;
- ошибок с кодом и метаданными;
- межслойных преобразований ошибок.

## Пример полного pipeline

```csharp
Result<OrderDto> result = Result.Try(() => LoadOrder(id))
    .Ensure(order => order is not null, "Order not found")
    .Bind(CheckPermissions)
    .Map(order => new OrderDto(order))
    .Tap(dto => logger.LogInformation("DTO prepared"));

string output = result.Match(
    onSuccess: dto => $"OK: {dto.Id}",
    onFailure: error => $"ERROR: {error}");
```

## Установка

Если библиотека опубликована как NuGet-пакет:

```bash
dotnet add package ResultType
```

Или подключи проект напрямую через `ProjectReference`.

## Лицензия

MIT
