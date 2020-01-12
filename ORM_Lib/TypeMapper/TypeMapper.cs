using System;

namespace ORM_Lib.TypeMapper
{
    public interface ITypeMapper
    {
        string GetDbType(Type type);
        
        // fk and pk should match in the type they convert to later (int/long)...
        string GetForeignKeyType();

        string GetPrimaryKeyType();

        // at the moment we always want text
        string GetEnumType();

    }
}