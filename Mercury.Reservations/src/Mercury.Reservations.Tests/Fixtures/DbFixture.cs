using System;
using System.Threading.Tasks;
using Mercury.Common;
using Mercury.Common.Settings;
using Mercury.Reservations.Service.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace Mercury.Reservations.Tests.Fixtures
{
    public class DbFixture : IDisposable
    {
        protected IMongoDatabase _db;
        private readonly string _roomsCollection = "rooms";

        public ServiceProvider _serviceProvider { get; set; }

        public DbFixture()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
            var serviceSettings = config.GetSection(nameof(ServiceSettings)).Get<ServiceSettings>();
            var mongoDbSettings = config.GetSection(nameof(MongoDbSettings)).Get<MongoDbSettings>();
            var mongoClient = new MongoClient(mongoDbSettings.ConnectionString);

            _db = mongoClient.GetDatabase(serviceSettings.ServiceName);
        }

        public void Dispose()
        {
            _db.DropCollection(_roomsCollection);
        }
    }
}