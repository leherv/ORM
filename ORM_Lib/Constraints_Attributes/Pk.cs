using System;

namespace ORM_Lib.Constraints_Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class Pk: Attribute, IConstraint
    {
        
    }
}