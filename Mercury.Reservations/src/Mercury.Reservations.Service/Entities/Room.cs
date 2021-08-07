using System;
using System.ComponentModel.DataAnnotations;
using Mercury.Common;
using Mercury.Reservations.Service.Dtos;

namespace Mercury.Reservations.Service.Entities
{
    public class Room : IEntity
    {
        public Room()
        {
            CreatedAt = DateTimeOffset.UtcNow;
            UpdatedAt = DateTimeOffset.UtcNow;
        }
        public Room(CreateRoomDto dto)
        {
            Title = dto.Title;
            Description = dto.Description;
            NumberOfTickets = dto.NumberOfTickets;
            CreatedAt = DateTimeOffset.UtcNow;
            UpdatedAt = DateTimeOffset.UtcNow;
        }
        [RequireNonDefault]
        public Guid Id { get; set; }
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        [Range(1, int.MaxValue)]
        public int NumberOfTickets { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
    }
}