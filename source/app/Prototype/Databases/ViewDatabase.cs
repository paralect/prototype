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
        public const string Subjects = "subjects";
        public const string SubjectsHistory = "subjects_history";
        public const string SubjectsReduced = "subjects_reduced";

        public const string Sites = "sites";
        public const string SitesHistory = "sites_history";
    }

    public class ViewDatabase
    {
        #region ViewDatabase 
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

        #endregion

        public IDocumentCollection<SubjectView> Subjects
        {
            get { return GetMongoCollection<SubjectView>(ViewCollections.Subjects); }
        }

        public IDocumentCollection<SubjectView> SubjectsHistory
        {
            get { return GetMongoCollection<SubjectView>(ViewCollections.SubjectsHistory); }
        }

        public IDocumentCollection<SubjectViewReduced> SubjectsReduced
        {
            get { return GetMongoCollection<SubjectViewReduced>(ViewCollections.SubjectsReduced); }
        }

        public IDocumentCollection<SiteView> Sites
        {
            get { return GetMongoCollection<SiteView>(ViewCollections.Sites); }
        }

        public IDocumentCollection<SiteView> SitesHistory
        {
            get { return GetMongoCollection<SiteView>(ViewCollections.SitesHistory); }
        }
    }
}