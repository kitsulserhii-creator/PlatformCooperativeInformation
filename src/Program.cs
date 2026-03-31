using System;
using System.Diagnostics;
using System.Threading;
using LabVariant1.Lab8;

namespace LabVariant1
{
    internal static class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("LabVariant1 demo");

            
            var p1 = new Person("Ivan", "Petrenko", new DateTime(2000, 5, 10));
            var p2 = new Person("Ivan", "Petrenko", new DateTime(2000, 5, 10));
            Console.WriteLine($"p1 reference equals p2: {ReferenceEquals(p1, p2)}");
            Console.WriteLine($"p1 == p2: {p1 == p2}");
            Console.WriteLine($"p1.Equals(p2): {p1.Equals(p2)}");
            Console.WriteLine($"p1 hash: {p1.GetHashCode()}, p2 hash: {p2.GetHashCode()}");

            
            var student = new Student(new Person("Olena", "Ivanova", new DateTime(1999, 3, 2)), Education.Bachelor, 202);
            student.AddExams(
                new Exam("Mathematics", 95, DateTime.Now.AddDays(-30)),
                new Exam("Physics", 88, DateTime.Now.AddDays(-20)),
                new Exam("Programming", 100, DateTime.Now.AddDays(-10))
            );
            Console.WriteLine("Original student");
            Console.WriteLine(student.ToString());

            
            var copy = (Student)student.DeepCopy();
            Console.WriteLine("Deep copy");
            Console.WriteLine(copy.ToString());

            
            if (student.Exams.Count > 0)
            {
                student.Exams[0].Score = 50;
            }
            Console.WriteLine("After modification");
            Console.WriteLine("Original");
            Console.WriteLine(student.ToString());
            Console.WriteLine("Copy");
            Console.WriteLine(copy.ToString());

            
            try
            {
                var bad = new Student(new Person("Bad", "Group", new DateTime(2001,1,1)), Education.Bachelor, 5);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Console.WriteLine($"Invalid group: {ex.Message}");
            }

            
            Console.WriteLine("--- StudentCollection demo ---");
            var coll = new StudentCollection();
            coll.AddDefaults();
            Console.WriteLine("Students (short):");
            Console.WriteLine(coll.ToShortString());

            Console.WriteLine("Sort by surname:");
            coll.SortBySurname();
            Console.WriteLine(coll.ToShortString());

            Console.WriteLine("Sort by birth date:");
            coll.SortByBirthDate();
            Console.WriteLine(coll.ToShortString());

            Console.WriteLine("Sort by average grade:");
            coll.SortByAverage();
            Console.WriteLine(coll.ToShortString());

            Console.WriteLine($"Max average: {coll.MaxAverage:F2}");
            Console.WriteLine("Masters in collection:");
            foreach (var s in coll.Masters) Console.WriteLine(s.ToShortString());

            Console.WriteLine("Students in groups where group-average >= 85:");
            var high = coll.AverageMarkGroup(85.0);
            foreach (var s in high) Console.WriteLine(s.ToShortString());

            
            Console.WriteLine("--- TestCollections demo ---");
            var tests = new TestCollections(1000);
            tests.MeasureSearches();

            var lab4 = new TestCollectionsLab4(1000);
            lab4.RunAll(5);

            Console.WriteLine("\n--- Lab5: StudentCollection with events demo ---");
            var coll1 = new StudentCollection() { Name = "GroupA" };
            var coll2 = new StudentCollection() { Name = "GroupB" };

            var journal1 = new Journal();
            var journal2 = new Journal();

            journal1.Subscribe(coll1);
            journal2.SubscribeReference(coll1);
            journal2.SubscribeCount(coll2);

            coll1.AddDefaults();
            coll1.AddStudents(new Student(new Person("Anna", "Kovalenko", new DateTime(2002,1,1)), Education.Bachelor, 303));
            coll1.Remove(1);
            coll1[0] = new Student(new Person("Replacement", "Student", new DateTime(2000,2,2)), Education.Master, 111);

            coll2.AddDefaults();
            coll2.AddStudents(new Student(new Person("Boris", "Bondar", new DateTime(2001,4,4)), Education.Bachelor, 404));
            coll2.Remove(0);

