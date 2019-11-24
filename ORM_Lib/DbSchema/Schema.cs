using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace ORM_Lib.DbSchema
{
    internal class Schema
    {
        private readonly Dictionary<Type, Entity> _entitiesByType = new Dictionary<Type, Entity>();
        private readonly Dictionary<string, Entity> _entitiesByName = new Dictionary<string, Entity>();
        public ReadOnlyCollection<Entity> Entities { get; set; }

        public Schema(List<Entity> entities)
        {
            Entities = new ReadOnlyCollection<Entity>(entities);
            entities.ForEach(entity =>
            {
                _entitiesByName.Add(entity.Name, entity);
                _entitiesByType.Add(entity.PocoType, entity);
            });
        }
        
        public Entity GetByName(string name)
        {
            return _entitiesByName[name];
        }
        
        public Entity GetByType(Type t)
        {
            return _entitiesByType[t];
        }
    }
}