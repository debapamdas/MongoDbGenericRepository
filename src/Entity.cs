using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MongoDbGenericRepository
{
    public interface IEntity
    {
        string Id {get; set;}
    }

    public abstract class Entity : IEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
    }
}