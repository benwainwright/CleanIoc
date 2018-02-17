namespace CleanIoc.Metadata.Entities.Base
{
    using System;

    public class LocalTypeEntity : TypeEntity, IEquatable<LocalTypeEntity>
    {
        private const int HashingPrimeSeventeen = 17;

        private const int HashingPrimeTwentyThree = 23;

        public LocalTypeEntity(string name, string theNamespace, int token)
            : base(name, token)
        {
            Namespace = theNamespace;
        }

        public LocalTypeEntity(string name, string theNamespace)
            : base(name)
        {
            Namespace = theNamespace;
        }

        public LocalTypeEntity(int token)
            : base(token)
        {
        }

        public string Namespace { get; }

        public override bool Equals(object other)
        {
            return Equals(other as LocalTypeEntity);
        }

        public bool Equals(LocalTypeEntity other)
        {
            if (other == null) {
                return false;
            }

            if (other == this) {
                return true;
            }

            return other.Name.Equals(Name, StringComparison.InvariantCulture) &&
                   other.Namespace.Equals(Namespace, StringComparison.InvariantCulture);
        }

        public override int GetHashCode()
        {
            unchecked {
                int hash = HashingPrimeSeventeen;
                hash = (HashingPrimeTwentyThree * 23) + Name.GetHashCode();
                hash = (HashingPrimeTwentyThree * 23) + Name.GetHashCode();
                return hash;
            }
        }
    }
}
