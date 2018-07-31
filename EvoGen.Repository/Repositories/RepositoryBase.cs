using EvoGen.Domain.Collections;
using EvoGen.Domain.Interfaces.Repositories;
using EvoGen.Repository.Context;
using MongoDB.Driver;
using System.Linq;

namespace EvoGen.Repository.Repositories
{
    public class RepositoryBase<TCollection> : IRepositoryBase<TCollection> where TCollection : MongoDbBase
    {
        public MongoDbContext<TCollection> Context { get; private set; }

        public RepositoryBase()
        {
            Context = new MongoDbContext<TCollection>();
        }

        public TCollection Create(TCollection obj)
        {
            Context.Collection.InsertOne(obj);
            return GetById(obj.guidString);
        }

        public TCollection Delete(string guidString)
        {
            var result = Context.Collection.DeleteOne(x => x.guidString.Equals(guidString), null);
            return GetById(guidString);
        }

        public IQueryable<TCollection> GetAll()
        {
            return Context.Collection.AsQueryable<TCollection>();
        }

        public TCollection GetById(string guidString)
        {
            var result = GetAll().Where(_ => _.guidString == guidString);
            return (result.Count() > 0) ? result.FirstOrDefault() : null;
        }

        public TCollection Update(TCollection obj)
        {
            var result = Context.Collection.ReplaceOne(x => x.guidString.Equals(obj.guidString), obj);
            return GetById(obj.guidString);
        }
    }
}
