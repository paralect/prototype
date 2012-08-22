using System;
using MongoDB.Bson.Serialization.Attributes;
using Uniform;

namespace Prototype.Views
{
    public class SubjectView
    {
        [DocumentId, BsonId]
        public string SubjectId { get; set; }


        public string Name { get; set; }
        public string Initials { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Int32 Level { get; set; }

        public string FullName { get; set; }

        public string SiteId { get; set; }
        public string SiteName { get; set; }

        public void UpdateName(String name, string initials)
        {
            Name = name;
            Initials = initials;
            FullName = name + " " + initials;
        }
    }
}