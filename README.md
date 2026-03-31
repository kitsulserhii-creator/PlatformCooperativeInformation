# Лабораторна робота №3 — Варіант 1

Опис
-
Лабораторної роботи №3 (варіант 1). Основний акцент — створення та використання:

- типізованих колекцій (`List<T>`, `Dictionary<K,V>`);
- інтерфейсів порівняння (`IComparable<T>`, `IComparer<T>`);
- методів для глибокого копіювання об'єктів (`DeepCopy()`);
- вимірювання продуктивності пошуку в колекціях.

Що реалізовано 
-
- `Person`:
  - властивості: `FirstName`, `LastName`, `BirthDate`;
  - реалізовано `IDateAndCopy` (властивість `Date`, метод `DeepCopy()`);
  - реалізовано `IComparable<Person>` — порівняння за прізвищем (для сортування);
  - реалізовано `IComparer<Person>` — порівняння двох `Person` за датою народження;
  - перевизначено `Equals`, `GetHashCode`, оператори `==`/`!=`.

- `Exam`:
  - властивості: `Subject`, `Score`, `Date` (`init`-only);
  - реалізує `IDateAndCopy` та `DeepCopy()`.

- `Test` (залік):
  - простий клас із полями `Subject`, `Passed` (bool) і `Date`;

- `Student`:
  - містить `Person` як поле `PersonData`;
  - поля: `List<Exam>` та `List<Test>`;
  - властивість `AverageGrade` (обчислюється по `Exams`);
  - `DeepCopy()` для створення повної копії (Person + копії екзаменів/заліків);
  - валідація `GroupNumber` у межах 100..699 (викидає `ArgumentOutOfRangeException`).

- `StudentAverageComparer` — реалізовано `IComparer<Student>` для сортування за `AverageGrade`.

- `StudentCollection`:
  - зберігає список студентів;
  - `AddDefaults()` — додає прикладні записи (для демонстрації);
  - сортування: `SortBySurname()`, `SortByBirthDate()`, `SortByAverage()`;
  - `MaxAverage` — максимальний середній бал у колекції;
  - `Masters` — підмножина студентів з `Education.Master`;
  - `AverageMarkGroup(double value)` — повертає студентів з груп, де середнє по групі >= value.

- `TestCollections`:
  - генерує колекції: `List<Person>`, `List<string>`, `Dictionary<Person, Student>`, `Dictionary<string, Student>`;
  - метод `MeasureSearches()` вимірює час операцій `Contains`, `ContainsKey`, `ContainsValue` для першого, середнього, останнього і неіснуючого елементів.

Як зібрати і запустити
-
1. Відкрити термінал у корені проекту (директорія з файлом `LabVariant1.sln`).
2. Побудувати рішення:

```bash
dotnet build LabVariant1.sln
```

3. Запустити програму (приклад):

```bash
dotnet run --project Variant1.csproj
```

Параметри запуску
-
Програма очікує у консолі введення двох чисел — `nRows` і `nColumns` — для демонстрації тесту продуктивності з масивами (приклад: `10 10`). Якщо пропущені, використовуються значення `10 10`.

Що виводиться під час запуску 
-
- Перевірка еквівалентності `Person` і хеш-кодів;
- Демонстрація `Student` + `DeepCopy()` та перевірка, що копія не змінюється при зміні оригіналу;
- Демонстрація `StudentCollection`: додавання дефолтних студентів, сортування, фільтрації, `MaxAverage`;
- Виклик `TestCollections.MeasureSearches()` з виводом часу пошуку для різних кейсів;
- Тест порівняння швидкості проходження по елементах для різних типів масивів (одновимірний, двовимірний, зубчастий).

Файли для перегляду (ключові):
- `Person.cs` — реалізація класу Person (порівняння, DeepCopy)
- `Student.cs` — реалізація класу Student (списки, DeepCopy, AverageGrade)
- `Exam.cs`, `Test.cs` — допоміжні класи
- `StudentCollection.cs`, `StudentAverageComparer.cs` — колекції і компаратори
- `TestCollections.cs` — вимірювання часу пошуку
- `Program.cs` — демонстрація всього функціоналу

