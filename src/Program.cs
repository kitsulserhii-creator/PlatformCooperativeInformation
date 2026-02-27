using System;

namespace LabVariant1
{
    internal static class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Lab Variant 1 - Student/Exam demo (equality, hashing, deep copy)");

            
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
            Console.WriteLine("Original student:");
            Console.WriteLine(student.ToString());

            
            var copy = (Student)student.DeepCopy();
            Console.WriteLine("Deep-copied student:");
            Console.WriteLine(copy.ToString());

            
            if (student.Exams.Count > 0)
            {
                student.Exams[0].Score = 50;
            }
            Console.WriteLine("After modifying original student's first exam score:");
            Console.WriteLine("Original:");
            Console.WriteLine(student.ToString());
            Console.WriteLine("Copy (should be unchanged):");
            Console.WriteLine(copy.ToString());

            
            try
            {
                var bad = new Student(new Person("Bad", "Group", new DateTime(2001,1,1)), Education.Bachelor, 5);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Console.WriteLine($"Caught exception for invalid group: {ex.Message}");
            }

            
            Console.WriteLine("\n--- StudentCollection demo ---");
            var coll = new StudentCollection();
            coll.AddDefaults();
            Console.WriteLine("Students (short):");
            Console.WriteLine(coll.ToShortString());

            Console.WriteLine("\nSort by surname:");
            coll.SortBySurname();
            Console.WriteLine(coll.ToShortString());

            Console.WriteLine("\nSort by birth date:");
            coll.SortByBirthDate();
            Console.WriteLine(coll.ToShortString());

            Console.WriteLine("\nSort by average grade:");
            coll.SortByAverage();
            Console.WriteLine(coll.ToShortString());

            Console.WriteLine($"Max average: {coll.MaxAverage:F2}");
            Console.WriteLine("Masters in collection:");
            foreach (var s in coll.Masters) Console.WriteLine(s.ToShortString());

            Console.WriteLine("Students in groups where group-average >= 85:");
            var high = coll.AverageMarkGroup(85.0);
            foreach (var s in high) Console.WriteLine(s.ToShortString());

            
            Console.WriteLine("\n--- TestCollections demo (search measurements) ---");
            var tests = new TestCollections(1000);
            tests.MeasureSearches();

            var lab4 = new TestCollectionsLab4(1000);
            lab4.RunAll(5);

            
            Console.WriteLine();
            Console.WriteLine("Timing array element operations. Enter nRows and nColumns separated by space (e.g. 100 100):");
            var input = Console.ReadLine() ?? "10 10";
            var parts = input.Split(new[] { ' ', '\t', ',' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length < 2) parts = new[] { "10", "10" };
            if (!int.TryParse(parts[0], out var nRows)) nRows = 10;
            if (!int.TryParse(parts[1], out var nCols)) nCols = 10;

            int total = nRows * nCols;
            Console.WriteLine($"nRows={nRows}, nCols={nCols}, total elements={total}");

            
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

            Console.WriteLine("Done. Press Enter to exit.");
            Console.ReadLine();
        }
    }
}
