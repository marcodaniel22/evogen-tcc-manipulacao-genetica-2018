using EvoGen.Domain.Collections;
using System.Linq;

namespace EvoGen.Domain.Interfaces.Services
{
    public interface IServiceBase<TCollection> where TCollection : MongoDbBase
    {
        TCollection Create(TCollection obj);
        IQueryable<TCollection> GetAll();
    }
}
