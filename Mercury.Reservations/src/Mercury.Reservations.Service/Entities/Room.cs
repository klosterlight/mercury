using System;
using System.Collections.Generic;
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
            IsValid = true;
            Errors = new List<string>();
        }
        public Room(CreateRoomDto dto)
        {
            Id = dto.Id;
            Title = dto.Title;
            Description = dto.Description;
            NumberOfTickets = dto.NumberOfTickets;
            CreatedAt = DateTimeOffset.UtcNow;
            UpdatedAt = DateTimeOffset.UtcNow;
            Tickets = new List<Ticket>();
            for(var i = 0; i < NumberOfTickets; i++)
            {
                var ticket = new Ticket();
                ticket.Folio = i + 1;
                Tickets.Add(ticket);
            }
            IsValid = true;
            Errors = new List<string>();
        }
        [RequireNonDefault]
        public Guid Id { get; set; }
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        [Range(1, int.MaxValue)]
        public int NumberOfTickets { get; set; }
        public List<Ticket> Tickets { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
        public bool IsValid { get; set; }
        public List<string> Errors { get; set; }
    }
}