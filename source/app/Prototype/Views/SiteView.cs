using MongoDB.Bson.Serialization.Attributes;
using Uniform;

namespace Prototype.Views
{
    public class SiteView
    {
        [DocumentId, BsonId]
        public string SiteId { get; set; }

        public string Name { get; set; }
        public int Capacity { get; set; }
    }
}