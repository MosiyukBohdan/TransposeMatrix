using System.Collections.Generic;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface IRepository<T>
    {
        Task<T> Get(string id);

        Task<IEnumerable<T>> GetAll();

        Task<T> Save(T model);

        Task<T> Update(T model);
    }
}
