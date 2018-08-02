using MongoDB.Driver;
using System.Configuration;

namespace EvoGen.Repository.Context
{
    public class MongoDbContext<TColletion> where TColletion: class
    {
        private readonly IMongoDatabase _database;

        public MongoDbContext()
        {
            var mongoConect = ConfigurationManager.AppSettings["MongoConect"];
            var dataBase = ConfigurationManager.AppSettings["DataBase"];
            if (string.IsNullOrEmpty(mongoConect) || string.IsNullOrEmpty(dataBase))
                throw new System.Exception("Cannot find MongoConect or DataBase");

            var client = new MongoClient(mongoConect);
            if (client != null)
                _database = client.GetDatabase(dataBase);
        }

        public IMongoCollection<TColletion> Collection
        {
            get
            {
                return _database.GetCollection<TColletion>(typeof(TColletion).Name);
            }
        }
    }
}
