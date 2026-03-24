# ResultType — композиция результатов

## Назначение

Библиотека `ResultType` позволяет описывать успешный и неуспешный результат без исключений
в ожидаемых сценариях.

После добавления compositional API библиотека поддерживает не только создание `Result`,
но и построение линейных цепочек обработки через:

- `Match`
- `Map`
- `Bind`
- `Ensure`
- `Tap`
- `TapError`
- `MapError`
- `Combine`

А также асинхронные варианты:

- `MatchAsync`
- `MapAsync`
- `BindAsync`
- `EnsureAsync`
- `TapAsync`
- `TapErrorAsync`

---

## Базовые типы

- `Result` — успех/неуспех без значения, ошибка типа `string`
- `Result<T>` — успех/неуспех со значением типа `T`, ошибка типа `string`
- `Result<T, E>` — успех/неуспех со значением типа `T` и ошибкой типа `E`
- `UnitResult<E>` — успех/неуспех без значения, ошибка типа `E`

### Инварианты

- `IsSuccess == !IsFailure`
- `Value` доступно только у успешного результата
- `Error` доступно только у неуспешного результата

Нарушение этих правил приводит к специализированным исключениям библиотеки.

---

## Как мыслить о pipeline

Обычный рабочий поток выглядит так:

1. Получить `Result`
2. Проверить дополнительные условия через `Ensure`
3. Выполнить шаги, которые тоже возвращают `Result`, через `Bind`
4. Преобразовать значение через `Map`
5. Выполнить побочные эффекты через `Tap` / `TapError`
6. Завершить цепочку через `Match`

Пример:

```csharp
Result<OrderDto> result = LoadOrder(orderId)
    .Ensure(order => order is not null, "Заказ не найден")
    .Bind(EnsureOrderIsActive)
    .Map(order => new OrderDto(order!))
    .Tap(dto => logger.LogInformation("Подготовлен DTO заказа"));
```

---

## Match

`Match` завершает pipeline и превращает `Result` в обычное значение.

### Когда использовать

- в контроллере
- в UI
- в консольной команде
- на границе слоя, где нужно вернуть обычное значение

### Пример

```csharp
string text = result.Match(
    onSuccess: value => $"OK: {value}",
    onFailure: error => $"ERROR: {error}");
```

Для нетипизированного `Result`:

```csharp
string text = result.Match(
    onSuccess: () => "OK",
    onFailure: error => $"ERROR: {error}");
```

---

## Map

`Map` преобразует успешное значение, не меняя неуспешный результат.

### Когда использовать

Когда преобразование не является отдельной доменной операцией, способной вернуть ошибку.

### Пример

```csharp
Result<User> userResult = GetUser();
Result<UserDto> dtoResult = userResult.Map(user => new UserDto(user!));
```

### Правило

- `Map`: `T -> K`
- `Bind`: `T -> Result<K>`

Если функция сама возвращает `Result`, нужен `Bind`, а не `Map`.

---

## Bind

`Bind` присоединяет следующий шаг, который сам возвращает `Result`.

### Когда использовать

Когда следующий шаг:

- валидирует
- ищет данные
- выполняет доменную операцию
- и сам может завершиться ошибкой

### Пример

```csharp
Result<User> result = LoadUser(userId)
    .Bind(CheckPermissions)
    .Bind(EnsureUserIsActive);
```

Для typed-error:

```csharp
Result<User, DomainError> result = LoadUser(userId)
    .Bind(EnsureUserIsActive)
    .Bind(AttachProfile);
```

---

## Ensure

`Ensure` проверяет дополнительное условие над успешным результатом.
Если условие не выполнено, успех превращается в failure.

### Когда использовать

Для простых инвариантов:

- строка не пустая
- число больше нуля
- коллекция не пустая
- объект находится в допустимом состоянии

### Пример

```csharp
Result<string> result = GetName()
    .Ensure(name => !string.IsNullOrWhiteSpace(name), "Имя пустое");
```

Для typed-error:

```csharp
Result<User, DomainError> result = GetUser()
    .Ensure(user => user!.IsActive, DomainError.UserInactive);
```

---

## Tap

`Tap` выполняет побочный эффект только при успехе и возвращает тот же результат.

### Когда использовать

- логирование
- аудит
- метрики
- публикация события
- обновление кэша

### Пример

```csharp
var result = CreateOrder(command)
    .Tap(order => logger.LogInformation("Создан заказ {Id}", order!.Id));
```

---

## TapError

