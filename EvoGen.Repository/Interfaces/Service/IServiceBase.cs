using EvoGen.Repository.Collection;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EvoGen.Repository.Interfaces.Service
{
    public interface IServiceBase<TCollection> where TCollection : MongoDbBase
    {
        JToken GetAll();
        JToken GetById(long controlId);
        TCollection Create(JToken obj);
        long Update(JToken obj);
        long Delete(string guidString);
    }
}
