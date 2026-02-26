using System;
using System.Collections.Generic;
using System.Linq;

namespace LabVariant1
{
    public class StudentCollection
    {
        private List<Student> _students = new List<Student>();

        public StudentCollection()
        {
        }

        public void AddDefaults()
        {
            var s1 = new Student(new Person("Ivan", "Petrenko", new DateTime(2000, 5, 10)), Education.Master, 101);
            s1.AddExams(new Exam("Math", 90, DateTime.Now.AddMonths(-6)), new Exam("Prog", 85, DateTime.Now.AddMonths(-3)));
            s1.AddTests(new Test("Physics", true, DateTime.Now.AddMonths(-5)));

            var s2 = new Student(new Person("Olena", "Ivanova", new DateTime(1999, 3, 2)), Education.Bachelor, 202);
            s2.AddExams(new Exam("Math", 75, DateTime.Now.AddMonths(-6)), new Exam("Prog", 80, DateTime.Now.AddMonths(-3)));
            s2.AddTests(new Test("Physics", true, DateTime.Now.AddMonths(-5)));

            var s3 = new Student(new Person("Petro", "Shevchenko", new DateTime(1998, 7, 21)), Education.Master, 101);
            s3.AddExams(new Exam("Math", 95, DateTime.Now.AddMonths(-6)), new Exam("Prog", 92, DateTime.Now.AddMonths(-3)));
            s3.AddTests(new Test("Physics", true, DateTime.Now.AddMonths(-5)));

            AddStudents(s1, s2, s3);
        }

        public void AddStudents(params Student[] students)
        {
            if (students == null || students.Length == 0) return;
            _students.AddRange(students);
        }

        public override string ToString()
        {
            return string.Join("\n----\n", _students.Select(s => s.ToString()));
        }

        public virtual string ToShortString()
        {
            return string.Join("\n", _students.Select(s => s.ToShortString()));
        }

        public void SortBySurname()
        {
            _students.Sort((a, b) => a.PersonData.CompareTo(b.PersonData));
        }

        public void SortByBirthDate()
        {
            var comparer = new Person();
            _students.Sort((a, b) => comparer.Compare(a.PersonData, b.PersonData));
        }

        public void SortByAverage()
        {
            _students.Sort(new StudentAverageComparer());
        }

        public double MaxAverage
        {
            get
            {
                if (_students == null || _students.Count == 0) return 0.0;
                return _students.Max(s => s.AverageGrade);
            }
        }

        public IEnumerable<Student> Masters
        {
            get => _students.Where(s => s.EducationForm == Education.Master);
        }

        public List<Student> AverageMarkGroup(double value)
        {
            var groups = _students.GroupBy(s => s.GroupNumber)
                                  .Where(g => g.Average(st => st.AverageGrade) >= value)
                                  .SelectMany(g => g)
                                  .ToList();
            return groups;
        }

        public List<Student> ToList() => new List<Student>(_students);
    }
}
