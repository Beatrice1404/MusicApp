using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class Artist
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = null!;

    public string Name { get; set; } = null!;
    public List<Album> Albums { get; set; } = null!;
}

public class Album
{
    public string Title { get; set; } = null!;
    public List<Song> Songs { get; set; } = null!;
    public string Description { get; set; } = null!;
}

public class Song
{
    public string Title { get; set; } = null!;
    public string Length { get; set; } = null!;
}
