using System;

namespace LabVariant1
{
    public class HumanMale : Human
    {
        public HumanMale() : base()
        {
        }

        public HumanMale(string first, string last, DateTime birth) : base(first, last, birth, Sex.Male)
        {
        }

        public string GetName() => FirstName;
    }
}
