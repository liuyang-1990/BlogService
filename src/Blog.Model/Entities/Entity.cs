
namespace Blog.Model.Entities
{
    public abstract class Entity : Entity<string>, IEntity
    {

    }

    public abstract class Entity<TPrimaryKey> : IEntity<TPrimaryKey>
    {
        public virtual TPrimaryKey Id { get; set; }
        public override string ToString()
        {
            return $"[{GetType().Name} {Id}]";
        }
    }
}