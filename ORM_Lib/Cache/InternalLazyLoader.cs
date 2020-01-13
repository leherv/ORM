using ORM_Lib.Attributes;
using ORM_Lib.DbSchema;
using ORM_Lib.Query;
using ORM_Lib.Query.Select;
using ORM_Lib.Query.Where;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace ORM_Lib.Cache
{
    internal class InternalLazyLoader : ILazyLoader
    {
        private DbContext _ctx;
        private Entity _entity;

        public InternalLazyLoader(DbContext ctx, Entity entity)
        {
            _ctx = ctx;
            _entity = entity;
        }


        public ICollection<T> Load<T>(object poco, ref ICollection<T> loadTo, [CallerMemberName] string name = "")
        {
            // should fail if there is no first
            var column = _entity.Columns.Where(c => c.PropInfo.Name == name).First();
            var relation = column.Relation;
            if (relation.GetType() == typeof(OneToMany))
            {
                return LoadOneToMany<T>(poco, column, ref loadTo, relation as OneToMany);
            }
            else if (relation.GetType() == typeof(Fk))
            {

            }
            else if (relation.GetType() == typeof(ManyToMany))
            {

            }
            throw new NotImplementedException("RelationType not supported");
        }

        public T Load<T>(object poco, ref T loadTo, [CallerMemberName] string name = "")
        {
            var column = _entity.Columns.Where(c => c.PropInfo.Name == name).First();
            var relation = column.Relation;

            if (relation.GetType() == typeof(ManyToOne))
            {
                return LoadManyToOne<T>(poco, column, ref loadTo, relation as ManyToOne);
            }
            throw new NotImplementedException("RelationType not supported");
        }



        private ICollection<T> LoadOneToMany<T>(object poco, Column column, ref ICollection<T> loadTo, OneToMany relation)
        {
            var targetEntity = relation.MappedByEntity;
            var selectQueryBuilder = new SelectQueryBuilder<T>(_ctx, null, relation.MappedByPocoType);

            var whereColumn = targetEntity.Columns.Where(c => c.PropInfo == relation.MappedByProperty).First();
            // primary key of the current object! 
            var whereValue = _entity.PkColumn.PropInfo.GetMethod.Invoke(poco, new object[] { });
            selectQueryBuilder = selectQueryBuilder
                .Where(
                    BinaryExpression.Eq(
                        new ColumnExpression(whereColumn.Name),
                        new ValueExpression(whereValue, whereColumn.DbType.PStmtDbType)
                    )
                );
            var result = selectQueryBuilder.Build().Execute().ToList();
            return result;
        }

        private T LoadManyToOne<T>(object poco, Column column, ref T loadTo, ManyToOne relation)
        {
            var cache = _ctx.Cache;
            var targetEntity = relation.ToEntity;
            var selectQueryBuilder = new SelectQueryBuilder<T>(_ctx, null, relation.ToPocoType);

            var pk = _entity.PkColumn.PropInfo.GetMethod.Invoke(poco, new object[] { });
            // load shadowEntity from cache because it holds the foreign key
            var cachePoco = cache.GetOrInsert(_entity, (long)pk, poco);
            cachePoco.ShadowAttributes.TryGetValue(column.Name, out var fk);

            var whereColumn = relation.To;

            selectQueryBuilder = selectQueryBuilder
                .Where(
                    BinaryExpression.Eq(
                        new ColumnExpression(whereColumn.Name),
                        new ValueExpression(fk, whereColumn.DbType.PStmtDbType)
                    )
                );
            var result = selectQueryBuilder.Build().Execute().ToList();
            return result.FirstOrDefault();
        }



    }
}
