using System;

namespace ORM_Lib.TypeMapper
{
    public interface ITypeMapper
    {

        string GetDbType(string type);
        string GetDbType(Type type);
        string GetForeignKeyType();

    }
}