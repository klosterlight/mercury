using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Mercury.Common;
using Mercury.Reservations.Service.Dtos;
using Mercury.Reservations.Service.Entities;
using Mercury.Reservations.Tests.Fixtures;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Xunit;

namespace Mercury.Reservations.Tests
{
    public class RoomTests : BaseTests
    {
        public RoomTests(WebApplicationFactory<Reservations.Service.Startup> factory, DbFixture dbFixture) : base(factory, dbFixture)
        {
        }

        [Fact]
        public async Task GetRooms()
        {
            var room = new Room()
            {
                Title = "Asdf",
                NumberOfTickets = 500
            };

            await _dbFixture.CreateRoom(room);
            
            var response = await _httpClient.GetAsync("/rooms");
            response.EnsureSuccessStatusCode();

            var stringResponse = await response.Content.ReadAsStringAsync();
            var rooms = JsonConvert.DeserializeObject<List<RoomDto>>(stringResponse);

            Assert.Equal(1, rooms.Count);
            Assert.Equal(room.Id, rooms[0].Id);
        }
    }
}