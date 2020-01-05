using ORM_Lib.TypeMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace ORM_Lib
{
    public class ORMConfiguration
    {

        // abstract TypeMapper vom User verlangen (ITypeMapper to be exact) und wir haben halt schon eine konkrete implementation bereitliegen für postgres
        public ITypeMapper TypeMapper { get; }


        // https://softwareengineering.stackexchange.com/questions/142065/creating-database-connections-do-it-once-or-for-each-query open and close everytime - automatic connection pooling in .NET
        public string ConnectionString { get; }

        // decides if the DDL for the creation of the database should be executed
        public Boolean CreateDB { get; }

        public ORMConfiguration(ITypeMapper typeMapper, string connectionString, Boolean createDB)
        {
            TypeMapper = typeMapper;
            ConnectionString = connectionString;
            CreateDB = createDB;
        }

    }
}
