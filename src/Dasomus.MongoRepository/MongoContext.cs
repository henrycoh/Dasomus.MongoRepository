using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dasomus.MongoRepository
{
    public class MongoContext : IMongoContext
    {
        public string ConnectionString { get; set; }
        public MongoUrl Url { get; set; }

        public MongoContext(string connectionString)
        {
            this.ConnectionString = connectionString;
        }

        public MongoContext(MongoUrl url)
        {
            this.Url = url;
        }
    }
}
