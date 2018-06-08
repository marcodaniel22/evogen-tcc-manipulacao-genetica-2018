using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace EvoGen.Repository.Collection
{
    public class MongoDbBase
    {
        [BsonId]
        public ObjectId id { get; set; }

        public long controlId { get; set; }

        public string guidString { get; set; }
    }
}
