using System;
using MongoDB.Bson.Serialization.Attributes;
using Uniform;

namespace Prototype.Views
{
    public class PatientView
    {
        [DocumentId, BsonId]
        public String PatientId { get; set; }

        public String SiteId { get; set; }
        public String Name { get; set; }
        public String Initials { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Int32 Level { get; set; }         
    }
}