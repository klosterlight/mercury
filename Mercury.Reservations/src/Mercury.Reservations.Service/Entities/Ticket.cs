using System;
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
        }
        public Guid Id { get; set; }
        public int Folio { get; set; }
        public Statuses Status { get; set; }
        public decimal Price { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
    }
}