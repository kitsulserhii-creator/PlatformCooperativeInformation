using System;
using System.Diagnostics;

namespace LabVariant1
{
    internal static class Program
    {
        static void Main(string[] args)
        {
            DemoPerson();
            DemoStudent();
            DemoStudentCollection();
            DemoTestCollections();
            DemoArrayTiming();

            Console.WriteLine("\nDone. Press Enter to exit.");
            Console.ReadLine();
        }

        // ── Demo sections ─────────────────────────────────────────────────────

        private static void DemoPerson()
        {
            PrintHeader("Person: equality, hashing, operators");

            var p1 = new Person("Ivan", "Petrenko", new DateTime(2000, 5, 10));
            var p2 = new Person("Ivan", "Petrenko", new DateTime(2000, 5, 10));

            Console.WriteLine($"ReferenceEquals(p1, p2) : {ReferenceEquals(p1, p2)}");
            Console.WriteLine($"p1 == p2                : {p1 == p2}");
            Console.WriteLine($"p1.Equals(p2)           : {p1.Equals(p2)}");
            Console.WriteLine($"p1.GetHashCode()        : {p1.GetHashCode()}");
            Console.WriteLine($"p2.GetHashCode()        : {p2.GetHashCode()}");
            Console.WriteLine($"Hashes equal            : {p1.GetHashCode() == p2.GetHashCode()}");
        }

        private static void DemoStudent()
        {
            PrintHeader("Student: deep copy independence + group validation");

            var student = new Student(
                new Person("Olena", "Ivanova", new DateTime(1999, 3, 2)),
                Education.Bachelor, 202);

            student.AddExams(
                new Exam("Mathematics",  95, DateTime.Now.AddDays(-30)),
                new Exam("Physics",      88, DateTime.Now.AddDays(-20)),
                new Exam("Programming", 100, DateTime.Now.AddDays(-10)));

            Console.WriteLine("Original:");
            Console.WriteLine(student);

            var copy = (Student)student.DeepCopy();

            // Mutate the original — the copy must stay unchanged.
            if (student.Exams.Count > 0)
                student.Exams[0].Score = 50;

            Console.WriteLine("\nAfter modifying original's first exam (score → 50):");
            Console.WriteLine($"  Original  avg: {student.AverageGrade:F2}");
            Console.WriteLine($"  Copy      avg: {copy.AverageGrade:F2}  (unchanged ✓)");

            // Group number validation
            try
            {
                _ = new Student(new Person("Bad", "Group", new DateTime(2001, 1, 1)),
                                Education.Bachelor, 5);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Console.WriteLine($"\nInvalid group number caught: {ex.ParamName}");
            }
        }

        private static void DemoStudentCollection()
        {
            PrintHeader("StudentCollection: sorting, filtering, LINQ");

            var coll = new StudentCollection();
            coll.AddDefaults();

            Console.WriteLine("Initial (short):");
            Console.WriteLine(coll.ToShortString());

            Console.WriteLine("\nSorted by surname:");
            coll.SortBySurname();
            Console.WriteLine(coll.ToShortString());

            Console.WriteLine("\nSorted by birth date:");
            coll.SortByBirthDate();
            Console.WriteLine(coll.ToShortString());

            Console.WriteLine("\nSorted by average grade:");
            coll.SortByAverage();
            Console.WriteLine(coll.ToShortString());

            Console.WriteLine($"\nMax average: {coll.MaxAverage:F2}");

            Console.WriteLine("\nMasters:");
            foreach (var s in coll.Masters)
                Console.WriteLine($"  {s.ToShortString()}");

            Console.WriteLine("\nGroups with average ≥ 85:");
            foreach (var s in coll.AverageMarkGroup(85.0))
                Console.WriteLine($"  {s.ToShortString()}");

            // foreach works because StudentCollection implements IEnumerable<Student>
            Console.WriteLine("\nAll students via foreach:");
            foreach (var s in coll)
                Console.WriteLine($"  {s.PersonData.ToShortString()}");
        }

        private static void DemoTestCollections()
        {
            PrintHeader("TestCollections: List vs Dictionary search performance");
            var tc = new TestCollections(1000);
            tc.MeasureSearches();
        }

        private static void DemoArrayTiming()
        {
            PrintHeader("Array access timing: 1-D vs rectangular vs jagged");

            Console.Write("Enter nRows and nCols (e.g. 1000 1000): ");
            var (nRows, nCols) = ParseDimensions(Console.ReadLine());
            int total = nRows * nCols;
            Console.WriteLine($"nRows={nRows}, nCols={nCols}, total={total}");

            // Populate all three array shapes with the same seed so results are comparable.
            var rnd = new Random(42);

            var oneD = new Exam[total];
            for (int i = 0; i < total; i++)
                oneD[i] = new Exam("S", rnd.Next(50, 101), DateTime.Now);

            var rect = new Exam[nRows, nCols];
            for (int i = 0; i < nRows; i++)
                for (int j = 0; j < nCols; j++)
                    rect[i, j] = new Exam("S", rnd.Next(50, 101), DateTime.Now);

            var jagged = new Exam[nRows][];
            for (int i = 0; i < nRows; i++)
            {
                jagged[i] = new Exam[nCols];
                for (int j = 0; j < nCols; j++)
                    jagged[i][j] = new Exam("S", rnd.Next(50, 101), DateTime.Now);
            }

            var sw = new Stopwatch();

            sw.Restart();
            for (int i = 0; i < total; i++) oneD[i].Score += 1;
            sw.Stop();
            Console.WriteLine($"  1-D array              : {sw.Elapsed.TotalMilliseconds:F4} ms");

            sw.Restart();
            for (int i = 0; i < nRows; i++)
                for (int j = 0; j < nCols; j++)
                    rect[i, j].Score += 1;
            sw.Stop();
            Console.WriteLine($"  2-D rectangular array  : {sw.Elapsed.TotalMilliseconds:F4} ms");

            sw.Restart();
            for (int i = 0; i < nRows; i++)
                for (int j = 0; j < nCols; j++)
                    jagged[i][j].Score += 1;
            sw.Stop();
            Console.WriteLine($"  2-D jagged array       : {sw.Elapsed.TotalMilliseconds:F4} ms");
        }

        // ── Helpers ───────────────────────────────────────────────────────────

        private static void PrintHeader(string title)
        {
            Console.WriteLine();
            Console.WriteLine($"=== {title} ===");
        }

        private static (int rows, int cols) ParseDimensions(string? input)
        {
            var parts = (input ?? string.Empty)
                .Split(new[] { ' ', '\t', ',' }, StringSplitOptions.RemoveEmptyEntries);

            int rows = parts.Length >= 1 && int.TryParse(parts[0], out var r) ? r : 10;
            int cols = parts.Length >= 2 && int.TryParse(parts[1], out var c) ? c : 10;
            return (Math.Max(rows, 1), Math.Max(cols, 1));
        }
    }
}
