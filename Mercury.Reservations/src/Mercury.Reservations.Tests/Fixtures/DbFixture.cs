using System;
using System.Threading.Tasks;
using Mercury.Common;
using Mercury.Common.Settings;
using Mercury.Reservations.Service.Entities;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Mercury.Reservations.Tests.Fixtures
{
    public class DbFixture : IDisposable
    {
        protected IMongoDatabase db;
        private readonly string _roomsCollection = "rooms";

        public DbFixture()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
            var serviceSettings = config.GetSection(nameof(ServiceSettings)).Get<ServiceSettings>();
            var mongoDbSettings = config.GetSection(nameof(MongoDbSettings)).Get<MongoDbSettings>();
            var mongoClient = new MongoClient(mongoDbSettings.ConnectionString);
            db = mongoClient.GetDatabase(serviceSettings.ServiceName);
        }

        public async Task<Room> CreateRoom(Room entity)
        {
            await db.GetCollection<Room>(_roomsCollection).InsertOneAsync(entity);

            return entity;
        }

        public void Dispose()
        {
            db.DropCollection(_roomsCollection);
        }
    }
}