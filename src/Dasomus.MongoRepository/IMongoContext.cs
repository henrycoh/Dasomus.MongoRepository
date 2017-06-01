using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dasomus.MongoRepository
{
    public interface IMongoContext
    {
        string ConnectionString { get; set; }
        MongoUrl Url { get; set; }
    }
}
