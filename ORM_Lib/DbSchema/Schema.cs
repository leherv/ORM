using System.Collections.Generic;
using System.Linq;

namespace ORM_Lib.DbSchema
{
    public class Schema
    {
        public List<Entity> Entities { get; set; }

        public Schema(List<Entity> entities)
        {
            Entities = entities;
        }

        public Entity GetByName(string name)
        {
            return Entities.Single(entity => name.Equals(entity.Name));
        }
    }
}