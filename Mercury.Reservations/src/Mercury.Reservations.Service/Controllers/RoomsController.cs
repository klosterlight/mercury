using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly IBaseComponent<Room> roomsComponent;

        public RoomsController(IBaseComponent<Room> roomsComponent)
        {
            this.roomsComponent = roomsComponent;
        }

        // GET /rooms
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RoomDto>>> GetAsync()
        {
            var rooms = (await roomsComponent.GetAllAsync())
                        .Select(room => room.AsDto());
            return Ok(rooms);
        }

        // GET /rooms/:id
        [HttpGet("id")]
        public async Task<ActionResult<RoomDto>> GetByIdAsync(Guid Id)
        {
            var room = await roomsComponent.GetAsync(Id);

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

            await roomsComponent.CreateAsync(room);

            return CreatedAtAction(nameof(GetByIdAsync), new { id = room.Id }, room.AsDto());
        }
    }
}