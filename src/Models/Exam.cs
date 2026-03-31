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
            if (score < 0 || score > 100)
                throw new ArgumentOutOfRangeException(nameof(score), score, "Score must be between 0 and 100.");
            Subject = subject ?? throw new ArgumentNullException(nameof(subject));
            Score   = score;
            Date    = date;
        }

        public Exam() : this(string.Empty, 0, DateTime.MinValue) { }

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
