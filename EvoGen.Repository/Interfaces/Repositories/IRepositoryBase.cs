using EvoGen.Repository.Collection;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EvoGen.Repository.Interfaces.Repositories
{
    public interface IRepositoryBase<TCollection> where TCollection : MongoDbBase
    {
        IQueryable<TCollection> GetAll();
        TCollection GetById(long controlId);
        TCollection Create(TCollection obj);
        long Update(TCollection obj);
        long Delete(string guidString);
    }
}
