using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MusicApp.Models;
using MusicApp.Settings;

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

        public async Task CreateAsync(Artist newArtist) =>
            await _artistsCollection.InsertOneAsync(newArtist);

        public async Task UpdateAsync(string id, Artist updatedArtist) =>
            await _artistsCollection.ReplaceOneAsync(x => x.Id == id, updatedArtist);

        public async Task RemoveAsync(string id) =>
            await _artistsCollection.DeleteOneAsync(x => x.Id == id);
    }
}
