using MongoDB.Bson.Serialization.Attributes;

namespace InLovingMemory.Data.Entity
{
    public class ImageInfo
    {
        [BsonElement("imageName")]
        public string ImageName { get; set; }

        [BsonElement("filePath")]
        public string FilePath { get; set; }

        [BsonElement("fileExt")]
        public string FileExt { get; set; }

        [BsonElement("file")]
        public string File { get; set; } // Base64 string
    }
}
