using System;

namespace LabVariant1
{
    public class Exam : IDateAndCopy
    {
        public string Subject { get; set; }
        public int Score { get; set; }
        public DateTime Date { get; init; }

        public Exam(string subject, int score, DateTime date)
        {
            Subject = subject;
            Score = score;
            Date = date;
        }

        public Exam()
        {
            Subject = string.Empty;
            Score = 0;
            Date = DateTime.MinValue;
        }

        public object DeepCopy()
        {
            return new Exam(Subject, Score, Date);
        }

        public override string ToString()
        {
            return $"{Subject}: {Score} ({Date:yyyy-MM-dd})";
        }
    }
}
