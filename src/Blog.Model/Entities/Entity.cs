
using SqlSugar;

namespace Blog.Model.Entities
{
    public abstract class Entity : Entity<int>, IEntity
    {

    }

    public abstract class Entity<TPrimaryKey> : IEntity<TPrimaryKey>
    {
        [SugarColumn(IsNullable = false, IsPrimaryKey = true, IsIdentity = true)]
        public virtual TPrimaryKey Id { get; set; }
        public override string ToString()
        {
            return $"[{GetType().Name} {Id}]";
        }
    }
}