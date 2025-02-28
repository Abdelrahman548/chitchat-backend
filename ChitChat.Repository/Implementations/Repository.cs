using ChitChat.Data.Entities.Abstracts;
using ChitChat.Repository.Helpers;
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
        {
            if (entity is SearchableEntity searchable)
                searchable.PrepareSearchable();

            await _collection.InsertOneAsync(entity);
        }

        public async Task DeleteAsync(string id)
            => await _collection.DeleteOneAsync(e => e.Id == id);

        public async Task DeleteAsync(Expression<Func<T, bool>> filter)
            => await _collection.DeleteOneAsync(filter);

        public async Task<List<T>> FindAsync(Expression<Func<T, bool>> filter)
            => await _collection.Find(filter).ToListAsync();
        public async Task<T> GetByIdAsync(string id)
            => await _collection.Find(e => e.Id == id).FirstOrDefaultAsync();

        public async Task UpdateAsync(string id, T entity)
        {
            if (entity is SearchableEntity searchable)
                searchable.PrepareSearchable();

            await _collection.ReplaceOneAsync(e => e.Id == id, entity);
        }
        

        public async Task<PagedList<T>> GetAllAsync(ItemQueryParams queryParams, Expression<Func<T, bool>> filterExpression = null)
        {
            var filters = new List<FilterDefinition<T>>();

            if (filterExpression != null)
                filters.Add(Builders<T>.Filter.Where(filterExpression));

            if (typeof(SearchableEntity).IsAssignableFrom(typeof(T)) && !string.IsNullOrEmpty(queryParams.Search))
                filters.Add(Builders<T>.Filter.Regex("searchable", new BsonRegularExpression(queryParams.Search, "i")));

            var filter = filters.Any() ? Builders<T>.Filter.And(filters) : Builders<T>.Filter.Empty;

            var totalCount = await _collection.CountDocumentsAsync(filter);

            var sort = queryParams.IsDecending
                ? Builders<T>.Sort.Descending(e => e.CreatedAt)
                : Builders<T>.Sort.Ascending(e => e.CreatedAt);

            var items = await _collection.Find(filter)
                .Sort(sort)
                .Skip((queryParams.Page - 1) * queryParams.Limit)
                .Limit(queryParams.Limit)
                .ToListAsync();

            return new PagedList<T>(items, queryParams.Page, queryParams.Limit, (int)totalCount);
        }
    }
}
