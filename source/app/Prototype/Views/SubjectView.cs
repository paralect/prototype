using System;
using MongoDB.Bson.Serialization.Attributes;
using Uniform;

namespace Prototype.Views
{
    public class SubjectView
    {
        /// <summary>
        /// SubjectView used also for history documents.
        /// In history collection Id will be in a form of "{subjectId}/{version}".
        /// In "normal" collection Id will be equal to SubjectId property
        /// </summary>
        [DocumentId, BsonId]
        public string Id { get; set; }

        public string SubjectId { get; set; }
        public int Version { get; set; }

        public string Name { get; set; }
        public string Initials { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int Level { get; set; }

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