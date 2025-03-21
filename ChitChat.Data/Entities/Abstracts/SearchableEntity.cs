﻿using MongoDB.Bson.Serialization.Attributes;

namespace ChitChat.Data.Entities.Abstracts
{
    public abstract class SearchableEntity : Entity
    {
        [BsonElement("searchable")]
        public string StoredSearchable { get; set; } = string.Empty;

        public virtual void PrepareSearchable() { }
    }
}
