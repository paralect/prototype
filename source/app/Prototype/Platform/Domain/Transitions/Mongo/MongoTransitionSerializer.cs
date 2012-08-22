using System;
using System.Collections.Generic;
using MongoDB.Bson;

namespace Prototype.Platform.Domain.Transitions.Mongo
{
    public class MongoTransitionSerializer
    {
        private readonly MongoTransitionDataSerializer _dataSerializer;

        public MongoTransitionSerializer()
        {
            _dataSerializer = new MongoTransitionDataSerializer();
        }

        /// <summary>
        /// Serialize transiton to BsonDocument
        /// </summary>
        public BsonDocument Serialize(Transition transition)
        {
            return new BsonDocument 
            {
                { "_id", SerializeTransitionId(transition.Id) },
                { "AggregateTypeId", transition.AggregateTypeId },
                { "Timestamp", transition.Timestamp },
                { "Events", SerializeTransitionEvents(transition.Events) },
            };
        }

        /// <summary>
        /// Deserialize from BsonDocument to Transition
        /// </summary>
        public Transition Deserialize(BsonDocument doc)
        {
            var transitionId = DeserializeTransitionId(doc["_id"]);
            var datetime = doc["Timestamp"].AsDateTime;
            var aggregateTypeId = GetString(doc, "AggregateTypeId");
            var events = DeserializeTransitionEvents(doc["Events"]);
            return new Transition(transitionId, aggregateTypeId, datetime, events);
        }

        private static string GetString(BsonDocument doc, string key, string defaultValue = "")
        {
            BsonValue value;
            var contains = doc.TryGetValue(key, out value);

            if (!contains || !value.IsString)
                return defaultValue;

            return value.AsString;
        }



        #region Transition Id Serialization

        public BsonDocument SerializeTransitionId(TransitionId transitionId)
        {
            return new BsonDocument 
            {
                { "StreamId", transitionId.StreamId }, 
                { "Version", transitionId.Version }
            };
        }

        private TransitionId DeserializeTransitionId(BsonValue id)
        {
            if (!id.IsBsonDocument)
                throw new Exception("Transition _id should be a BsonDocument.");

            var transitionId = id.AsBsonDocument;
            var streamId = transitionId["StreamId"].AsString;
            var version = transitionId["Version"].AsInt32;

            return new TransitionId(streamId, version);
        }

        #endregion

        #region Transition Events Serialization

        private BsonArray SerializeTransitionEvents(List<TransitionEvent> events)
        {
            BsonArray array = new BsonArray();

            foreach (var e in events)
            {
                array.Add(new BsonDocument()
                {
                    { "TypeId", e.TypeId },
                    { "Data", _dataSerializer.Serialize(e.Data) }
                });
            }

            return array;
        }

        private List<TransitionEvent> DeserializeTransitionEvents(BsonValue bsonValue)
        {
            if (!bsonValue.IsBsonArray)
                throw new Exception("Events should always be an array.");

            var eventArray = bsonValue.AsBsonArray;

            var events = new List<TransitionEvent>();
            foreach (var eventValue in eventArray)
            {
                var eventDoc = eventValue.AsBsonDocument;

                var eventTypeId = eventDoc["TypeId"].AsString;

                var eventType = Type.GetType(eventTypeId);

                if (eventType == null)
                    throw new Exception(String.Format("Cannot load this type: {0}. Make sure that assembly containing this type is referenced by your project.", eventTypeId));

                var eventData = _dataSerializer.Deserialize(eventDoc["Data"].AsBsonDocument, eventType);

                events.Add(new TransitionEvent(eventTypeId, eventData));
            }

            return events;
        }

        #endregion
    }
}
