using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace MusicApp.Models
{
    public class Artist
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string name { get; set; }

        [BsonElement("albums")]
        public List<Album> albums { get; set; }
    }

    public class Album
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string title { get; set; }

        public string description { get; set; }

        [BsonElement("songs")]
        public List<Song> songs { get; set; } = new List<Song>(); // Inițializează lista de cântece
    }


    public class Song
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string title { get; set; }

        public string length { get; set; }
    }
}
