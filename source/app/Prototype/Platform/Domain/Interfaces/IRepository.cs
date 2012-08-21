using System;
using Prototype.Domain.Aggregates;

namespace Prototype.Platform.Domain
{
    public interface IRepository 
    {
        void Save(String aggregateId, Aggregate aggregate);

        /// <summary>
        /// Generic version
        /// </summary>
        TAggregate GetById<TAggregate>(String id)
            where TAggregate : Aggregate;

        /// <summary>
        /// Perform action on aggregate with specified id.
        /// Aggregate should be already created.
        /// </summary>
        void Perform<TAggregate>(String id, Action<TAggregate> action)
            where TAggregate : Aggregate;
    }

    public interface IRepository<TAggregate> where TAggregate : Aggregate
    {
        void Save(String aggregateId, TAggregate aggregate);

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