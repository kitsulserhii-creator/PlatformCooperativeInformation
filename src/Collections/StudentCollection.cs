using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace LabVariant1
{
    /// <summary>
    /// An ordered collection of <see cref="Student"/> objects with sorting,
    /// filtering, and projection helpers.
    /// Implements <see cref="IEnumerable{T}"/> so the collection can be used
    /// directly in foreach loops and LINQ expressions.
    /// </summary>
    public class StudentCollection : IEnumerable<Student>
    {
        private readonly List<Student> _students = new();

        public StudentCollection() { }

        // ── Mutation ──────────────────────────────────────────────────────────

        public void AddStudents(params Student[] students)
        {
            if (students is null || students.Length == 0) return;
            _students.AddRange(students);
        }

        /// <summary>Adds a small set of hand-crafted students for demo purposes.</summary>
        public void AddDefaults()
        {
            var s1 = new Student(new Person("Ivan",  "Petrenko",   new DateTime(2000, 5, 10)), Education.Master,   101);
            s1.AddExams(new Exam("Math", 90, DateTime.Now.AddMonths(-6)),
                        new Exam("Prog", 85, DateTime.Now.AddMonths(-3)));
            s1.AddTests(new Test("Physics", true, DateTime.Now.AddMonths(-5)));

            var s2 = new Student(new Person("Olena", "Ivanova",    new DateTime(1999, 3,  2)), Education.Bachelor, 202);
            s2.AddExams(new Exam("Math", 75, DateTime.Now.AddMonths(-6)),
                        new Exam("Prog", 80, DateTime.Now.AddMonths(-3)));
            s2.AddTests(new Test("Physics", true, DateTime.Now.AddMonths(-5)));

            var s3 = new Student(new Person("Petro", "Shevchenko", new DateTime(1998, 7, 21)), Education.Master,   101);
            s3.AddExams(new Exam("Math", 95, DateTime.Now.AddMonths(-6)),
                        new Exam("Prog", 92, DateTime.Now.AddMonths(-3)));
            s3.AddTests(new Test("Physics", true, DateTime.Now.AddMonths(-5)));

            AddStudents(s1, s2, s3);
        }

        // ── Sorting ───────────────────────────────────────────────────────────

        public void SortBySurname()   => _students.Sort((a, b) => a.PersonData.CompareTo(b.PersonData));

        public void SortByBirthDate() => _students.Sort(
            (a, b) => PersonBirthDateComparer.Instance.Compare(a.PersonData, b.PersonData));

        public void SortByAverage()   => _students.Sort(StudentAverageComparer.Instance);

        // ── Queries ───────────────────────────────────────────────────────────

        /// <summary>Highest average grade in the collection, or 0 if empty.</summary>
        public double MaxAverage =>
            _students.Count == 0 ? 0.0 : _students.Max(s => s.AverageGrade);

        /// <summary>All students enrolled in the Master programme.</summary>
        public IEnumerable<Student> Masters =>
            _students.Where(s => s.EducationForm == Education.Master);

        /// <summary>
        /// Returns all students that belong to groups whose average grade is
        /// at or above <paramref name="minAverage"/>.
        /// </summary>
        public IReadOnlyList<Student> AverageMarkGroup(double minAverage) =>
            _students
                .GroupBy(s => s.GroupNumber)
                .Where(g => g.Average(st => st.AverageGrade) >= minAverage)
                .SelectMany(g => g)
                .ToList();

        // ── Projection ────────────────────────────────────────────────────────

        public override string ToString() =>
            string.Join("\n----\n", _students.Select(s => s.ToString()));

        public virtual string ToShortString() =>
            string.Join("\n", _students.Select(s => s.ToShortString()));

        // ── IEnumerable<Student> ─────────────────────────────────────────────

        public IEnumerator<Student> GetEnumerator()    => _students.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator()        => GetEnumerator();
    }
}
