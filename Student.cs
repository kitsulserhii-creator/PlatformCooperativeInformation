using System;
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
        private Exam[] _exams;

        public Student(Person person, Education education, int groupNumber)
        {
            _person = person ?? new Person();
            _education = education;
            GroupNumber = groupNumber;
            _exams = Array.Empty<Exam>();
        }

        public Student()
        {
            _person = new Person();
            _education = Education.Bachelor;
            _groupNumber = 0;
            _exams = Array.Empty<Exam>();
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
            init
            {
                if (value < 100 || value > 699) throw new ArgumentOutOfRangeException(nameof(GroupNumber), "Group number must be between 100 and 699.");
                _groupNumber = value;
            }
        }

        public Exam[] Exams
        {
            get => _exams;
            set => _exams = value ?? Array.Empty<Exam>();
        }

        public double AverageGrade
        {
            get
            {
                if (_exams == null || _exams.Length == 0) return 0.0;
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
            var list = new Exam[_exams.Length + exams.Length];
            Array.Copy(_exams, list, _exams.Length);
            Array.Copy(exams, 0, list, _exams.Length, exams.Length);
            _exams = list;
        }

        public override string ToString()
        {
            var examsText = _exams.Length == 0 ? "No exams" : string.Join("; ", _exams.Select(e => e.ToString()));
            return $"{_person}\nEducation: {_education}, Group: {_groupNumber}\nExams: {examsText}";
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
            var examsCopy = _exams?.Select(e => (Exam)e.DeepCopy()).ToArray() ?? Array.Empty<Exam>();
            var copy = new Student(personCopy, _education, _groupNumber)
            {
                Exams = examsCopy
            };
            return copy;
        }
    }
}
