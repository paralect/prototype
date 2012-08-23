using System;
using MongoDB.Driver;
using Prototype.Platform.Mongo;
using Prototype.Views;

namespace Prototype.Databases
{
    public class MongoViewDatabase
    {
        /// <summary>
        /// MongoDB Server
        /// </summary>
        private readonly MongoServer _server;

        /// <summary>
        /// Name of database 
        /// </summary>
        private readonly string _databaseName;

        public MongoUrl MongoUrl { get; private set; }

        /// <summary>
        /// Opens connection to MongoDB Server
        /// </summary>
        public MongoViewDatabase(String connectionString)
        {
            MongoUrl = MongoUrl.Create(connectionString);
            _databaseName = MongoUrl.DatabaseName;
            _server = MongoServer.Create(connectionString);
        }

        public MongoViewDatabase EnsureIndexes()
        {
            // build indexes here
            return this;
        }

        /// <summary>
        /// MongoDB Server
        /// </summary>
        public MongoServer Server
        {
            get { return _server; }
        }

        /// <summary>
        /// Get database
        /// </summary>
        public MongoDatabase Database
        {
            get { return _server.GetDatabase(_databaseName); }
        }

        protected ReadonlyMongoCollection GetCollection(String collectionName)
        {
            return Database.GetCollection(collectionName).ToReadonly();
        }

        protected ReadonlyMongoCollection<TDocument> GetCollection<TDocument>(String collectionName)
        {
            return Database.GetCollection<TDocument>(collectionName).ToReadonly();
        }

        public ReadonlyMongoCollection<SubjectView> Subjects
        {
            get { return GetCollection<SubjectView>(ViewCollections.Subjects); }
        }

        public ReadonlyMongoCollection<SubjectView> SubjectsHistory
        {
            get { return GetCollection<SubjectView>(ViewCollections.SubjectsHistory); }
        }

        public ReadonlyMongoCollection<SiteView> Sites
        {
            get { return GetCollection<SiteView>(ViewCollections.Sites); }
        }

        public ReadonlyMongoCollection<SiteView> SitesHistory
        {
            get { return GetCollection<SiteView>(ViewCollections.SitesHistory); }
        }
    }
}
