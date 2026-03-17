using System;
using System.Linq;
using System.Text.Json.Serialization;

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
        private System.Collections.Generic.List<Test> _tests;
        private System.Collections.Generic.List<Exam> _exams;

        public Student(Person person, Education education, int groupNumber)
        {
            _person = person ?? new Person();
            _education = education;
            GroupNumber = groupNumber;
            _tests = new System.Collections.Generic.List<Test>();
            _exams = new System.Collections.Generic.List<Exam>();
        }

        public Student()
        {
            _person = new Person();
            _education = Education.Bachelor;
            _groupNumber = 0;
            _tests = new System.Collections.Generic.List<Test>();
            _exams = new System.Collections.Generic.List<Exam>();
        }

        public Person PersonData
        {
            get => _person;
            set => _person = value ?? new Person();
        }

        public Education EducationForm
        {
            get => _education;
            set => _education = value;
        }

        public int GroupNumber
        {
            get => _groupNumber;
            set
            {
                if (value < 100 || value > 699) throw new ArgumentOutOfRangeException(nameof(GroupNumber), "Group number must be between 100 and 699.");
                _groupNumber = value;
            }
        }

        public System.Collections.Generic.List<Test> Tests
        {
            get => _tests;
            set => _tests = value ?? new System.Collections.Generic.List<Test>();
        }

        public System.Collections.Generic.List<Exam> Exams
        {
            get => _exams;
            set => _exams = value ?? new System.Collections.Generic.List<Exam>();
        }

        [JsonIgnore]
        public double AverageGrade
        {
            get
            {
                if (_exams == null || _exams.Count == 0) return 0.0;
                return _exams.Average(e => e.Score);
            }
        }

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

        [JsonIgnore]
        public DateTime Date
        {
            get => PersonData.BirthDate;
            init => PersonData = new Person(PersonData.FirstName, PersonData.LastName, value);
        }

        public virtual object DeepCopy()
        {
            var personCopy = (Person)_person.DeepCopy();
            var examsCopy = _exams?.Select(e => (Exam)e.DeepCopy()).ToList() ?? new System.Collections.Generic.List<Exam>();
            var testsCopy = _tests?.Select(t => new Test(t.Subject, t.Passed, t.Date)).ToList() ?? new System.Collections.Generic.List<Test>();
            var copy = new Student(personCopy, _education, _groupNumber)
            {
                Exams = examsCopy,
                Tests = testsCopy
            };
            return copy;
        }
    }
}
