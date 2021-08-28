using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Mercury.Common;
using Mercury.Reservations.Service.Dtos;
using Mercury.Reservations.Service.Entities;
using Mercury.Reservations.Tests.Fixtures;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Xunit;

namespace Mercury.Reservations.Tests
{
    public class RoomTests : BaseTests
    {
        public RoomTests(WebApplicationFactory<Reservations.Service.Startup> factory, DbFixture dbFixture)
            : base(factory, dbFixture)
        {
        }

        [Fact]
        public async Task GetRooms()
        {
            using (var scope = _factory.Services.CreateScope())
            {
                _dbFixture.Dispose();
                var roomRepository = scope.ServiceProvider.GetService<IRepository<Room>>();
                var roomFixture = new RoomFixture(roomRepository);
                var room = await roomFixture.CreateValidRoom();

                var response = await _httpClient.GetAsync("/rooms");
                response.EnsureSuccessStatusCode();

                var stringResponse = await response.Content.ReadAsStringAsync();
                var rooms = JsonConvert.DeserializeObject<List<RoomDto>>(stringResponse);

                Assert.Single(rooms);
                Assert.Equal(room.Id, rooms[0].Id);
            }
        }

        [Fact]
        public async Task GetActiveRooms()
        {
            using (var scope = _factory.Services.CreateScope())
            {
                var roomRepository = scope.ServiceProvider.GetService<IRepository<Room>>();
                var roomFixture = new RoomFixture(roomRepository);
                var numberOfRooms = 10;
                var validRooms = await roomFixture.CreateMultipleValidRooms(numberOfRooms);
                var invalidRoom0 = await roomFixture.CreateExpiredRoom();
                var invalidRoom1 = await roomFixture.CreateExpiredRoom();
                var invalidRoom2 = await roomFixture.CreateExpiredRoom();

                var response =  await _httpClient.GetAsync("/rooms?active=true");
                var stringResponse = await response.Content.ReadAsStringAsync();
                var rooms = JsonConvert.DeserializeObject<List<RoomDto>>(stringResponse);

                Assert.Equal(numberOfRooms, rooms.Count());
            }
        }

        [Theory]
        [ClassData(typeof(RoomDataGenerator))]
        public async Task InvalidRoom(Guid id, string title, string description, int numberOfTickets, DateTimeOffset expiresAt, string errorKey, string errorMessage)
        {
            var room = new CreateRoomDto(id, title, description, numberOfTickets, expiresAt);
            var stringContent = new StringContent(JsonConvert.SerializeObject(room), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("/rooms", stringContent);
            var stringResponse = await response.Content.ReadAsStringAsync();
            var error = JsonConvert.DeserializeObject<ErrorResponse>(stringResponse);
            Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Contains(errorMessage, error.Errors[errorKey]);
        }
        
        [Fact]
        public async Task CreatingDuplicatedRoom()
        {
            using (var scope = _factory.Services.CreateScope())
            {
                var roomRepository = scope.ServiceProvider.GetService<IRepository<Room>>();
                var roomDto = new CreateRoomDto(Guid.NewGuid(), "Title", "", 10, DateTimeOffset.Now.AddDays(1));
                var room = new Room(roomDto);
                var stringContent = new StringContent(JsonConvert.SerializeObject(room), Encoding.UTF8, "application/json");
                await roomRepository.CreateAsync(room);

                var response = await _httpClient.PostAsync("/rooms", stringContent);
                var stringResponse = await response.Content.ReadAsStringAsync();
                var error = JsonConvert.DeserializeObject<ErrorResponse>(stringResponse);
                string errorMessage = "The Field Id is repeated";
                string errorMessage0 = "The Field Title is repeated";
                Assert.Equal(System.Net.HttpStatusCode.UnprocessableEntity, response.StatusCode);
                Assert.Contains(errorMessage, error.Errors["Id"]);
                Assert.Contains(errorMessage0, error.Errors["Title"]);
            }
        }

        [Fact]
        public async Task CreateRoom()
        {
            _dbFixture.Dispose();
            var currentDateTime = DateTimeOffset.Now.AddDays(7);
            var room = new CreateRoomDto(Guid.NewGuid(), "Title", "Description", 500, currentDateTime);
            var stringContent = new StringContent(JsonConvert.SerializeObject(room), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("/rooms", stringContent);
            var stringResponse = await response.Content.ReadAsStringAsync();
            var responseObject = JsonConvert.DeserializeObject<RoomDto>(stringResponse);
            var tickets = responseObject.Tickets.ToList();

            Assert.Equal(System.Net.HttpStatusCode.Created, response.StatusCode);
            Assert.Equal(room.NumberOfTickets, tickets.Count());
            Assert.Equal("001", tickets[0].Folio);
            Assert.Equal(500, tickets[0].Price);
            Assert.Equal(Ticket.Statuses.Available.ToString(), tickets[0].Status);
            Assert.Equal(currentDateTime, responseObject.ExpiresAt);
        }
    }

    class RoomDataGenerator : IEnumerable<object[]>
    {
        private readonly List<object[]> _data = new List<object[]>
        {
            new object[] { Guid.NewGuid(), "asdf", "", 0, DateTimeOffset.Now, "NumberOfTickets", "The field NumberOfTickets must be between 1 and 2147483647."},
            new object[] { Guid.NewGuid(), "", "", 1, DateTimeOffset.Now, "Title", "The Title field is required."},
            new object[] { Guid.Empty, "asdf", "", 1, DateTimeOffset.Now, "Id", "The Id field requires a non-default value."},
            new object[] { Guid.NewGuid(), "asdf", "", 1, DateTimeOffset.Now.AddDays(-1), "ExpiresAt", $"ExpiresAt must be greater than {DateTimeOffset.Now.ToString("yyyy-MM-dd")}."},
        };

        public IEnumerator<object[]> GetEnumerator() => _data.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}