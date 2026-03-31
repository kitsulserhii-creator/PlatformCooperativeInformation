using System.Collections.Generic;

namespace LabVariant1
{
    /// <summary>
    /// Compares two <see cref="Student"/> instances by their average exam grade.
    /// Use the singleton <see cref="Instance"/> to avoid repeated allocations.
    /// </summary>
    public sealed class StudentAverageComparer : IComparer<Student>
    {
        public static readonly StudentAverageComparer Instance = new();

        private StudentAverageComparer() { }

        public int Compare(Student? x, Student? y)
        {
            if (ReferenceEquals(x, y)) return 0;
            if (x is null) return -1;
            if (y is null) return 1;
            return x.AverageGrade.CompareTo(y.AverageGrade);
        }
    }
}
