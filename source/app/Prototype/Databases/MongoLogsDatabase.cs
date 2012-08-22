using System;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace Prototype.Databases
{
    public class MongoLogsDatabase
    {
        /// <summary>
        /// MongoDB Server
        /// </summary>
        private readonly MongoServer _server;

        /// <summary>
        /// Name of database 
        /// </summary>
        private readonly string _databaseName;

        /// <summary>
        /// Opens connection to MongoDB Server
        /// </summary>
        public MongoLogsDatabase(String connectionString)
        {
            _databaseName = MongoUrl.Create(connectionString).DatabaseName;
            _server = MongoServer.Create(connectionString);
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

        public MongoLogsDatabase EnsureIndexes()
        {
            Logs.EnsureIndex(IndexKeys.Descending("Command.Metadata.CreatedDate"));
            Logs.EnsureIndex(IndexKeys.Descending("Command.Metadata.CreatedDate").Ascending("Errors"));
            return this;
        }


        public MongoCollection Logs
        {
            get { return Database.GetCollection("logs"); }
        }
    }
}
