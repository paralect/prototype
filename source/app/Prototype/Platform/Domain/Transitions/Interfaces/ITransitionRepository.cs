using System;
using System.Collections.Generic;

namespace Prototype.Platform.Domain.Transitions.Interfaces
{
    public interface ITransitionRepository
    {
        /// <summary>
        /// Append transition to store
        /// </summary>
        void AppendTransition(Transition transition);

        /// <summary>
        /// Get transitions for specified streamId (aggregate ID).
        /// Will return empty list, if such stream not exists.
        /// </summary>
        List<Transition> GetTransitions(String streamId, Int32 fromVersion, Int32 toVersion);

        /// <summary>
        /// Get all transitions ordered ascendantly by Timestamp
        /// </summary>
        IEnumerable<Transition> GetTransitions();

        /// <summary>
        /// Get transitions paged and ordered ascendantly by Timestamp
        /// </summary>
        IEnumerable<Transition> GetTransitions(Int32 startIndex, Int32 count);

        /// <summary>
        /// Remove single transition from stream
        /// </summary>
        void RemoveTransition(String streamId, Int32 version);

        /// <summary>
        /// Remove stream (and all transitions belonging to this stream)
        /// </summary>
        /// <param name="streamId"></param>
        void RemoveStream(String streamId);

        /// <summary>
        /// Returns total number of transitions in store
        /// </summary>
        /// <returns></returns>
        Int64 CountTransitions();

        /// <summary>
        /// Build indexes for transitions
        /// </summary>
        void EnsureIndexes();
    }
}
