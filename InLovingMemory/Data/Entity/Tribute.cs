using System.Reflection.Metadata;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace InLovingMemory.Data.Entity
{
        public class MemorialTribute
        {
            [BsonId]
            [BsonRepresentation(BsonType.ObjectId)]
            [BsonElement("_id")]
            public string? Id { get; set; }

            [BsonElement("fullName")]
            public string FullName { get; set; }

            [BsonElement("tribute")]
            public string Tribute { get; set; }

            [BsonElement("images")]
            public List<ImageInfo>? Images { get; set; }

            [BsonElement("dateModified")]
            public DateTime DateModified { get; set; }

            [BsonElement("dateCreated")]
            public DateTime DateCreated { get; set; }
        }
    }
