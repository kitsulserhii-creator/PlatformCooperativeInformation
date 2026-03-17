using System;

namespace LabVariant1
{
    public class SameGenderException : Exception
    {
        public SameGenderException(string message) : base(message) { }
    }
}
