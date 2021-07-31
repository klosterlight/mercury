using System;

namespace Mercury.Common
{
    public interface IEntity
    {
        Guid Id { get; set; }
        DateTimeOffset CreatedAt { get; set; }
        DateTimeOffset UpdatedAt { get; set; }
    }
}