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


        // we only want to lazily load if 
        // 1. the InternalLazyLoader was already set because the object was read from the database or inserted into the database
        // 2. the collection was not already instantiated (either by a user) or by a first lazyload
        // 3. we are not collecting changes at the moment - to avoid lazily fetching everything when updating relations!
        public bool ShouldLoad<T>(T loadTo)
        {
            return !_ctx.Cache.SavingChanges;
        }

        public ICollection<T> Load<T>(object poco, ref ICollection<T> loadTo, string name)
        {
            // if we are updating the db we want to avoid lazily fetching all the collections
            if (!ShouldLoad(loadTo)) return loadTo;

            // should fail if there is no first
            var column = _entity.Columns.Where(c => c.PropInfo.Name == name).First();
            var relation = column.Relation;
            if (relation.GetType() == typeof(OneToMany))
            {
                var result = LoadOneToMany<T>(poco, column, ref loadTo, relation as OneToMany);
                loadTo = result;
                return loadTo;
            }
            else if (relation.GetType() == typeof(Fk))
            {

            }
            else if (relation.GetType() == typeof(ManyToMany))
            {
                var result = LoadManyToMany<T>(poco, column, ref loadTo, relation as ManyToMany);
                loadTo = result;
                return loadTo;
            }
            throw new NotImplementedException("RelationType not supported");
        }

        public T Load<T>(object poco, ref T loadTo, string name)
        {
            // if we are updating the db we want to avoid lazily fetching all the collections
            if (!ShouldLoad(loadTo)) return loadTo;

            var column = _entity.Columns.Where(c => c.PropInfo.Name == name).First();
            var relation = column.Relation;

            if (relation.GetType() == typeof(ManyToOne))
            {
                var result = LoadManyToOne<T>(poco, column, ref loadTo, relation as ManyToOne);
                loadTo = result;
                return loadTo;
            }
            throw new NotImplementedException("RelationType not supported");
        }


        private ICollection<T> LoadManyToMany<T>(object poco, Column column, ref ICollection<T> loadTo, ManyToMany relation)
        {
            var selectQueryBuilder = new SelectQueryBuilder<T>(_ctx, relation.ToPocoType)
                .Join(new Join(
                    relation.ToEntity.Alias, relation.ToEntity.PkColumn.Name, relation.TableName, "relationNeedsAlias", relation.ForeignKeyFar
                 ))
                .Join(new Join(
                    "relatioNneedsAlias", relation.ForeignKeyNear, _entity.Name, _entity.Alias, _entity.PkColumn.Name
                ));

            ICollection<T> resultList = selectQueryBuilder.Build().Execute().ToList();
            // get the current cacheEntry for THIS object
            var currentPk = _entity.PkColumn.PropInfo.GetMethod.Invoke(poco, new object[0]);
            var cacheEntry = _ctx.Cache.GetOrInsert(_entity, (long)currentPk, poco);
            cacheEntry.ManyToManyKeys = new List<object>();
            if (resultList != null && resultList.Count > 0)
            {
                // get primary keys of the objects just loaded and set them in the CacheEntry so we can track them!
                var toEntity = relation.ToEntity;

                foreach (var obj in resultList)
                {
                    var objPk = toEntity.PkColumn.PropInfo.GetMethod.Invoke(obj, new object[0]);
                    if (!cacheEntry.ManyToManyKeys.Contains(objPk))
                        cacheEntry.ManyToManyKeys.Add(objPk);
                }
            }
            return resultList;
        }

        private ICollection<T> LoadOneToMany<T>(object poco, Column column, ref ICollection<T> loadTo, OneToMany relation)
        {
            var targetEntity = relation.MappedByEntity;
            var selectQueryBuilder = new SelectQueryBuilder<T>(_ctx, relation.MappedByPocoType);
            var whereColumn = targetEntity.Columns.Where(c => c.PropInfo == relation.MappedByProperty).First();
            // primary key of the current object! 
            var whereValue = _entity.PkColumn.PropInfo.GetMethod.Invoke(poco, new object[0]);
            selectQueryBuilder = selectQueryBuilder
                .Where(
                    BinaryExpression.Eq(
                        new ColumnExpression(whereColumn.Name),
                        new ValueExpression(whereValue, whereColumn.DbType.PStmtDbType)
                    )
                );
            return selectQueryBuilder.Build().Execute().ToList();
        }

        private T LoadManyToOne<T>(object poco, Column column, ref T loadTo, ManyToOne relation)
        {
            var cache = _ctx.Cache;
            var targetEntity = relation.ToEntity;
            var selectQueryBuilder = new SelectQueryBuilder<T>(_ctx, relation.ToPocoType);

            var pk = _entity.PkColumn.PropInfo.GetMethod.Invoke(poco, new object[0]);
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
            return selectQueryBuilder.Build().Execute().FirstOrDefault();
        }


    }
}
