using System;
using System.Collections.Generic;
using System.Linq;

namespace LabVariant1
{
    public enum Education
    {
        Master = 0,
        Bachelor = 1,
        SecondEducation = 2
    }

    public class Student : IDateAndCopy
    {
        private Person _person;
        private Education _education;
        private int _groupNumber;
        private readonly List<Test> _tests;
        private readonly List<Exam> _exams;

        public Student(Person person, Education education, int groupNumber)
        {
            _person    = person ?? new Person();
            _education = education;
            GroupNumber = groupNumber;
            _tests = new List<Test>();
            _exams = new List<Exam>();
        }

        public Student() : this(new Person(), Education.Bachelor, 100) { }

        public Person PersonData
        {
            get => _person;
            set => _person = value ?? throw new ArgumentNullException(nameof(value));
        }

        public Education EducationForm
        {
            get => _education;
            set => _education = value;
        }

        public int GroupNumber
        {
            get => _groupNumber;
            init
            {
                if (value < 100 || value > 699) throw new ArgumentOutOfRangeException(nameof(GroupNumber), "Group number must be between 100 and 699.");
                _groupNumber = value;
            }
        }

        /// <summary>Read-only view of tests. Use <see cref="AddTests"/> to add.</summary>
        public IReadOnlyList<Test> Tests => _tests;

        /// <summary>Read-only view of exams. Use <see cref="AddExams"/> to add.</summary>
        public IReadOnlyList<Exam> Exams => _exams;

        public double AverageGrade =>
            _exams.Count == 0 ? 0.0 : _exams.Average(e => e.Score);

        public bool this[Education education]
        {
            get => _education == education;
        }

        public void AddExams(params Exam[] exams)
        {
            if (exams == null || exams.Length == 0) return;
            _exams.AddRange(exams);
        }

        public void AddTests(params Test[] tests)
        {
            if (tests == null || tests.Length == 0) return;
            _tests.AddRange(tests);
        }

        public override string ToString()
        {
            var examsText = _exams.Count == 0 ? "No exams" : string.Join("; ", _exams.Select(e => e.ToString()));
            var testsText = _tests.Count == 0 ? "No tests" : string.Join("; ", _tests.Select(t => t.ToString()));
            return $"{_person}\nEducation: {_education}, Group: {_groupNumber}\nExams: {examsText}\nTests: {testsText}";
        }

        public virtual string ToShortString()
        {
            return $"{_person}\nEducation: {_education}, Group: {_groupNumber}, Avg: {AverageGrade:F2}";
        }

        public DateTime Date
        {
            get => PersonData.BirthDate;
            init => PersonData = new Person(PersonData.FirstName, PersonData.LastName, value);
        }

        public virtual object DeepCopy()
        {
            var personCopy = (Person)_person.DeepCopy();
            var copy = new Student(personCopy, _education, _groupNumber);
            copy.AddExams(_exams.Select(e => (Exam)e.DeepCopy()).ToArray());
            copy.AddTests(_tests.Select(t => (Test)t.DeepCopy()).ToArray());
            return copy;
        }
    }
}
