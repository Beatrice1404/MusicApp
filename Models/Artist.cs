using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace MusicApp.Models
{
    public class Artist
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }  // Proprietatea Id

        public string Name { get; set; }
        public List<Album> Albums { get; set; }
    }

    public class Album
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }  // Proprietatea Id

        public string Title { get; set; }
        public string Description { get; set; }
        public List<Song> Songs { get; set; }
    }

    public class Song
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }  // Proprietatea Id

        public string Title { get; set; }
        public string Length { get; set; }
    }
}
