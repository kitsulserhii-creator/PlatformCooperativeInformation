using System;
using System.Collections.Generic;

namespace LabVariant1
{
    /// <summary>
    /// Compares two <see cref="Person"/> instances by their <see cref="Person.BirthDate"/>.
    /// Use the singleton <see cref="Instance"/> to avoid repeated allocations.
    /// </summary>
    public sealed class PersonBirthDateComparer : IComparer<Person>
    {
        public static readonly PersonBirthDateComparer Instance = new();

        private PersonBirthDateComparer() { }

        public int Compare(Person? x, Person? y)
        {
            if (ReferenceEquals(x, y)) return 0;
            if (x is null) return -1;
            if (y is null) return 1;
            return DateTime.Compare(x.BirthDate, y.BirthDate);
        }
    }
}
