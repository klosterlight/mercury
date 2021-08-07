using Mercury.Common;
using Mercury.Reservations.Service;
using Mercury.Reservations.Tests.Fixtures;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http;
using Xunit;

namespace Mercury.Reservations.Tests
{
    public class BaseTests : IClassFixture<WebApplicationFactory<Reservations.Service.Startup>>, IClassFixture<DbFixture>
    {
        protected readonly HttpClient _httpClient;
        protected readonly DbFixture _dbFixture;

        public BaseTests(WebApplicationFactory<Reservations.Service.Startup> factory, DbFixture dbFixture)
        {
            _httpClient = factory.CreateClient();
            _dbFixture = dbFixture;
        }
    }
}