using Mercury.Reservations.Service.Entities;

namespace Mercury.Reservations.Service.Dtos
{
    public static class Extensions
    {
        public static RoomDto AsDto(this Room room)
        {
            return new RoomDto(room.Id, room.Title, room.Description, room.NumberOfTickets, room.CreatedAt, room.UpdatedAt);
        }
    }
}