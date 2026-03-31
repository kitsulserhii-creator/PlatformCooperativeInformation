# Лабораторна робота 8 — Варіант 1

Множення двох прямокутних матриць за допомогою **TPL Dataflow** — кожен рядок результату обчислюється у двоетапному конвеєрі.

## Задача

Дано матриці **A** (m×k) та **B** (k×n). Обчислити **C = A × B** (m×n), де кожен елемент `C[i,j]` визначається за формулою:

$$C[i,j] = \sum_{t=0}^{k-1} A[i,t] \cdot B[t,j]$$

Розміри можуть бути великими (наприклад, 1000×100 та 100×7000). Підтримується переривання обчислення через **Ctrl+C**.

## Архітектура рішення

```
TransformBlock<int, (int row, double[])>   ←  індекс рядка i
        ↓  PropagateCompletion
ActionBlock<(int row, double[])>           →  запис рядка в матрицю C
```

| Компонент | Клас | Призначення |
|---|---|---|
| Зберігання матриці | `Matrix` | Плоский масив `double[]` (row-major), індексатор `[i, j]` |
| Генерація | `MatrixGenerator` | `Generate()` — випадкова матриця, `Identity()` — одинична |
| Конвеєр | `DataFlowMatrixMultiplier` | `MultiplyAsync()` — TPL Dataflow пайплайн |

### Деталі конвеєра

- **`TransformBlock`** — обчислює один рядок `C[i, *]` паралельно (`MaxDegreeOfParallelism = ProcessorCount`).  
  Алгоритм «row-update» (зовнішній цикл по `k`): рядок `B[k, *]` читається суміжно в пам'яті — cache-friendly.
- **`ActionBlock`** — записує готовий рядок у результат (`MaxDegreeOfParallelism = 1`), уникаючи конкуренції за cache-line.
- **Backpressure**: `BoundedCapacity = dop × 2` на обох блоках обмежує кількість живих буферів рядків у пам'яті.
- **Скасування**: `CancellationToken` прокинутий у обидва блоки; Ctrl+C через `ConsoleCancelEventHandler`.

## Запуск

```powershell
dotnet build LabVariant1.sln
dotnet run --project Variant1.csproj
```

Програма запитає розміри матриць A та B (рядки і стовпці через пробіл або кому), виведе прев'ю верхнього лівого кута 6×6, виконає множення, покаже час та результат самоперевірки (`A × I = A`).

Для зупинки під час обчислення натисніть **Ctrl+C**.

## Структура файлів

```
src/
  Lab8/
    Matrix.cs                    — клас матриці (flat row-major layout)
    MatrixGenerator.cs           — генерація випадкових та одиничних матриць
    DataFlowMatrixMultiplier.cs  — TPL Dataflow конвеєр
  Program.cs                     — точка входу, демо Lab 8
```

Збірні артефакти з'являються в `bin/Debug/net10.0/` після збірки.