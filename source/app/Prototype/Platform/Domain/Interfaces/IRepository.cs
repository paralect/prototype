using System;

namespace Prototype.Platform.Domain
{
    public interface IRepository 
    {
        void Save(AggregateRoot aggregate);

        /// <summary>
        /// Generic version
        /// </summary>
        TAggregate GetById<TAggregate>(String id)
            where TAggregate : AggregateRoot;

        /// <summary>
        /// Perform action on aggregate with specified id.
        /// Aggregate should be already created.
        /// </summary>
        void Perform<TAggregate>(String id, Action<TAggregate> action)
            where TAggregate : AggregateRoot;
    }

    public interface IRepository<TAggregate> where TAggregate : AggregateRoot
    {
        void Save(TAggregate aggregate);

        /// <summary>
        /// Generic version
        /// </summary>
        TAggregate GetById(String id);

        /// <summary>
        /// Perform action on aggregate with specified id.
        /// Aggregate should be already created.
        /// </summary>
        void Perform(String id, Action<TAggregate> action);
    }
}