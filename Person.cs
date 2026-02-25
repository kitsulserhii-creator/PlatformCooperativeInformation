using System;

namespace LabVariant1
{
    public class Person
    {
        private string _firstName;
        private string _lastName;
        private DateTime _birthDate;

        public Person(string firstName, string lastName, DateTime birthDate)
        {
            _firstName = firstName;
            _lastName = lastName;
            _birthDate = birthDate;
        }

        public Person()
        {
            _firstName = "";
            _lastName = "";
            _birthDate = DateTime.MinValue;
        }

        public string FirstName
        {
            get => _firstName;
            init => _firstName = value;
        }

        public string LastName
        {
            get => _lastName;
            init => _lastName = value;
        }

        public DateTime BirthDate
        {
            get => _birthDate;
            init => _birthDate = value;
        }

        public override string ToString()
        {
            return $"{_firstName} {_lastName}, DOB: {_birthDate:yyyy-MM-dd}";
        }

        public virtual string ToShortString()
        {
            return $"{_lastName} {_firstName}";
        }
    }
}
