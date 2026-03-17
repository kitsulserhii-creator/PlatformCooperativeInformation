using System;

namespace LabVariant1
{
    public enum Sex { Male, Female }

    public abstract class Human : Person, IHasName
    {
        public Sex Sex { get; init; }

        protected Human()
        {
            Sex = Sex.Male;
        }

        protected Human(string firstName, string lastName, DateTime birth, Sex sex) : base(firstName, lastName, birth)
        {
            Sex = sex;
        }

        public virtual string Name => FirstName + " " + LastName;

        public override string ToShortString()
        {
            return Name;
        }
    }
}
