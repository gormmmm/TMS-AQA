# Выпускное задание «MiniAutomationToolkit»

## Общая цель

Разработать на C# небольшую библиотеку вспомогательных компонентов для проекта автоматизации и консольное приложение, которое демонстрирует их работу.

В ходе выполнения задания необходимо:

- создать решение из библиотеки классов и консольного приложения;
- применить коллекции, LINQ, ООП, исключения, обобщения, методы расширения и асинхронность;
- научиться читать и записывать обычные текстовые файлы;
- фиксировать завершённые этапы в Git;
- обеспечить понятный вывод результатов и ошибок в консоль.

## Общие требования

- Используйте актуальную LTS-версию .NET.
- Все проекты должны собираться командой `dotnet build`.
- Все демонстрационные сценарии должны запускаться командой `dotnet run --project MiniAutomationToolkit.App`.
- Бизнес-логика должна находиться в `MiniAutomationToolkit.Core`.
- В `MiniAutomationToolkit.App` должны находиться только подготовка входных данных, вызовы методов и вывод результатов.
- Не оставляйте пустые шаблонные файлы и неиспользуемый код.
- Имена классов, методов и файлов оформляйте на английском языке.
- Каждое из 11 заданий выполняйте отдельным осмысленным коммитом.

---

## Блок 1. Структура решения и Git

### Задание 1. «Скелет проекта и первый коммит»

Вы присоединились к команде автоматизации. Нужно с нуля создать структуру решения, в которой библиотека содержит переиспользуемую логику, а консольное приложение показывает её работу.

#### Задача

Через IDE создайте в папке `C:\Projects\MiniAutomationToolkit` решение `MiniAutomationToolkit`, состоящее из:

- библиотеки классов `MiniAutomationToolkit.Core`;
- консольного приложения `MiniAutomationToolkit.App`.

Консольное приложение должно ссылаться на библиотеку и при запуске выводить `MiniAutomationToolkit started`. В решении не должно остаться пустых шаблонных классов.

Подготовьте `.gitignore` для Visual Studio и C#, исключив из репозитория `bin`, `obj` и временные файлы IDE. Инициализируйте Git-репозиторий через интерфейс системы контроля версий и зафиксируйте готовую структуру коммитом `Initial solution structure`.

Результат должен собираться и запускаться средствами IDE без ошибок.

#### Файлы, с которыми ведётся работа

- `MiniAutomationToolkit.sln`
- `MiniAutomationToolkit.Core\MiniAutomationToolkit.Core.csproj`
- `MiniAutomationToolkit.App\MiniAutomationToolkit.App.csproj`
- `MiniAutomationToolkit.App\Program.cs`
- `.gitignore`

---

## Блок 2. Логика и коллекции

### Задание 2. «Калькулятор скидок»

Интернет-магазину нужен модуль расчёта скидок в зависимости от типа клиента и суммы заказа.

#### Задача

В файле `MiniAutomationToolkit.Core\Models\ClientType.cs` создайте `enum ClientType` со значениями `Regular`, `Premium` и `Vip`.

В файле `MiniAutomationToolkit.Core\Services\DiscountCalculator.cs` реализуйте класс `DiscountCalculator` с методом:

```csharp
public static decimal CalculateDiscount(
    decimal orderAmount,
    ClientType clientType)
```

Метод должен возвращать сумму скидки в рублях и соблюдать следующие бизнес-правила:

- `Vip` — 15% при любой сумме;
- `Premium` — 5%, а при сумме больше 1000 рублей — 10%;
- `Regular` — 0%, а при сумме больше 1000 рублей — 5%.

Отрицательная сумма заказа считается недопустимой и должна приводить к `ArgumentOutOfRangeException`. Для выбора правила используйте `switch`-выражение, а не цепочку вложенных `if`.

В `MiniAutomationToolkit.App\Program.cs` продемонстрируйте работу калькулятора на разных типах клиентов, суммах ниже и выше 1000, а также на граничном значении 1000. Формат вывода:

```text
Client: Premium, amount: 1500, discount: 150
```

