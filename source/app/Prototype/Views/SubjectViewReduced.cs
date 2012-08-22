using System;
using MongoDB.Bson.Serialization.Attributes;
using Uniform;

namespace Prototype.Views
{
    public class SubjectViewReduced
    {
        [DocumentId, BsonId]
        public string SubjectId { get; set; }
        public string Name { get; set; }
    }
}