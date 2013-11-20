using System;

namespace Bloghost.Core.Entities
{
    public abstract class BaseEntity : IEquatable<BaseEntity>
    {
        private int transientHashCode;

        public virtual Guid ID { get; set; }

        public virtual bool Equals(BaseEntity other)
        {
            if (other == null) return false;

            if (IsTransient)
            {
                return ReferenceEquals(this, other);
            }

            return other.ID == ID && other.GetType() == GetType();
        }

        private bool IsTransient
        {

            get { return ID == Guid.Empty; }

        }

        public override bool Equals(object obj)
        {
            return Equals(obj as BaseEntity);
        }

        public override int GetHashCode()
        {
            if (IsTransient)
            {
                if (transientHashCode == 0)
                {
                    transientHashCode = base.GetHashCode();
                }
                return transientHashCode;
            }
            return base.GetHashCode();
        }
    }
}
