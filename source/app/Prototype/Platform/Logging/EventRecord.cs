using MongoDB.Bson;
using Prototype.Platform.Domain;
using Prototype.Platform.Mongo;

namespace Prototype.Platform.Logging
{
    public class EventRecord
    {
        public BsonDocument EventDocument { get; set; }
        public EventMetadata Metadata { get; set; }
        public EventHandlerRecordCollection Handlers { get; set; }

        public static EventRecord FromBson(BsonDocument doc)
        {
            var eventDocument = doc.GetBsonDocument("Event");
            var record = new EventRecord()
            {
                EventDocument = eventDocument,
                Metadata = eventDocument.GetBsonDocument("Metadata").CreateEventMetadata(),
                Handlers = EventHandlerRecordCollection.FromBson(doc.GetBsonArray("Handlers"))
            };

            return record;
        }  
    }
}