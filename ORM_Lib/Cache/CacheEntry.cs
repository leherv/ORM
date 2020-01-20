using ORM_Lib.Attributes;
using ORM_Lib.DbSchema;
using ORM_Lib.Saving;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ORM_Lib.Cache
{
    class CacheEntry
    {
        private DbContext _ctx;
        private Entity _entity;
        public object Poco { get; }
        public Dictionary<string, object> OriginalPoco = new Dictionary<string, object>();
        public Dictionary<string, object> ShadowAttributes = new Dictionary<string, object>();
        public List<object> ManyToManyKeys = new List<object>();

        public CacheEntry(object poco, Entity entity, DbContext ctx)
        {
            Poco = poco;
            _entity = entity;
            _ctx = ctx;
            entity.Columns.Where(col => col.Relation.GetType() == typeof(ManyToMany));
        }


        // we prepare here because if we have a manytoone we have to update the entity where the manytoone sits on
        // but if we have onetomany we have to update the entity on the other hand - so the change can not be on the entity onetomany sits on
        // therefore we set the object on the other side and collect changes will find it then and update the correct entity
        // also the other object is correctly set without needing to fetch from the database!
        public void PrepareForCollectChanges()
        {
            var pk = _entity.PkColumn.PropInfo.GetMethod.Invoke(Poco, new object[0]);
            foreach (var col in _entity.CombinedColumns().Where(c => c.Relation?.GetType() == typeof(OneToMany)))
            {
                //get the list
                var objects = col.PropInfo.GetMethod.Invoke(Poco, new object[0]);
                if (objects != null)
                {
                    // convert to real list
                    var objs = objects as ICollection;
                    if (objs != null && objs.Count > 0)
                    {
                        var relation = col.Relation as OneToMany;
                        var entity = relation.MappedByEntity;
                        var toSet = relation.MappedByProperty;

                        foreach (var obj in objs)
                        {
                            var objPk = entity.PkColumn.PropInfo.GetMethod.Invoke(obj, new object[0]);
                            if (objPk == null || ValuesEqual(0L, objPk)) throw new InvalidOperationException("Trying to add an unmanaged object!");

                            // now we set the object to our current Poco 
                            toSet.SetMethod.Invoke(obj, new object[] { Poco });
                        }
                    }
                }

            }
        }


        public (PocoUpdateChange, List<PocoInsertChange>) CalculateChange()
        {
            var insertChanges = new List<PocoInsertChange>();
            Dictionary<string, object> newValues = new Dictionary<string, object>();
            foreach (var col in _entity.CombinedColumns())
            {
                if (col.IsDbColumn)
                {
                    if (col.IsShadowAttribute)
                    {
                        var currentObj = col.PropInfo.GetMethod.Invoke(Poco, new object[0]);
                        if (currentObj != null)
                        {
                            var cEntity = _ctx.Schema.GetByType(currentObj.GetType());
                            var fk = cEntity.PkColumn.PropInfo.GetMethod.Invoke(currentObj, new object[0]);
                            if (fk == null || ValuesEqual(fk, 0L)) throw new InvalidOperationException("Trying to add an unmanaged object!");
                            if (!ValuesEqual(ShadowAttributes[col.Name], fk))
                            {
                                newValues[col.Name] = fk;
                            }
                        }

                    }
                    else
                    {
                        var value = col.PropInfo.GetMethod.Invoke(Poco, new object[0]);
                        // handle null in dictionary here
                        if (!ValuesEqual(OriginalPoco[col.Name], value))
                        {
                            newValues[col.Name] = value;
                        }
                    }
                }
                else
                {
                    if (col.Relation is ManyToMany relation)
                    {
                        var objects = col.PropInfo.GetMethod.Invoke(Poco, new object[0]);
                        if (objects != null)
                        {
                            // convert to real list
                            var objs = objects as ICollection;
                            if (objs != null && objs.Count > 0)
                            {
                                var entity = relation.ToEntity;
                                foreach (var obj in objs)
                                {
                                    // PROBLEM we add manytomany objs everytime with insertstatements!
                                    // check if managed
                                    var objPk = entity.PkColumn.PropInfo.GetMethod.Invoke(obj, new object[0]);
                                    if (objPk == null || ValuesEqual(0L, objPk)) throw new InvalidOperationException("Trying to add an unmanaged object!");

                                    // we only continue if it doesnt exist yet in db
                                    if(!ManyToManyKeys.Contains(objPk))
                                    {
                                        // set the other side too! (simulates a refetch with new key from database for the other side)
                                        var otherToSet = entity.Columns
                                            .Select(c => c.PropInfo)
                                            .Where(arg => arg.GetMethod.ReturnType.GenericTypeArguments.Length > 0 && arg.GetMethod.ReturnType.GenericTypeArguments[0] == Poco.GetType()).FirstOrDefault();

                                        if (otherToSet != null)
                                        {
                                            var otherSideCurrent = otherToSet.GetMethod.Invoke(obj, new object[0]);
                                            // other collection was not set/loaded yet so we should not set it - will be loaded next time anyway
                                            if (otherSideCurrent != null)
                                            {
                                                // we now want to add the object to the collection if it not already exists
                                                var currentObjs = otherSideCurrent as IList;
                                                if (!currentObjs.Contains(Poco))
                                                    currentObjs.Add(Poco);
                                            }
                                        }

                                        insertChanges.Add(new PocoInsertChange(
                                           relation.TableName,
                                           (relation.ForeignKeyNear, _entity.PkColumn.PropInfo.GetMethod.Invoke(Poco, new object[0])),
                                           (relation.ForeignKeyFar, objPk)
                                        ));
                                        // now we add it so we do not write to database again
                                        ManyToManyKeys.Add(objPk);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return (new PocoUpdateChange(newValues, _entity, _entity.PkColumn.PropInfo.GetMethod.Invoke(Poco, new object[0])), insertChanges);
        }


        private static bool ValuesEqual(object value1, object value2)
        {
            bool areValuesEqual = true;
            IComparable selfValueComparer = value1 as IComparable;

            // one of the values is null            
            if (value1 == null && value2 != null || value1 != null && value2 == null)
                areValuesEqual = false;
            else if (selfValueComparer != null && selfValueComparer.CompareTo(value2) != 0)
                areValuesEqual = false;
            else if (!object.Equals(value1, value2))
                areValuesEqual = false;

            return areValuesEqual;
        }
    }

}
