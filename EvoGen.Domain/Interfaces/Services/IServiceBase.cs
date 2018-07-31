using EvoGen.Domain.Collections;

namespace EvoGen.Domain.Interfaces.Services
{
    public interface IServiceBase<TCollection> where TCollection : MongoDbBase
    {
        TCollection Create(TCollection obj);
    }
}
