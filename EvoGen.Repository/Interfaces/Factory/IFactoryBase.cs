using EvoGen.Repository.Collection;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace EvoGen.Repository.Interfaces.Factory
{
    public interface IFactoryBase<TCollection> where TCollection : MongoDbBase
    {
        TCollection getCollectionByJToken(JToken jtoken);
        JToken getJTokenByCollection(TCollection obj);
        JToken getJTokenByCollectionList(IEnumerable<TCollection> list);
    }
}
