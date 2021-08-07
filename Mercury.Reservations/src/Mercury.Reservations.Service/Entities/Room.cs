using System;
using Mercury.Common;
using Mercury.Reservations.Service.Dtos;

namespace Mercury.Reservations.Service.Entities
{
    public class Room : IEntity
    {
        public Room()
        {
            Id = Guid.NewGuid();
            CreatedAt = DateTimeOffset.UtcNow;
            UpdatedAt = DateTimeOffset.UtcNow;
        }
        public Room(CreateRoomDto dto)
        {
            Id = Guid.NewGuid();
            Title = dto.Title;
            Description = dto.Description;
            NumberOfTickets = dto.NumberOfTickets;
            CreatedAt = DateTimeOffset.UtcNow;
            UpdatedAt = DateTimeOffset.UtcNow;
        }
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int NumberOfTickets { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
    }
}