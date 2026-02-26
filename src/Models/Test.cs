using System;

namespace LabVariant1
{
    public class Test
    {
        public string Subject { get; set; }
        public bool Passed { get; set; }
        public DateTime Date { get; set; }

        public Test(string subject, bool passed, DateTime date)
        {
            Subject = subject;
            Passed = passed;
            Date = date;
        }

        public Test()
        {
            Subject = string.Empty;
            Passed = false;
            Date = DateTime.MinValue;
        }

        public override string ToString()
        {
            return $"{Subject}: {(Passed ? "Passed" : "Failed")} ({Date:yyyy-MM-dd})";
        }
    }
}
