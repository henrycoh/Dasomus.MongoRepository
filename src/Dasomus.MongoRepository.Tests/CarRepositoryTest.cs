using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using MongoDB.Driver.Linq;
using System.Threading.Tasks;

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
        [Fact]
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

        [Fact]
        public async Task GetCarTest()
        {
            var context = new MongoContext("mongodb://localhost/CarDatabase");
            var repo = new MongoRepository<Car>(context);

            var test = await repo.Where(x => x.Year == 1999).FirstAsync();
        }
    }
}
