using System;

namespace ORM_Lib.TypeMapper
{
    public interface ITypeMapper
    {
        string GetDbType(Type type);
        string GetForeignKeyType();

    }
}