using System.Linq;
using Mercury.Reservations.Service.Entities;

namespace Mercury.Reservations.Service.Dtos
{
    public static class Extensions
    {
        public static RoomDto AsDto(this Room room)
        {
            return new RoomDto(room.Id, room.Title, room.Description, room.NumberOfTickets, room.CreatedAt, room.UpdatedAt, room.Tickets.Select(x => x.AsDto()));
        }

        public static TicketDto AsDto(this Ticket ticket)
        {
            return new TicketDto(ticket.Id, ticket.Folio.ToString().PadLeft(3, '0'), ticket.Status.ToString(), ticket.Price);
        }
    }
}