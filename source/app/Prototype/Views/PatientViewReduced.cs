using System;
using MongoDB.Bson.Serialization.Attributes;
using Uniform;

namespace Prototype.Views
{
    public class PatientViewReduced
    {
        [DocumentId, BsonId]
        public String PatientId { get; set; }
        public String Name { get; set; }
    }
}