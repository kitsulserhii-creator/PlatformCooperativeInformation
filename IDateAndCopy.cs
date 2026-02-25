using System;

namespace LabVariant1
{
    public interface IDateAndCopy
    {
        DateTime Date { get; init; }
        object DeepCopy();
    }
}