Ожидаемые результаты:

- `Vip`, 500 → 75;
- `Vip`, 2000 → 300;
- `Premium`, 800 → 40;
- `Premium`, 1000 → 50;
- `Premium`, 1500 → 150;
- `Regular`, 500 → 0;
- `Regular`, 1500 → 75;
- `Regular`, 1000 → 0.

#### Файлы, с которыми ведётся работа

- `MiniAutomationToolkit.Core\Models\ClientType.cs`
- `MiniAutomationToolkit.Core\Services\DiscountCalculator.cs`
- `MiniAutomationToolkit.App\Program.cs`

### Задание 3. «Поиск в хаосе»

Цель задания — закрепить работу с LINQ (`Where`, `FirstOrDefault`, `Any`) и обобщённой коллекцией `List<string>`.

#### Задача

В файле `MiniAutomationToolkit.App\Program.cs` создайте локальную переменную `fileNames` типа `List<string>` и добавьте в неё 20 перемешанных имён снимков экрана, логов и текстовых файлов, например:

```text
screen_001.png
error_2024.log
screen_002.png
debug.txt
```

В файле `MiniAutomationToolkit.Core\Helpers\FileSearcher.cs` создайте статический класс `FileSearcher` и реализуйте в нём метод:

```csharp
public static string FindFirstScreenshot(List<string> fileNames)
```

Метод должен вернуть первое имя с расширением `.png`, не учитывая регистр расширения. Для решения обязательно используйте `Where`, `Any` и `FirstOrDefault`.

Если подходящих имён нет, метод должен выбросить `FileNotFoundException` с сообщением:

```text
No screenshots found in the provided list.
```

В `MiniAutomationToolkit.App\Program.cs` передайте `fileNames` в `FindFirstScreenshot` и выведите найденное имя. Там же создайте вторую локальную переменную `fileNamesWithoutScreenshots` типа `List<string>`, не содержащую `.png`, и продемонстрируйте обработку `FileNotFoundException`.

#### Файлы, с которыми ведётся работа

- `MiniAutomationToolkit.Core\Helpers\FileSearcher.cs`
- `MiniAutomationToolkit.App\Program.cs`

---

## Блок 3. ООП и исключения

### Задание 4. «Неизменяемый пользователь»

Приложению нужна модель пользователя, состояние которой нельзя изменить после создания. Объект должен сразу проверять корректность имени и email, чтобы в программе не появлялись некорректные данные.

#### Задача

В файле `MiniAutomationToolkit.Core\Models\UserDto.cs` реализуйте `record UserDto` с неизменяемыми свойствами `Name` и `Email`. Оба значения передаются при создании объекта и должны сразу проходить валидацию.

Требования к данным:

- имя не должно быть пустым (используйте `string.IsNullOrWhiteSpace()`);
- email не должен быть пустым (используйте `string.IsNullOrWhiteSpace()`);
- email должен содержать символ `@` (используйте `.Contains()`);
- в email не должно быть пробелов (используйте `.Contains()`).

Некорректные данные должны приводить к `ArgumentException`. Для ошибки email используйте сообщение:

```text
Invalid email: <полученное значение>
```

В `MiniAutomationToolkit.App\Program.cs` покажите:

- успешное создание пользователя `Alex Smith` с email `alex@example.com`;
- равенство двух объектов с одинаковыми значениями;
- невозможность изменить свойства уже созданного объекта.

Некорректные данные для демонстрации:

- пустое имя и корректный email;
- корректное имя и пустой email;
- корректное имя и email без символа `@`;
- корректное имя и email с пробелом.

Ошибочные сценарии не должны завершать работу приложения: выведите сообщения перехваченных исключений.

#### Файлы, с которыми ведётся работа

- `MiniAutomationToolkit.Core\Models\UserDto.cs`
- `MiniAutomationToolkit.App\Program.cs`

### Задание 5. «Базовая страница»

Вы разрабатываете основу Page Object Model. Каждая страница имеет адрес и отображаемое имя, а общая логика загрузки не должна дублироваться.

