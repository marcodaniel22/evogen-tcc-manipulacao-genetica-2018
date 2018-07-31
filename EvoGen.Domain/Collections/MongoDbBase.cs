using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace EvoGen.Domain.Collections
{
    public class MongoDbBase
    {
        [BsonId]
        public ObjectId id { get; set; }

        public string guidString { get; set; }
    }
}
