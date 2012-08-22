using System;
using Prototype.Views;
using Uniform;

namespace Prototype.Databases
{
    public static class ViewDatabases
    {
        public const string Mongodb = "mongodb";
        public const string Sql = "sql";
    }

    public static class ViewCollections
    {
        public const string Subjects = "Subjects";
        public const string SubjectsReduced = "Subjects_reduced";
        public const string Sites = "Sites";
    }

    public class ViewDatabase
    {
        private readonly UniformDatabase _db;

        public ViewDatabase(UniformDatabase db)
        {
            _db = db;
        }

        /// <summary>
        /// Helper method for mongodb collections
        /// </summary>
        private IDocumentCollection<TDocument> GetMongoCollection<TDocument>(String collectionName) where TDocument : new()
        {
            return _db.GetCollection<TDocument>(ViewDatabases.Mongodb, collectionName);
        }

        /// <summary>
        /// Helper method for sql collections (tables)
        /// </summary>
        private IDocumentCollection<TDocument> GetSqlCollection<TDocument>(String tableName) where TDocument : new()
        {
            return _db.GetCollection<TDocument>(ViewDatabases.Sql, tableName);
        }


        public IDocumentCollection<SubjectView> Subjects
        {
            get { return GetMongoCollection<SubjectView>(ViewCollections.Subjects); }
        }

        public IDocumentCollection<SubjectViewReduced> SubjectsReduced
        {
            get { return GetMongoCollection<SubjectViewReduced>(ViewCollections.SubjectsReduced); }
        }

        public IDocumentCollection<SiteView> Sites
        {
            get { return GetMongoCollection<SiteView>(ViewCollections.Sites); }
        }
    }
}