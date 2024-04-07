namespace LotteryDDD.Domain.Common
{
    public abstract class Entity<T> : IEntity<T>
    {
        public T Id { get; set; }
        public bool IsDeleted { get; set; }
    }

    public abstract class Aggregate<TId> : Entity<TId>, IAggregate<TId>
    {

    }

    public interface IAggregate<T> : IAggregate, IEntity<T>
    {
    }

    public interface IAggregate : IEntity
    {
    }

    public interface IEntity<T> : IEntity
    {
        public T Id { get; set; }
    }

    public interface IEntity
    {
        public bool IsDeleted { get; set; }
    }
}