#### Задача

В файле `MiniAutomationToolkit.Core\Pages\BasePage.cs` создайте абстрактный класс `BasePage` со следующими членами:

```csharp
public abstract string Url { get; }
public abstract string PageName { get; }
```

```csharp
public virtual void Load()
```

Метод `Load` должен выводить:

```text
Loading page: <PageName> at <Url>
```

Создайте два наследника:

- `MiniAutomationToolkit.Core\Pages\LoginPage.cs` — класс `LoginPage`: URL `/login`, имя `Login Page`;
- `MiniAutomationToolkit.Core\Pages\HomePage.cs` — класс `HomePage`: URL `/home`, имя `Home Page`.

В `MiniAutomationToolkit.App\Program.cs` объедините обе страницы в `List<BasePage>`, вызовите для них `Load` и проверьте уникальность URL с помощью LINQ. При наличии дубликатов должно возникать `InvalidOperationException`, иначе приложение должно выводить `All page URLs are unique`.

#### Файлы, с которыми ведётся работа

- `MiniAutomationToolkit.Core\Pages\BasePage.cs`
- `MiniAutomationToolkit.Core\Pages\LoginPage.cs`
- `MiniAutomationToolkit.Core\Pages\HomePage.cs`
- `MiniAutomationToolkit.App\Program.cs`

---

## Блок 4. Обобщения и методы расширения

### Задание 6. «Умная конфигурация»

Приложение читает параметры запуска из текстового файла `key=value`. Значения хранятся как строки, но вызывающий код должен получать их в нужном типе.

#### Формат входного файла

Создайте `MiniAutomationToolkit.App\data\appsettings.txt`:

```text
baseUrl=https://demo.example.com
timeout=30
headless=true
retryCount=3
```

#### Задача

В файле `MiniAutomationToolkit.Core\Configuration\AppConfig.cs` реализуйте класс `AppConfig`. Он должен принимать путь к конфигурационному файлу, загружать параметры в приватный словарь `_settings` и предоставлять доступ к значениям через обобщённый метод:

```csharp
public T GetSetting<T>(string key)
```

Требования к разбору файла:

- игнорируйте пустые строки (используйте `string.IsNullOrWhiteSpace()`);
- игнорируйте строки, начинающиеся с `#` (используйте `.TrimStart()` и `.StartsWith()`);
- разделяйте строку на ключ и значение по первому символу `=` (используйте `.Split()` с ограничением количества частей до двух);
- удаляйте пробелы по краям ключа и значения (используйте `.Trim()`);
- для пустого ключа или строки без `=` выбрасывайте `InvalidDataException` (проверьте количество частей после `.Split()` и используйте `string.IsNullOrWhiteSpace()`);
- повторяющийся ключ считайте ошибкой конфигурации и выбрасывайте `InvalidDataException`.

Значение должно преобразовываться к запрошенному типу через `Convert.ChangeType`. Отсутствующий ключ должен приводить к `KeyNotFoundException`. Если значение невозможно преобразовать, выбрасывайте `InvalidDataException`; сообщение должно содержать ключ и ожидаемый тип.

В `MiniAutomationToolkit.App\Program.cs` получите и выведите все параметры из примера в соответствующих типах, а также покажите обработку отсутствующего ключа.

#### Файлы, с которыми ведётся работа

- `MiniAutomationToolkit.Core\Configuration\AppConfig.cs`
- `MiniAutomationToolkit.App\data\appsettings.txt`
- `MiniAutomationToolkit.App\Program.cs`

### Задание 7. «Расширяем возможности строк»

В проекте часто требуется определить, начинается ли строка с поддерживаемой схемы веб-адреса. Эту операцию удобно оформить как метод расширения.

#### Задача

В файле `MiniAutomationToolkit.Core\Extensions\StringExtensions.cs` реализуйте статический класс `StringExtensions` с методом расширения:

```csharp
public static bool HasHttpScheme(this string? input)
```

Поведение метода:

