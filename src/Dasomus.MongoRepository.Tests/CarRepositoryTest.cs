using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dasomus.MongoRepository.Tests
{
    [CollectionName("car_collection")]
    [BsonIgnoreExtraElements]
    public class Car : MongoEntity
    {
        [BsonElement("year")]
        public int Year { get; set; }
        [BsonElement("make")]
        public string Make { get; set; }
        [BsonElement("model")]
        public string Model { get; set; }
    }

    public class CarRepositoryTest
    {
        public void AddCarTest()
        {
            var context = new MongoContext("mongodb://localhost/CarDatabase");
            var repo = new MongoRepository<Car>(context);

            var newCar = new Car
            {
                Year = 1999,
                Make = "Toyota",
                Model = "Camry"
            };

            repo.Add(newCar);
        }
    }
}