`TapError` выполняет побочный эффект только при ошибке и возвращает тот же результат.

### Когда использовать

- логирование ошибки
- запись в telemetry
- аудит неудачных операций

### Пример

```csharp
var result = CreateOrder(command)
    .TapError(error => logger.LogWarning("Ошибка создания заказа: {Error}", error));
```

---

## MapError

`MapError` преобразует ошибку, не изменяя успешный результат.
`MapErrorTo` позволяет изменить тип ошибки.

### Когда использовать

На границах слоёв:

- доменная ошибка -> API-модель ошибки
- внутренняя ошибка -> пользовательское сообщение
- строковая ошибка -> типизированная ошибка

### Примеры

```csharp
Result<int> result = Result.Failure<int>("boom");
Result<int> mapped = result.MapError(error => $"[{error}]");
```

```csharp
Result<int> result = Result.Failure<int>("boom");
Result<int, int> mapped = result.MapErrorTo(error => error?.Length ?? 0);
```

```csharp
Result<int, DomainError> result = GetValue();
Result<int, ApiError> mapped = result.MapErrorTo(ApiError.FromDomain);
```

---

## Combine

`Combine` агрегирует несколько независимых результатов.

### Когда использовать

- валидация нескольких полей
- набор независимых проверок
- пакетная обработка результатов

### Примеры

```csharp
var validationResults = new[]
{
    ValidateName(name),
    ValidateEmail(email),
    ValidateAge(age)
};

Result combined = Result.Combine(validationResults);
```

```csharp
Result<int[]> combinedValues = Result.Combine(new[]
{
    Result.Success(10),
    Result.Success(20),
    Result.Success(30)
});
```

```csharp
Result<int[], DomainError[]> combinedTyped = Result.Combine(new[]
{
    Result.Success<int, DomainError>(10),
    Result.Failure<int, DomainError>(DomainError.NotFound)
});
```

---

## Асинхронная композиция

Асинхронные методы нужны, когда в pipeline появляются:

- база данных
- HTTP-запросы
- файловая система
- очереди
- внешние сервисы

Без async-методов цепочка распадается на набор `await + if (IsFailure)`.

---

## MatchAsync

Асинхронный вариант `Match`.

```csharp
string text = await result.MatchAsync(
    onSuccess: value => Task.FromResult($"OK: {value}"),
    onFailure: error => Task.FromResult($"ERROR: {error}"));
```

---

## MapAsync

Асинхронный вариант `Map`.

```csharp
Result<UserDto> dtoResult = await userResult.MapAsync(async user =>
{
    var avatar = await avatarService.LoadAvatarAsync(user!.Id);
    return new UserDto(user, avatar);
});
```

---

## BindAsync

Асинхронный вариант `Bind`.

```csharp
Result<UserDto> result = await LoadUserAsync(userId)
    .BindAsync(CheckPermissionsAsync)
    .MapAsync(async user => new UserDto(user!));
```

Также поддерживается fluent-цепочка для `Task<Result<T>>` через extension methods,
что позволяет не делать `await` после каждого шага.

---

## EnsureAsync

Асинхронная проверка дополнительного условия.

```csharp
Result<User> result = await GetUserAsync(id)
    .EnsureAsync(user => repository.IsEmailUniqueAsync(user!.Email), "Email уже занят");
```

---

## TapAsync и TapErrorAsync

Асинхронные побочные эффекты.

```csharp
var result = await CreateOrderAsync(command)
    .TapAsync(order => audit.WriteAsync(order!.Id))
    .TapErrorAsync(error => logger.WriteAsync(error));
```

---

## Рекомендации по использованию

### Используй `Map`, когда:

- нужно преобразовать значение
- шаг сам по себе не возвращает `Result`

### Используй `Bind`, когда:

- следующий шаг уже возвращает `Result`
- нужно продолжить pipeline без вложенных `Result<Result<T>>`

### Используй `Ensure`, когда:

- нужно проверить инвариант
- failure можно выразить одним сообщением/ошибкой

### Используй `Tap`, когда:

- нужен side effect
- но смысл результата менять не нужно

### Используй `Match`, когда:

- нужно выйти из мира `Result` и получить обычное значение

---

## Замечания

- Для `Task<Result<...>>` доступны extension methods, чтобы строить fluent async-цепочки.
- Асинхронные методы используют `Result.DefaultConfigureAwait`.
- `Value` и `Error` по-прежнему защищены инвариантами и не предназначены для чтения в неверном состоянии.

