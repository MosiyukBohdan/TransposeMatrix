using DAL.DataContext;
using DAL.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;
using Share.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class MatrixRepository : IMatrixRepository
    {
        private readonly IMongoCollection<Matrix> matrixes;

        public MatrixRepository(MatrixContext matrixContext) => matrixes = matrixContext.Matrixes;

        public async Task<Matrix> Get(string id) => await matrixes.Find(new BsonDocument("_id", new ObjectId(id))).FirstOrDefaultAsync();

        public async Task<IEnumerable<Matrix>> GetAll() => await matrixes.Find(new BsonDocument()).ToListAsync();

        public async Task<Matrix> Save(Matrix model)
        {
            await matrixes.InsertOneAsync(model);
            return model;
        }

        public async Task<Matrix> Update(Matrix model)
        {
            await matrixes.ReplaceOneAsync(m => m.Id == model.Id, model);
            return model;
        }
    }
}
