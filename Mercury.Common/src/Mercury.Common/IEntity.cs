using System;
using System.Collections.Generic;

namespace Mercury.Common
{
    public interface IEntity
    {
        Guid Id { get; set; }
        DateTimeOffset CreatedAt { get; set; }
        DateTimeOffset UpdatedAt { get; set; }

        bool IsValid { get; set; }

        Dictionary<string, object[]> Errors { get; set; }
    }
}