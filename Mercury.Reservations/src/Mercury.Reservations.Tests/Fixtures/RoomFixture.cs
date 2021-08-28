using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mercury.Common;
using Mercury.Reservations.Service.Dtos;
using Mercury.Reservations.Service.Entities;

namespace Mercury.Reservations.Tests
{
    public class RoomFixture : IDisposable
    {
        private IRepository<Room> _repository;
        public RoomFixture(IRepository<Room> repository)
        {
            _repository = repository;
        }

        public async Task<Room> CreateValidRoom()
        {
            var roomDto = new CreateRoomDto(
                Guid.NewGuid(), Faker.Name.First(), Faker.Name.FullName(), 10, DateTimeOffset.Now.AddDays(1)
            );
            var room = new Room(roomDto);

            return await _repository.CreateAsync(room);
        }

        public async Task<Room> CreateExpiredRoom()
        {
            var room = new Room()
            {
                Id = Guid.NewGuid(),
                Title = Faker.Name.First(),
                Description = Faker.Name.FullName(),
                NumberOfTickets = 10,
                ExpiresAt = DateTimeOffset.Now.AddDays(-1)
            };

            return await _repository.CreateAsync(room);
        }

        public async Task<List<Room>> CreateMultipleValidRooms(int numberOfRooms)
        {
            List<Room> rooms = new List<Room>();
            for(var i = 0; i < numberOfRooms; i++)
            {
                var room = await CreateValidRoom();
                rooms.Add(room);
            }

            return rooms;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}