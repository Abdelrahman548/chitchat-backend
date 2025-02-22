using MongoDB.Bson.Serialization.Attributes;

namespace ChitChat.Data.Entities.Abstracts
{
    public abstract class SearchableEntity : Entity
    {
        [BsonElement("searchable")]
        public string StoredSearchable { get; set; } = string.Empty;
        [BsonIgnore]
        public virtual string Searchable => StoredSearchable;
    }
}
