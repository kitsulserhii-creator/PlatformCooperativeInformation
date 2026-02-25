using System;

namespace LabVariant1
{
    internal static class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Lab Variant 1 - Student/Exam timing demo");

            // Create a sample student and print ToShortString
            var p = new Person("Ivan", "Petrenko", new DateTime(2000, 5, 10));
            var student = new Student(p, Education.Master, 101);
            Console.WriteLine("Student ToShortString():");
            Console.WriteLine(student.ToShortString());

            // Show enum values
            Console.WriteLine($"Education indices: Master={(int)Education.Master}, Bachelor={(int)Education.Bachelor}, SecondEducation={(int)Education.SecondEducation}");

            // Assign properties and print ToString
            student.PersonData = new Person("Olena", "Ivanova", new DateTime(1999, 3, 2));
            student.EducationForm = Education.Bachelor;
            student.GroupNumber = 202;
            Console.WriteLine("After assigning properties, ToString():");
            Console.WriteLine(student.ToString());

            // Add exams
            student.AddExams(
                new Exam("Mathematics", 95, DateTime.Now.AddDays(-30)),
                new Exam("Physics", 88, DateTime.Now.AddDays(-20)),
                new Exam("Programming", 100, DateTime.Now.AddDays(-10))
            );
            Console.WriteLine("After AddExams():");
            Console.WriteLine(student.ToString());

            // Timing experiments
            Console.WriteLine();
            Console.WriteLine("Timing array element operations. Enter nRows and nColumns separated by space (e.g. 100 100):");
            var input = Console.ReadLine() ?? "10 10";
            var parts = input.Split(new[] { ' ', '\t', ',' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length < 2) parts = new[] { "10", "10" };
            if (!int.TryParse(parts[0], out var nRows)) nRows = 10;
            if (!int.TryParse(parts[1], out var nCols)) nCols = 10;

            int total = nRows * nCols;
            Console.WriteLine($"nRows={nRows}, nCols={nCols}, total elements={total}");

            // Prepare arrays of Exam objects
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

            // Operation to measure: increase Score by 1 for each exam

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