            Console.WriteLine("Journal 1 (subscribed to coll1 count+reference):");
            Console.WriteLine(journal1.ToString());
            Console.WriteLine("\nJournal 2 (subscribed to coll1 reference and coll2 count):");
            Console.WriteLine(journal2.ToString());

            
            Console.WriteLine("Enter nRows nCols:");
            var input = Console.ReadLine() ?? "10 10";
            var parts = input.Split(new[] { ' ', '\t', ',' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length < 2) parts = new[] { "10", "10" };
            if (!int.TryParse(parts[0], out var nRows)) nRows = 10;
            if (!int.TryParse(parts[1], out var nCols)) nCols = 10;

            int total = nRows * nCols;
            Console.WriteLine($"nRows={nRows}, nCols={nCols}, total={total}");

            
            var rnd = new Random(42);
            var oneD = new Exam[total];
            for (int i = 0; i < total; i++) oneD[i] = new Exam("Subject", rnd.Next(50, 101), DateTime.Now);

            var rect = new Exam[nRows, nCols];
            for (int i = 0; i < nRows; i++)
                for (int j = 0; j < nCols; j++)
                    rect[i, j] = new Exam("Subject", rnd.Next(50, 101), DateTime.Now);

            var jagged = new Exam[nRows][];
            for (int i = 0; i < nRows; i++)
            {
                jagged[i] = new Exam[nCols];
                for (int j = 0; j < nCols; j++) jagged[i][j] = new Exam("Subject", rnd.Next(50, 101), DateTime.Now);
            }

            

            int before, after;

            before = Environment.TickCount;
            for (int i = 0; i < total; i++)
            {
                oneD[i].Score += 1;
            }
            after = Environment.TickCount;
            Console.WriteLine($"One-dimensional array time (ms): {after - before}");

            before = Environment.TickCount;
            for (int i = 0; i < nRows; i++)
                for (int j = 0; j < nCols; j++)
                    rect[i, j].Score += 1;
            after = Environment.TickCount;
            Console.WriteLine($"Two-dimensional rectangular array time (ms): {after - before}");

            before = Environment.TickCount;
            for (int i = 0; i < nRows; i++)
                for (int j = 0; j < nCols; j++)
                    jagged[i][j].Score += 1;
            after = Environment.TickCount;
            Console.WriteLine($"Two-dimensional jagged array time (ms): {after - before}");

            Console.WriteLine("Lab6 Serialization:");

            var lab6Student = new SerializableStudent(
                new Person("Mykola", "Lysenko", new DateTime(2001, 7, 15)),
                Education.Master,
                301);
            lab6Student.AddExams(
                new Exam("Algorithms", 92, new DateTime(2025, 6, 1)),
                new Exam("Databases", 87, new DateTime(2025, 6, 15)));

            Console.WriteLine("SerializableStudent original:");
            Console.WriteLine(lab6Student);
            var lab6Copy = lab6Student.DeepCopy();
            Console.WriteLine("Deep copy:");
            Console.WriteLine(lab6Copy);
            lab6Student.Exams[0].Score = 10;
            Console.WriteLine($"After changing original's first exam score to 10:");
            Console.WriteLine($"  Original: {lab6Student.Exams[0]}");
            Console.WriteLine($"  Copy: {lab6Copy.Exams[0]}");

            Console.Write("Enter filename for save/load: ");
            string lab6file = Console.ReadLine()?.Trim() ?? "student";
            if (string.IsNullOrWhiteSpace(lab6file)) lab6file = "student";

            if (!System.IO.File.Exists(lab6file))
            {
                lab6Student.Save(lab6file);
                Console.WriteLine("File created.");
            }
            else
            {
                bool loaded = lab6Student.Load(lab6file);
                Console.WriteLine(loaded ? "Loaded." : "Load failed.");
            }

            Console.WriteLine("Student T:");
            Console.WriteLine(lab6Student);

            Console.WriteLine("AddFromConsole → Save → print");
            lab6Student.AddFromConsole();
            lab6Student.Save(lab6file);
            Console.WriteLine("Student T after AddFromConsole + Save:");
            Console.WriteLine(lab6Student);

            Console.WriteLine("Static Load → AddFromConsole → Static Save");
            bool sLoaded = SerializableStudent.Load(lab6file, lab6Student);
            Console.WriteLine(sLoaded ? "Static Load: OK." : "Static Load failed.");
            lab6Student.AddFromConsole();
            SerializableStudent.Save(lab6file, lab6Student);

            Console.WriteLine("Final state of Student T:");
            Console.WriteLine(lab6Student);

            Console.WriteLine("Done. Press Enter.");
            Console.ReadLine();
            ConsoleEx.WriteHeader("--- Lab7: Couple demo ---");
            if (DateTime.Now.DayOfWeek == DayOfWeek.Sunday)
            {
                ConsoleEx.WriteWarning("Console demo is disabled on Sundays.");
            }
            else
            {
                var male = new HumanMale("Petro", "Shevchenko", new DateTime(1995, 1, 1));
                var studentGirl = new Girl("Oksana", "Bondar", new DateTime(1998, 5, 5));
                var pretty = new PrettyGirl("Irina", "Khmelyuk", new DateTime(1997, 7, 7));

                RunPair(male, studentGirl);
                ConsoleEx.WriteInfo("Press Enter for next pair, or press Q/F10 to quit.");
                var k = Console.ReadKey(true);
                if (k.Key != ConsoleKey.Q && k.Key != ConsoleKey.F10)
                {
                    RunPair(male, pretty);
                    ConsoleEx.WriteSuccess("Pairing demo finished.");
                }
            }

            await DemoLab8Async();
        }

