using System;
using System.Collections.Generic;
using Mercury.Common;

namespace Mercury.Reservations.Service.Entities
{
    public class Ticket : IEntity
    {
        public enum Statuses
        {
            Sold,
            Available,
            Reserved
        }
        public Ticket()
        {
            Id = Guid.NewGuid();
            CreatedAt = DateTimeOffset.UtcNow;
            UpdatedAt = DateTimeOffset.UtcNow;
            Status = Statuses.Available;
            Price = 500;
            Errors = new Dictionary<string, object[]>();
        }
        public Guid Id { get; set; }
        public int Folio { get; set; }
        public Statuses Status { get; set; }
        public decimal Price { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
        public bool IsValid { get; set; }
        public Dictionary<string, object[]> Errors { get; set; }
    }
}