using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EvoGen.Repository.Collection;
using EvoGen.Repository.Context;
using EvoGen.Repository.Interfaces.Repositories;
using MongoDB.Driver;

namespace EvoGen.Repository.Repositories
{
    public class ElementRepository : RepositoryBase<Element>, IElementRepository
    {
        private readonly MongoDbContext<Element> _elementContext = null;

        public ElementRepository()
        {
            _elementContext = new MongoDbContext<Element>();
        }

        public async Task<Element> GetBySymbol(string symbol)
        {
            FindOptions<Element> options = new FindOptions<Element> { Limit = 1 };
            IAsyncCursor<Element> task = await _elementContext.Collection.FindAsync(x => x.Symbol.Equals(symbol), options);
            List<Element> list = await task.ToListAsync();
            return list.Count > 0 ? list.FirstOrDefault() : null;
        }
    }
}
