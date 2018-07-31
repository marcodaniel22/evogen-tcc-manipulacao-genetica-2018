using EvoGen.Domain.Collections;
using EvoGen.Domain.Interfaces.Repositories;
using EvoGen.Domain.Interfaces.Services;

namespace EvoGen.Domain.Services
{
    public class ServiceBase<TCollection> : IServiceBase<TCollection> where TCollection : MongoDbBase
    {
        private readonly IRepositoryBase<TCollection> _repositoryBase;

        public ServiceBase(IRepositoryBase<TCollection> repositoryBase)
        {
            this._repositoryBase = repositoryBase;
        }

        public TCollection Create(TCollection obj)
        {
            return _repositoryBase.Create(obj);
        }
    }
}
