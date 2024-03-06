using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TrackerRepository
{
    [BsonIgnoreExtraElements]
    public class DBEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
    }
}