        private static void RunPair(Human a, Human b)
        {
            try
            {
                ConsoleEx.WriteHeader($"Trying to couple {a.GetType().Name} ({a.Name}) and {b.GetType().Name} ({b.Name})");
                var child = Matchmaker.Couple(a, b);
                if (child == null)
                {
                    ConsoleEx.WriteWarning("No mutual sympathy or child creation failed.");
                    return;
                }
                ConsoleEx.WriteSuccess($"Child created: {child.Name} (Type: {child.GetType().Name})");
            }
            catch (SameGenderException ex)
            {
                ConsoleEx.WriteError($"Same gender exception: {ex.Message}");
            }
            catch (Exception ex)
            {
                ConsoleEx.WriteError($"Unexpected error during coupling: {ex.Message}");
            }
        }

        // ── Lab 8 ─────────────────────────────────────────────────────────────

        private static async Task DemoLab8Async()
        {
            Console.WriteLine();
            ConsoleEx.WriteHeader("--- Lab 8: Matrix Multiplication via TPL Dataflow (Variant 1) ---");

            Console.Write("  Matrix A — rows cols (e.g. 500 100): ");
            var (aRows, aCols) = ParseMatrixDimensions(Console.ReadLine());

            Console.Write("  Matrix B — rows cols (e.g. 100 500): ");
            var (bRows, bCols) = ParseMatrixDimensions(Console.ReadLine());

            if (aCols != bRows)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"  Incompatible dimensions: A({aRows}x{aCols}) x B({bRows}x{bCols})");
                Console.WriteLine($"  A.Cols ({aCols}) must equal B.Rows ({bRows}).");
                Console.ResetColor();
                return;
            }

            Console.Write($"  Generating A({aRows}x{aCols}) and B({bRows}x{bCols})... ");
            var swGen = Stopwatch.StartNew();
            var a = MatrixGenerator.Generate(aRows, aCols, seed: 42);
            var b = MatrixGenerator.Generate(bRows, bCols, seed: 84);
            swGen.Stop();
            Console.WriteLine($"done in {swGen.Elapsed.TotalMilliseconds:F1} ms.");

            Console.WriteLine("A (preview):");
            Console.WriteLine(a);
            Console.WriteLine("B (preview):");
            Console.WriteLine(b);

            int processorCount = Environment.ProcessorCount;
            Console.WriteLine($"  Result will be {aRows}x{bCols}.  Pipeline parallelism: {processorCount} CPU(s).");
            Console.WriteLine("  Press Ctrl+C to cancel.");

            using var cts = new CancellationTokenSource();
            ConsoleCancelEventHandler ctrlCHandler = (_, e) =>
            {
                e.Cancel = true;   // keep the process alive
                cts.Cancel();
                Console.WriteLine("\n  [Ctrl+C received — cancelling pipeline]");
            };
            Console.CancelKeyPress += ctrlCHandler;

            var sw = Stopwatch.StartNew();
            try
            {
                var result = await DataFlowMatrixMultiplier.MultiplyAsync(
                    a, b, cts.Token, processorCount);
                sw.Stop();

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"  Completed in {sw.Elapsed.TotalMilliseconds:F1} ms.");
                Console.ResetColor();

                Console.WriteLine("Result C = A x B (preview):");
                Console.WriteLine(result);

                // Quick self-check: multiply A by identity of matching size.
                // A x I_K should equal A (compare first element).
                Console.Write("  Self-check A x Identity... ");
                var identity = MatrixGenerator.Identity(aCols);
                var check    = await DataFlowMatrixMultiplier.MultiplyAsync(
                    a, identity, cts.Token, processorCount);
                double diff = Math.Abs(check[0, 0] - a[0, 0]);
                if (diff < 1e-9)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"PASS (delta = {diff:E2})");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"FAIL (delta = {diff:E2})");
                }
                Console.ResetColor();
            }
            catch (OperationCanceledException)
            {
                sw.Stop();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"  Cancelled after {sw.Elapsed.TotalMilliseconds:F1} ms.");
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                sw.Stop();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"  Error: {ex.Message}");
                Console.ResetColor();
            }
            finally
            {
                Console.CancelKeyPress -= ctrlCHandler;
            }
        }

        private static (int rows, int cols) ParseMatrixDimensions(string? input)
        {
            var parts = (input ?? string.Empty)
                .Split(new[] { ' ', '\t', ',' }, StringSplitOptions.RemoveEmptyEntries);
            int rows = parts.Length >= 1 && int.TryParse(parts[0], out var r) ? r : 10;
            int cols = parts.Length >= 2 && int.TryParse(parts[1], out var c) ? c : 10;
            return (Math.Max(rows, 1), Math.Max(cols, 1));
        }
    }
}
