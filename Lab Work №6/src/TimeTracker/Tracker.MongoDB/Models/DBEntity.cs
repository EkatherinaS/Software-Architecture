using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Tracker.MongoDB.DBEntities
{
    [BsonIgnoreExtraElements]
    public class DBEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
    }
}
