using System;
using System.ComponentModel.DataAnnotations;

namespace Mercury.Reservations.Service.Dtos
{
    public record RoomDto(Guid Id, string Title, string Description, int NumberOfTickets, DateTimeOffset CreatedAt, DateTimeOffset UpdatedAt);

    public record CreateRoomDto(string Title, string Description, [Range(1, Double.PositiveInfinity)] int NumberOfTickets);
}