- вернуть `false`, если строка равна `null`, пустая или состоит из пробелов;
- вернуть `true`, если строка начинается с `http://` или `https://`;
- выполнять сравнение без учёта регистра;
- использовать `StartsWith` и `StringComparison.OrdinalIgnoreCase`.

Продемонстрируйте работу метода в `MiniAutomationToolkit.App\Program.cs` на значениях:

```text
https://google.com          → true
http://example.org          → true
ftp://files.example.com     → false
<null>                      → false
HTTPS://SITE.EXAMPLE.COM    → true
```

`HasHttpScheme` должен вызываться как метод экземпляра строки. Для каждого входного значения выведите исходную строку и результат.

#### Файлы, с которыми ведётся работа

- `MiniAutomationToolkit.Core\Extensions\StringExtensions.cs`
- `MiniAutomationToolkit.App\Program.cs`

---

## Блок 5. Асинхронность и обработка ошибок

### Задание 8. «Имитация длительной операции»

Приложение может ждать ответа сервиса или завершения фоновой операции. На этом этапе нужно сравнить блокирующее ожидание с асинхронным.

#### Задача

В файле `MiniAutomationToolkit.Core\Simulations\LongOperationSimulator.cs` реализуйте класс `LongOperationSimulator` с двумя вариантами одной длительной операции:

```csharp
public string LongOperation()
```

```csharp
public async Task<string> LongOperationAsync()
```

Синхронный вариант должен блокировать поток на две секунды через `Thread.Sleep`, асинхронный — ожидать две секунды через `Task.Delay`. Оба метода возвращают `Done`.

В `MiniAutomationToolkit.App\Program.cs` вызовите асинхронный вариант через `await` и выведите результат вместе с длительностью выполнения, измеренной через `Stopwatch`. Использовать `.Result` и `.Wait()` нельзя.

#### Файлы, с которыми ведётся работа

- `MiniAutomationToolkit.Core\Simulations\LongOperationSimulator.cs`
- `MiniAutomationToolkit.App\Program.cs`

### Задание 9. «Логгер ошибок»

При чтении файлов могут возникать ожидаемые ошибки. Приложение должно записывать сведения о таких ошибках в отдельный текстовый лог-файл.

#### Задача

В файле `MiniAutomationToolkit.Core\Services\ErrorLogger.cs` реализуйте класс `ErrorLogger` с методом:

```csharp
public string? TryReadFile(
    string sourceFilePath,
    string logFilePath)
```

Метод должен прочитать файл целиком с помощью `File.ReadAllText` и вернуть его содержимое. Следующие ожидаемые ошибки не должны аварийно завершать приложение:

- `FileNotFoundException`;
- `UnauthorizedAccessException`.

При такой ошибке метод должен вернуть `null`, а сведения об исключении — дописать в текстовый файл по пути `logFilePath` в формате:

```text
<дата и время> | <тип исключения> | <сообщение>
```

Каждую запись сохраняйте отдельной строкой. Лог должен создаваться автоматически и дополняться через `File.AppendAllText`, не теряя предыдущие записи. Обрабатывайте только перечисленные типы исключений.

В `MiniAutomationToolkit.App\data\input.txt` добавьте несколько строк произвольного текста.

В `MiniAutomationToolkit.App\Program.cs` покажите два сценария:

- чтение существующего файла `MiniAutomationToolkit.App\data\input.txt`;
- попытка чтения отсутствующего файла `MiniAutomationToolkit.App\data\missing.txt`.

Для лога используйте `MiniAutomationToolkit.App\data\errors.log`. После ошибочного сценария выведите содержимое созданного лог-файла.

#### Файлы, с которыми ведётся работа

- `MiniAutomationToolkit.Core\Services\ErrorLogger.cs`
- `MiniAutomationToolkit.App\data\input.txt`
- `MiniAutomationToolkit.App\data\errors.log`
- `MiniAutomationToolkit.App\Program.cs`

---

## Блок 6. Итоговая сборка компонентов

### Задание 10. «Защитный валидатор»

