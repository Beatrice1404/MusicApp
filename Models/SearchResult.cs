namespace MusicApp.Models
{
    public interface ISearchableItem { }

    public class ArtistSearchResult : ISearchableItem
    {
        public Artist Artist { get; set; }
    }

    public class SongSearchResult : ISearchableItem
    {
        public Song Song { get; set; }
    }

    public class AlbumSearchResult : ISearchableItem
    {
        public Album Album { get; set; }
    }
}
