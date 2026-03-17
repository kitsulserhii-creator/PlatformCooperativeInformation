using System;

namespace LabVariant1
{
    [Couple(Pair = "Male", Probability = 1.0, ChildType = "PrettyGirl")]
    public sealed class PrettyGirl : Girl
    {
        public PrettyGirl() : base()
        {
        }

        public PrettyGirl(string first, string last, DateTime birth) : base(first, last, birth)
        {
        }

        public new string GetName() => FirstName;
    }
}
