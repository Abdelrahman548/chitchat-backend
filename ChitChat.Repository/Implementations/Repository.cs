using ChitChat.Data.Entities.Abstracts;
using ChitChat.Repository.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Linq.Expressions;


namespace ChitChat.Repository.Implementations
{
    public class Repository<T> : IRepository<T>
        where T : Entity
    {
        private readonly IMongoCollection<T> _collection;
        
        public Repository(IMongoDatabase mongoDB, string collectionName)
        {
            _collection = mongoDB.GetCollection<T>(collectionName);
        }

        public async Task AddAsync(T entity)
            => await _collection.InsertOneAsync(entity);

        public async Task DeleteAsync(ObjectId id)
            => await _collection.DeleteOneAsync(e => e.Id == id);

        public async Task<List<T>> FindAsync(Expression<Func<T, bool>> filter)
            => await _collection.Find(filter).ToListAsync();
        public async Task<T> GetByIdAsync(ObjectId id)
            => await _collection.Find(e => e.Id == id).FirstOrDefaultAsync();

        public async Task UpdateAsync(ObjectId id, T entity)
            => await _collection.ReplaceOneAsync(e => e.Id == id, entity);
        
        public async Task<List<T>> GetAllAsync(int page, int pageSize, string? search, bool sortDescending = true)
        {
            var filter = Builders<T>.Filter.Empty;

            if (typeof(SearchableEntity).IsAssignableFrom(typeof(T)) && !string.IsNullOrEmpty(search))
            {
                filter = Builders<T>.Filter.Regex("storedSearchable", new BsonRegularExpression(search, "i"));
            }

            var sort = sortDescending ? Builders<T>.Sort.Descending(e => e.CreatedAt) : Builders<T>.Sort.Ascending(e => e.CreatedAt);

            return await _collection.Find(filter)
                .Sort(sort)
                .Skip((page - 1) * pageSize)
                .Limit(pageSize)
                .ToListAsync();
        }

    }
}
