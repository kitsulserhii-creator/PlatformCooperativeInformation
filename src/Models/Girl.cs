using System;

namespace LabVariant1
{
    [Couple(Pair = "Male", Probability = 0.7, ChildType = "Girl")]
    public class Girl : Human
    {
        public Girl() : base()
        {
        }

        public Girl(string first, string last, DateTime birth) : base(first, last, birth, Sex.Female)
        {
        }

        public string GetName() => FirstName;
    }
}
