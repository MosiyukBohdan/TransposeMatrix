using Share.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IMatrixService
    {
        Task<Matrix> GetMatrixById(string id);

        Task<IEnumerable<Matrix>> GetAll();

        Task<Matrix> GenerateMatrix(int size);

        Task<Matrix> TransposeMatrix(string id);
    }
}
