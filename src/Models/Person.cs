using System;

namespace LabVariant1
{
    public class Person : IDateAndCopy, IComparable<Person>, IEquatable<Person>
    {
        private readonly string _firstName;
        private readonly string _lastName;
        private readonly DateTime _birthDate;

        public Person(string firstName, string lastName, DateTime birthDate)
        {
            _firstName = firstName ?? throw new ArgumentNullException(nameof(firstName));
            _lastName  = lastName  ?? throw new ArgumentNullException(nameof(lastName));
            _birthDate = birthDate;
        }

        public Person() : this(string.Empty, string.Empty, DateTime.MinValue) { }

        // Get-only: Person is immutable after construction.
        public string   FirstName => _firstName;
        public string   LastName  => _lastName;
        public DateTime BirthDate => _birthDate;

        // IDateAndCopy.Date maps to BirthDate; explicit to avoid a second
        // init-setter path that could silently overwrite BirthDate.
        DateTime IDateAndCopy.Date
        {
            get  => _birthDate;
            init => _birthDate = value;   // readonly field may be set in init
        }

        public override string ToString() =>
            $"{_firstName} {_lastName}, DOB: {_birthDate:yyyy-MM-dd}";

        public virtual string ToShortString() => $"{_lastName} {_firstName}";

        public virtual object DeepCopy() => new Person(_firstName, _lastName, _birthDate);

        // IComparable<Person>: primary sort by last name, secondary by first name
        // for a stable, deterministic ordering.
        public int CompareTo(Person? other)
        {
            if (other is null) return 1;
            int cmp = string.Compare(_lastName, other._lastName, StringComparison.OrdinalIgnoreCase);
            return cmp != 0
                ? cmp
                : string.Compare(_firstName, other._firstName, StringComparison.OrdinalIgnoreCase);
        }

        // IEquatable<Person>: case-sensitive, ordinal to match GetHashCode.
        public bool Equals(Person? other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(_firstName, other._firstName, StringComparison.Ordinal) &&
                   string.Equals(_lastName,  other._lastName,  StringComparison.Ordinal) &&
                   _birthDate == other._birthDate;
        }

        public override bool Equals(object? obj) => Equals(obj as Person);

        // StringComparer.Ordinal is explicit for consistency with Equals.
        public override int GetHashCode() => HashCode.Combine(
            StringComparer.Ordinal.GetHashCode(_firstName),
            StringComparer.Ordinal.GetHashCode(_lastName),
            _birthDate);

        // Null-conditional pattern: short, correct for all null combinations.
        public static bool operator ==(Person? a, Person? b) => a?.Equals(b) ?? b is null;
        public static bool operator !=(Person? a, Person? b) => !(a == b);
    }
}
