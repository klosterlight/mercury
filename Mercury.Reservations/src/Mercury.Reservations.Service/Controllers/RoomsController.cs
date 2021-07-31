using System;
using System.Threading.Tasks;
using Mercury.Common;
using Mercury.Reservations.Service.Dtos;
using Mercury.Reservations.Service.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Mercury.Reservations.Service.Controllers
{
    [ApiController]
    [Route("rooms")]
    public class RoomsController : ControllerBase
    {
        private readonly IRepository<Room> roomsRepository;

        public RoomsController(IRepository<Room> roomsRepository)
        {
            this.roomsRepository = roomsRepository;
        }

        // GET /rooms/:id
        [HttpGet]
        public async Task<ActionResult<RoomDto>> GetByIdAsync(Guid Id)
        {
            var room = await roomsRepository.GetAsync(Id);

            if (room == null)
            {
                return NotFound();
            }

            return room.AsDto();
        }

        // POST /rooms
        [HttpPost]
        public async Task<ActionResult<RoomDto>> CreateAsync(CreateRoomDto roomDto)
        {
            var room = new Room(roomDto);

            await roomsRepository.CreateAsync(room);

            return CreatedAtAction(nameof(GetByIdAsync), new { id = room.Id }, room);
        }
    }
}