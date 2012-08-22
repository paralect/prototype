using System;
using Prototype.Views;
using Uniform;

namespace Prototype.Databases
{
    public static class ViewDatabases
    {
        public const String Mongodb = "mongodb";
        public const String Sql = "sql";
    }

    public static class ViewCollections
    {
        public const String Patients = "patients";
        public const String PatientsReduced = "patients_reduced";
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


        public IDocumentCollection<PatientView> Patients
        {
            get { return GetMongoCollection<PatientView>(ViewCollections.Patients); }
        }

        public IDocumentCollection<PatientViewReduced> PatientsReduced
        {
            get { return GetMongoCollection<PatientViewReduced>(ViewCollections.PatientsReduced); }
        }
    }
}