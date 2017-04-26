using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dasomus.MongoRepository
{
    /// <summary>
    /// Abstract Entity for all the MongoEntities.
    /// </summary>
    [BsonIgnoreExtraElements(Inherited = true)]
    public abstract class MongoEntity<TKey> : IMongoEntity<TKey>
    {
        /// <summary>
        /// Gets or sets the id for this object (the primary record for an entity).
        /// </summary>
        /// <value>The id for this object (the primary record for an entity).</value>
        [BsonRepresentation(BsonType.ObjectId)]
        public TKey Id { get; set; }
    }
}
