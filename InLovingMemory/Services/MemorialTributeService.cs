using InLovingMemory.Data.Entity;
using InLovingMemory.Model;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace InLovingMemory.Services
{
 
public class MemorialTributeService
    {
        private readonly IMongoCollection<MemorialTribute> _collection;

        public MemorialTributeService(IMongoDatabase db, IOptions<MongoDbSettings> settings)
        {
            var mongoSettings = settings.Value;
            _collection = db.GetCollection<MemorialTribute>(mongoSettings.CollectionName);
        }

        public async Task<List<MemorialTribute>> GetAll() =>
            await _collection.Find(_ => true).ToListAsync();

        public async Task<MemorialTribute> GetById(string id) =>
            await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task Create(MemorialTribute tribute) =>
            await _collection.InsertOneAsync(tribute);

        public async Task Update(MemorialTribute tribute) =>
            await _collection.ReplaceOneAsync(x => x.Id == tribute.Id, tribute);
    }

}

