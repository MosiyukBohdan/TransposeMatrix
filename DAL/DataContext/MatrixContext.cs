using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using Newtonsoft.Json;
using Share.Models;

namespace DAL.DataContext
{
    public class MatrixContext
    {
        private readonly IMongoDatabase database;
        public MatrixContext(string connectionString)
        {
            var client = new MongoClient(connectionString);

            database = client.GetDatabase("Matrixes");
        }

        public IMongoCollection<Matrix> Matrixes
        {
            get => database.GetCollection<Matrix>("Matrixes");
        }
    }
}
