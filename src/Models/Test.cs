using System;

namespace LabVariant1
{
    public class Test : IDateAndCopy
    {
        public string Subject { get; set; }
        public bool Passed { get; set; }
        public DateTime Date { get; init; }

        public Test(string subject, bool passed, DateTime date)
        {
            Subject = subject;
            Passed  = passed;
            Date    = date;
        }

        public Test() : this(string.Empty, false, DateTime.MinValue) { }

        public object DeepCopy() => new Test(Subject, Passed, Date);

        public override string ToString() =>
            $"{Subject}: {(Passed ? "Passed" : "Failed")} ({Date:yyyy-MM-dd})";
    }
}
