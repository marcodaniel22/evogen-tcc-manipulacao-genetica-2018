using EvoGen.Repository.Collection;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EvoGen.Repository.Interfaces.Repositories
{
    public interface IElementRepository : IRepositoryBase<Element>
    {
        Task<Element> GetBySymbol(string symbol);
    }
}
