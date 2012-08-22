using System;
using System.Collections.Generic;
using MongoDB.Bson;
using System.Linq;
using Prototype.Platform.Domain.Transitions;

namespace Prototype.Admin.Models
{
    public class AggregatePage
    {
        public string Id { get; set; }
        public BsonDocument CurrentAggregateState { get; set; }
        public List<Transition> Transitions { get; set; }

        public Int32 EventsCount 
        {
            get { return Transitions.SelectMany(t => t.Events).Count(); } 
        }

        public AggregatePage()
        {
        }

        public AggregatePage(String id, List<Transition> transitions, BsonDocument currentAggregateState)
        {
            Id = id;
            CurrentAggregateState = currentAggregateState;
            Transitions = transitions;
        }
    }
}