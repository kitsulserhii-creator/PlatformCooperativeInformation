using System.Collections.Generic;

namespace LabVariant1
{
    public class StudentAverageComparer : IComparer<Student>
    {
        public int Compare(Student? x, Student? y)
        {
            if (ReferenceEquals(x, y)) return 0;
            if (x is null) return -1;
            if (y is null) return 1;
            return x.AverageGrade.CompareTo(y.AverageGrade);
        }
    }
}