Публичные методы библиотеки должны получать корректные аргументы. Повторяющиеся проверки удобно вынести в отдельный вспомогательный класс с понятным исключением.

#### Задача

В файле `MiniAutomationToolkit.Core\Validation\ValidationException.cs` создайте собственное исключение `ValidationException`, наследующееся от `Exception`. Исключение должно принимать текст сообщения через конструктор.

В файле `MiniAutomationToolkit.Core\Validation\Guard.cs` создайте статический класс `Guard` с методом:

```csharp
public static void EnsurePositive(
    int number,
    string parameterName = "number")
```

Метод должен пропускать положительные числа без ошибки, а для нуля и отрицательных значений выбрасывать `ValidationException` с сообщением:

```text
Validation failed: <parameterName> must be positive. Value: <number>
```

В `MiniAutomationToolkit.App\Program.cs` продемонстрируйте поведение для `5`, `-5` и `0`.

#### Файлы, с которыми ведётся работа

- `MiniAutomationToolkit.Core\Validation\ValidationException.cs`
- `MiniAutomationToolkit.Core\Validation\Guard.cs`
- `MiniAutomationToolkit.App\Program.cs`

### Задание 11. «Склад товаров»

Интернет-магазин выгружает каталог в CSV-файл. Нужно прочитать товары, преобразовать строки в объекты и с помощью LINQ выбрать доступные продукты указанной категории.

#### Формат входного файла

Создайте `MiniAutomationToolkit.App\data\products.csv`:

```text
Name;Price;Category
Laptop;1200;Electronics
Mouse;25;Electronics
Bread;2;Food
Milk;1;Food
Cheese;5;Food
T-Shirt;15;Clothing
Novel;12;Books
```

#### Задача

В `MiniAutomationToolkit.Core\Models` создайте:

- в файле `ProductCategory.cs` — `enum ProductCategory` со значениями `Electronics`, `Food`, `Clothing` и `Books`;
- в файле `Product.cs` — `record Product`, содержащий название, цену типа `decimal` и категорию.

В файле `MiniAutomationToolkit.Core\Repositories\ProductRepository.cs` создайте статический класс `ProductRepository` и реализуйте два метода:

```csharp
public static List<Product> LoadFromCsv(string filePath)
```

```csharp
public static List<string> GetAffordableProducts(
    IEnumerable<Product> products,
    ProductCategory category,
    decimal maxPrice)
```

`LoadFromCsv` должен загрузить корректные товары из `products.csv`. Заголовок и пустые строки не являются товарами. Каждая строка данных должна содержать три непустых поля, разделённые `;` (используйте метод `string.Split()`). Удаляйте пробелы по краям каждого поля с помощью `.Trim()`. Цена не может быть отрицательной. Для преобразования цены используйте `decimal.TryParse`, для категории — `Enum.TryParse` без учёта регистра. Некорректная строка должна приводить к `InvalidDataException` с физическим номером строки в CSV-файле, где заголовок считается первой строкой.

`GetAffordableProducts` должен одной LINQ-цепочкой:

- выбрать указанную категорию;
- оставить товары с ценой строго меньше `maxPrice`;
- отсортировать товары по цене, затем по названию;
- вернуть не объекты `Product`, а `List<string>`, содержащий только значения свойства `Name` отобранных товаров (используйте `Select` и `ToList`).

Использовать `foreach` внутри `GetAffordableProducts` нельзя.

В `MiniAutomationToolkit.App\Program.cs` выведите количество загруженных товаров и результаты выборки категории `Food` для бюджетов 10 и 1. Для пустого результата выведите `No products found`.

#### Файлы, с которыми ведётся работа

- `MiniAutomationToolkit.Core\Models\ProductCategory.cs`
- `MiniAutomationToolkit.Core\Models\Product.cs`
- `MiniAutomationToolkit.Core\Repositories\ProductRepository.cs`
- `MiniAutomationToolkit.App\data\products.csv`
- `MiniAutomationToolkit.App\Program.cs`

---

## Что необходимо сдать

Ссылку на Git-репозиторий.
