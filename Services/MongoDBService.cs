using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MusicApp.Models;
using MusicApp.Settings;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicApp.Services
{
    public class MongoDBService
    {
        private readonly IMongoCollection<Artist> _artistsCollection;
        private readonly IMongoCollection<Song> _songsCollection;
        private readonly IMongoCollection<Album> _albumsCollection;

        public MongoDBService(IOptions<MongoDBSettings> mongoDBSettings)
        {
            var mongoClient = new MongoClient(mongoDBSettings.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(mongoDBSettings.Value.DatabaseName);
            _artistsCollection = mongoDatabase.GetCollection<Artist>("artists");
            _songsCollection = mongoDatabase.GetCollection<Song>("songs");
            _albumsCollection = mongoDatabase.GetCollection<Album>("albums");
        }

        public async Task<List<Artist>> GetAsync() =>
            await _artistsCollection.Find(_ => true).ToListAsync();

        public async Task<Artist> GetArtistAsync(string id) =>
            await _artistsCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateArtistAsync(Artist artist) =>
            await _artistsCollection.InsertOneAsync(artist);

        public async Task UpdateArtistAsync(string id, Artist artist) =>
            await _artistsCollection.ReplaceOneAsync(x => x.Id == id, artist);

        public async Task DeleteArtistAsync(string id) =>
            await _artistsCollection.DeleteOneAsync(x => x.Id == id);

        public async Task<Album> GetAlbumByIdAsync(string albumId)
        {
            var artist = await _artistsCollection.Find(a => a.albums.Any(album => album.Id == albumId)).FirstOrDefaultAsync();
            return artist?.albums.FirstOrDefault(album => album.Id == albumId);
        }

        public async Task AddAlbumAsync(string artistId, Album album)
        {
            var update = Builders<Artist>.Update.Push(a => a.albums, album);
            await _artistsCollection.UpdateOneAsync(a => a.Id == artistId, update);
        }

        public async Task UpdateAlbumAsync(string albumId, Album album)
        {
            var filter = Builders<Artist>.Filter.ElemMatch(a => a.albums, ab => ab.Id == albumId);
            var update = Builders<Artist>.Update.Set("albums.$", album);
            await _artistsCollection.UpdateOneAsync(filter, update);
        }

        public async Task DeleteAlbumAsync(string artistId, string albumId)
        {
            var filter = Builders<Artist>.Filter.And(
                Builders<Artist>.Filter.Eq(a => a.Id, artistId),
                Builders<Artist>.Filter.ElemMatch(a => a.albums, ab => ab.Id == albumId)
            );
            var update = Builders<Artist>.Update.PullFilter(a => a.albums, ab => ab.Id == albumId);
            await _artistsCollection.UpdateOneAsync(filter, update);
        }

        public async Task<Artist> GetArtistByAlbumTitleAsync(string albumTitle)
        {
            return await _artistsCollection.Find(a => a.albums.Any(album => album.title == albumTitle)).FirstOrDefaultAsync();
        }

        public async Task<List<ISearchableItem>> SearchAsync(string term)
        {
            var artistResults = await _artistsCollection.Find(a => a.name.Contains(term)).ToListAsync();
            var songResults = await _songsCollection.Find(s => s.title.Contains(term)).ToListAsync();
            var albumResults = await _albumsCollection.Find(a => a.title.Contains(term)).ToListAsync();

            var searchResults = new List<ISearchableItem>();

            searchResults.AddRange(artistResults.Select(a => new ArtistSearchResult { Artist = a }));
            searchResults.AddRange(songResults.Select(s => new SongSearchResult { Song = s }));
            searchResults.AddRange(albumResults.Select(a => new AlbumSearchResult { Album = a }));

            return searchResults;
        }
    }
}
