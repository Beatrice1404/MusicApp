﻿using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using MusicApp.Models;
using MusicApp.Settings;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicApp.Services
{
    public class MongoDBService
    {
        private readonly IMongoCollection<Artist> _artistsCollection;

        public MongoDBService(IOptions<MongoDBSettings> mongoDBSettings)
        {
            var mongoClient = new MongoClient(mongoDBSettings.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(mongoDBSettings.Value.DatabaseName);
            _artistsCollection = mongoDatabase.GetCollection<Artist>("artists");
        }

        public async Task<List<Artist>> GetAsync() =>
            await _artistsCollection.Find(_ => true).ToListAsync();

        public async Task<Artist> GetAsync(string id) =>
            await _artistsCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task<Album> GetAlbumByTitleAsync(string albumTitle)
        {
            var artist = await _artistsCollection.Find(a => a.albums.Any(album => album.title == albumTitle)).FirstOrDefaultAsync();
            return artist?.albums.FirstOrDefault(album => album.title == albumTitle);
        }

        public async Task<Album> GetAlbumByIdAsync(string albumId)
        {
            var artist = await _artistsCollection.Find(a => a.albums.Any(album => album.Id == albumId)).FirstOrDefaultAsync();
            return artist?.albums.FirstOrDefault(album => album.Id == albumId);
        }


        public async Task<List<Artist>> GetArtistsAsync() => await _artistsCollection.Find(_ => true).ToListAsync();

        public async Task<Artist> GetArtistAsync(string id) => await _artistsCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateArtistAsync(Artist artist) => await _artistsCollection.InsertOneAsync(artist);

        public async Task UpdateArtistAsync(string id, Artist artist) => await _artistsCollection.ReplaceOneAsync(x => x.Id == id, artist);

        public async Task DeleteArtistAsync(string id) => await _artistsCollection.DeleteOneAsync(x => x.Id == id);

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
            var update = Builders<Artist>.Update.PullFilter(a => a.albums, ab => ab.Id == albumId);
            await _artistsCollection.UpdateOneAsync(a => a.Id == artistId, update);
        }

        // CRUD for Songs
        public async Task AddSongAsync(string artistId, string albumId, Song song)
        {
            var filter = Builders<Artist>.Filter.And(
                Builders<Artist>.Filter.Eq(a => a.Id, artistId),
                Builders<Artist>.Filter.ElemMatch(a => a.albums, ab => ab.Id == albumId)
            );
            var update = Builders<Artist>.Update.Push("albums.$.songs", song);
            await _artistsCollection.UpdateOneAsync(filter, update);
        }

        public async Task UpdateSongAsync(string artistId, string albumId, Song song)
        {
            var filter = Builders<Artist>.Filter.And(
                Builders<Artist>.Filter.Eq(a => a.Id, artistId),
                Builders<Artist>.Filter.ElemMatch(a => a.albums, ab => ab.Id == albumId),
                Builders<Artist>.Filter.ElemMatch(a => a.albums[-1].songs, s => s.Id == song.Id)
            );

            var update = Builders<Artist>.Update
                .Set("albums.$[a].songs.$[s].title", song.title)
                .Set("albums.$[a].songs.$[s].length", song.length);

            var arrayFilters = new[]
            {
                new BsonDocumentArrayFilterDefinition<BsonDocument>(new BsonDocument("a.Id", albumId)),
                new BsonDocumentArrayFilterDefinition<BsonDocument>(new BsonDocument("s.Id", song.Id))
            };

            var updateOptions = new UpdateOptions { ArrayFilters = arrayFilters };

            await _artistsCollection.UpdateOneAsync(filter, update, updateOptions);
        }

        public async Task DeleteSongAsync(string artistId, string albumId, string songId)
        {
            var filter = Builders<Artist>.Filter.And(
                Builders<Artist>.Filter.Eq(a => a.Id, artistId),
                Builders<Artist>.Filter.ElemMatch(a => a.albums, ab => ab.Id == albumId)
            );

            var update = Builders<Artist>.Update.PullFilter("albums.$.songs", Builders<Song>.Filter.Eq(s => s.Id, songId));

            await _artistsCollection.UpdateOneAsync(filter, update);
        }

        public async Task<Song> GetSongByTitleAsync(string songTitle)
        {
            var artist = await _artistsCollection.Find(a => a.albums.Any(album => album.songs.Any(song => song.title == songTitle))).FirstOrDefaultAsync();
            if (artist == null) return null;

            foreach (var album in artist.albums)
            {
                var song = album.songs.FirstOrDefault(s => s.title == songTitle);
                if (song != null)
                {
                    return song;
                }
            }
            return null;
        }
    }
}
