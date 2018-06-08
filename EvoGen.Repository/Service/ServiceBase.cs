using EvoGen.Repository.Collection;
using EvoGen.Repository.Interfaces.Factory;
using EvoGen.Repository.Interfaces.Repositories;
using EvoGen.Repository.Interfaces.Service;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvoGen.Repository.Service
{
    public class ServiceBase<TCollection> : IServiceBase<TCollection> where TCollection : MongoDbBase
    {
        private readonly IFactoryBase<TCollection> _factoryBase;
        private readonly IRepositoryBase<TCollection> _repositoryBase;

        public ServiceBase(IRepositoryBase<TCollection> repositoryBase, IFactoryBase<TCollection> factoryBase)
        {
            this._repositoryBase = repositoryBase;
            this._factoryBase = factoryBase;
        }

        public TCollection Create(JToken obj)
        {
            return _repositoryBase.Create(_factoryBase.getCollectionByJToken(obj));
        }

        public long Delete(string guidString)
        {
            return _repositoryBase.Delete(guidString);
        }

        public JToken GetAll()
        {
            return _factoryBase.getJTokenByCollectionList(_repositoryBase.GetAll().ToList());
        }

        public JToken GetById(long controlId)
        {
            return _factoryBase.getJTokenByCollection(_repositoryBase.GetById(controlId));
        }

        public long Update(JToken obj)
        {
            return _repositoryBase.Update(_factoryBase.getCollectionByJToken(obj));
        }
    }
}
