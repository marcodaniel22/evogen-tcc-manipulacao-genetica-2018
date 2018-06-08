using EvoGen.Repository.Collection;
using EvoGen.Repository.Context;
using EvoGen.Repository.Interfaces.Repositories;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
            return GetById(obj.controlId);
        }

        public long Delete(string guidString)
        {
            var result = Context.Collection.DeleteOne(x => x.guidString.Equals(guidString), null);
            return result.DeletedCount;
        }

        public IQueryable<TCollection> GetAll()
        {
            return Context.Collection.AsQueryable<TCollection>();
        }

        public TCollection GetById(long controlId)
        {
            var result = GetAll().Where(_ => _.controlId == controlId).ToList();
            return (result.Count > 0) ? result.First() : null;
        }

        public long Update(TCollection obj)
        {
            var result = Context.Collection.ReplaceOne(x => x.guidString.Equals(obj.guidString), obj);
            return result.ModifiedCount;
        }
    }
}
