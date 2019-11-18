using System.Collections.Generic;

namespace ORM_Lib.Constraints_Attributes
{
    public class ManyToManyComparer: IEqualityComparer<ManyToMany>
    {
        public bool Equals(ManyToMany x, ManyToMany y)
        {
            return x.ForeignKeyFar.Equals(y.ForeignKeyNear) &&
                x.ForeignKeyNear.Equals(y.ForeignKeyFar);
        }

        public int GetHashCode(ManyToMany obj)
        {
            return 0;
        }
    }
}