namespace MusicApp.Models
{
    public interface ISearchableItem
    {
        string Type { get; }
    }

    public class ArtistSearchResult : ISearchableItem
    {
        public string Type => "Artist";
        public Artist Artist { get; set; }
    }

    public class SongSearchResult : ISearchableItem
    {
        public string Type => "Song";
        public Song Song { get; set; }
    }

    public class AlbumSearchResult : ISearchableItem
    {
        public string Type => "Album";
        public Album Album { get; set; }
    }
}

