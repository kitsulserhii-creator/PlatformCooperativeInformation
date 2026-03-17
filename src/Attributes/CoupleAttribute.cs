using System;

namespace LabVariant1
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public sealed class CoupleAttribute : Attribute
    {
        public string Pair { get; init; } = string.Empty;
        public double Probability { get; init; } = 0.0;
        public string ChildType { get; init; } = string.Empty;
    }
}
