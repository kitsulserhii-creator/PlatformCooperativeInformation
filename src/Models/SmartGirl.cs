using System;

namespace LabVariant1
{
    [Couple(Pair = "Male", Probability = 0.5, ChildType = "Girl")]
    public sealed class SmartGirl : Girl
    {
        public SmartGirl() : base()
        {
        }

        public SmartGirl(string first, string last, DateTime birth) : base(first, last, birth)
        {
        }

        public new string GetName() => FirstName;
    }
}
