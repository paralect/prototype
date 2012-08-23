using MongoDB.Bson.Serialization.Attributes;
using Uniform;

namespace Prototype.Views
{
    public class SiteView
    {
        /// <summary>
        /// SubjectView used also for history documents.
        /// In history collection Id will be in a form of "{subjectId}/{version}".
        /// In "normal" collection Id will be equal to SubjectId property
        /// </summary>
        [DocumentId, BsonId]
        public string Id { get; set; }

        public string SiteId { get; set; }

        public int Version { get; set; }

        public string Name { get; set; }
        public int Capacity { get; set; }
    }
}