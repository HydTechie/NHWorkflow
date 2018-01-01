using System;
using System.Collections.Generic;
using System.Text;

namespace NHWorkflow.Core
{
    public partial class BaseEntity
    {
        public Guid Id;

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        // Does it just empty Id?
        private static bool IsTransient(BaseEntity obj)
        {
            return obj != null && Equals(obj.Id, default(Guid));
        }

        private Type GetUnproxiedType()
        {
            return GetType();
        }

        public virtual bool Equals(BaseEntity other)
        {
            if (other == null)
                return false;

            // references are same?
            if (ReferenceEquals(this, other))
                return true;

            // not empty ones
            if (!IsTransient(this) &&
                !IsTransient(other) &&
                Equals(Id, other.Id)
                )
            {
                var otherType = other.GetUnproxiedType();
                var thisType = this.GetUnproxiedType();

                return thisType.IsAssignableFrom(otherType) ||
                    otherType.IsAssignableFrom(thisType);
            }

            return false;

        }

        public override int GetHashCode()
        {            
             if(Equals(Id, default(Guid)))
                return base.GetHashCode();
             
            return Id.GetHashCode();
        }

        public override string ToString()
        {
            return base.ToString();
        }

    }
}
