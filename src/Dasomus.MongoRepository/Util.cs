using MongoDB.Driver;
using ReflectionBridge.Extensions;
using System;
using System.Reflection;

namespace Dasomus.MongoRepository
{
    internal static class Util<U>
    {
        private static IMongoDatabase GetDatabaseFromUrl(MongoUrl url)
        {
            var client = new MongoClient(url);
            return client.GetDatabase(url.DatabaseName);
        }

        public static IMongoCollection<T> GetCollectionFromConnectionString<T>(string connectionString)
            where T : IMongoEntity<U>
        {
            return Util<U>.GetCollectionFromConnectionString<T>(connectionString, GetCollectionName<T>());
        }

        public static IMongoCollection<T> GetCollectionFromConnectionString<T>(string connectionString, string collectionName)
            where T : IMongoEntity<U>
        {
            return Util<U>.GetDatabaseFromUrl(new MongoUrl(connectionString))
                .GetCollection<T>(collectionName);
        }

        public static IMongoCollection<T> GetCollectionFromUrl<T>(MongoUrl url)
            where T : IMongoEntity<U>
        {
            return Util<U>.GetCollectionFromUrl<T>(url, GetCollectionName<T>());
        }

        public static IMongoCollection<T> GetCollectionFromUrl<T>(MongoUrl url, string collectionName)
            where T : IMongoEntity<U>
        {
            return Util<U>.GetDatabaseFromUrl(url)
                .GetCollection<T>(collectionName);
        }

        private static string GetCollectionName<T>() where T : IMongoEntity<U>
        {
            string collectionName;
            if (typeof(T).BaseType().Equals(typeof(object)))
            {
                collectionName = GetCollectioNameFromInterface<T>();
            }
            else
            {
                collectionName = GetCollectionNameFromType(typeof(T));
            }

            if (string.IsNullOrEmpty(collectionName))
            {
                throw new ArgumentException("Collection name cannot be empty for this entity");
            }
            return collectionName;
        }

        private static string GetCollectioNameFromInterface<T>()
        {
            string collectionname;
            
            var att = typeof(T).GetTypeInfo().GetCustomAttribute(typeof(CollectionName));
            if (att != null)
            {
                collectionname = ((CollectionName)att).Name;
            }
            else
            {
                collectionname = typeof(T).Name;
            }

            return collectionname;
        }

        private static string GetCollectionNameFromType(Type entitytype)
        {
            string collectionname;

            var att = entitytype.GetTypeInfo().GetCustomAttribute(typeof(CollectionName));
            if (att != null)
            {
                collectionname = ((CollectionName)att).Name;
            }
            else
            {
                if (typeof(MongoEntity).IsAssignableFrom(entitytype))
                {
                    while (!entitytype.BaseType().Equals(typeof(MongoEntity)))
                    {
                        entitytype = entitytype.BaseType();
                    }
                }
                collectionname = entitytype.Name;
            }

            return collectionname;
        }
    }
}
