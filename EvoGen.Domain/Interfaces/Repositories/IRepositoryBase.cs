using EvoGen.Domain.Collections;
using System.Linq;

namespace EvoGen.Domain.Interfaces.Repositories
{
    public interface IRepositoryBase<TCollection> where TCollection : MongoDbBase
    {
        IQueryable<TCollection> GetAll();
        TCollection GetById(string guidString);
        TCollection Create(TCollection obj);
        TCollection Update(TCollection obj);
        TCollection Delete(string guidString);
    }
}
