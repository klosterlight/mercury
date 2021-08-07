using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Mercury.Common;

namespace Mercury.Reservations.Service.Dtos
{
    public record RoomDto(Guid Id, string Title, string Description, int NumberOfTickets, DateTimeOffset CreatedAt, DateTimeOffset UpdatedAt, IEnumerable<TicketDto> Tickets);

    public record TicketDto(Guid Id, string Folio, string Status, decimal Price);

    public record CreateRoomDto(
        [RequireNonDefault] Guid Id,
        [Required] string Title,
        string Description,
        [Range(1, int.MaxValue)] int NumberOfTickets
    );
}