using System;

namespace LabVariant1
{
    public class Person : IDateAndCopy
    {
        protected string _firstName;
        protected string _lastName;
        protected DateTime _birthDate;

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

        public DateTime Date
        {
            get => BirthDate;
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

        public virtual object DeepCopy()
        {
            return new Person(FirstName, LastName, BirthDate);
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(this, obj)) return true;
            if (obj is not Person other) return false;
            return string.Equals(FirstName, other.FirstName, StringComparison.Ordinal) &&
                   string.Equals(LastName, other.LastName, StringComparison.Ordinal) &&
                   BirthDate.Equals(other.BirthDate);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(FirstName, LastName, BirthDate);
        }

        public static bool operator ==(Person? a, Person? b)
        {
            if (ReferenceEquals(a, b)) return true;
            if (a is null || b is null) return false;
            return a.Equals(b);
        }

        public static bool operator !=(Person? a, Person? b)
        {
            return !(a == b);
        }
    }
}
