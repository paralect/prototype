using MongoDB.Bson;
using MongoDB.Bson.IO;
using Prototype.Admin.Models;
using Prototype.Platform.Domain;
using Prototype.Platform.Mongo;

namespace Prototype.Admin.Helpers
{
    public class MessageHelper
    {
        public static MessageJsonModel GetJson(ICommand cmd)
        {
            var evntBson = cmd.ToBsonDocument(cmd.GetType());
            return GetJson(evntBson);
        }

        public static MessageJsonModel GetJson(IEvent evnt)
        {
            var evntBson = evnt.ToBsonDocument(evnt.GetType());
            return GetJson(evntBson);
        }

        public static MessageJsonModel GetJson(BsonDocument document)
        {
            if (document == null)
                return new MessageJsonModel();

            var sett = new JsonWriterSettings();
            sett.Indent = true;

            var evntBson = document;
            var metadata = evntBson.GetBsonDocument("Metadata");

            evntBson.Remove("Metadata");
            evntBson.Remove("_t");
            metadata.Remove("_t");

            return new MessageJsonModel()
            {
                Message = evntBson.ToJson(sett),
                Metadata = metadata.ToJson(sett)
            };
        }
    }
}