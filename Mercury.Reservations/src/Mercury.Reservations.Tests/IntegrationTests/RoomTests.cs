using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
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

            Assert.Single(rooms);
            Assert.Equal(room.Id, rooms[0].Id);
        }

        [Theory]
        [ClassData(typeof(RoomDataGenerator))]
        public async Task InvalidRoom(Guid id, string title, string description, int numberOfTickets, string errorKey, string errorMessage)
        {
            var room = new CreateRoomDto(id, title, description, numberOfTickets);
            var stringContent = new StringContent(JsonConvert.SerializeObject(room), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("/rooms", stringContent);
            var stringResponse = await response.Content.ReadAsStringAsync();
            var error = JsonConvert.DeserializeObject<ErrorResponse>(stringResponse);
            Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
            var a = error.Errors[errorKey];
            Assert.Contains(errorMessage, a);
        }
    }

    class RoomDataGenerator : IEnumerable<object[]>
    {
        private readonly List<object[]> _data = new List<object[]>
        {
            new object[] { Guid.NewGuid(), "asdf", "", 0, "NumberOfTickets", "The field NumberOfTickets must be between 1 and 2147483647."},
            new object[] { Guid.NewGuid(), "", "", 1, "Title", "The Title field is required."},
            new object[] { Guid.Empty, "asdf", "", 1, "Id", "The Id field requires a non-default value."}
        };

        public IEnumerator<object[]> GetEnumerator() => _data.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}