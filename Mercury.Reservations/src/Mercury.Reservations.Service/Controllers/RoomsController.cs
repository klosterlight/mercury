using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mercury.Common;
using Mercury.Reservations.Hubs;
using Mercury.Reservations.Service.Business;
using Mercury.Reservations.Service.Dtos;
using Mercury.Reservations.Service.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Mercury.Reservations.Service.Controllers
{
    [ApiController]
    [Route("rooms")]
    public class RoomsController : ControllerBase
    {
        private readonly RoomsComponent roomsComponent;
        private readonly IHubContext<RoomHub> _hub;

        public RoomsController(IRepository<Room> repository, IHubContext<RoomHub> hub)
        {
            this.roomsComponent = new RoomsComponent(repository);
            _hub = hub;
        }

        // GET /rooms
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RoomDto>>> GetAsync(string active = "")
        {
            var rooms = (await roomsComponent.GetAllAsync(active))
                        .Select(room => room.AsDto());

            await _hub.Clients.All.SendAsync("newMessage", "anonymous", "Hello World");
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

            room = await roomsComponent.CreateAsync(room);

            if(room.IsValid)
            {
                return CreatedAtAction(nameof(GetByIdAsync), new { id = room.Id }, room.AsDto());
            }
            else
            {
                return UnprocessableEntity(new ErrorResponse(422, room));
            }
        }
    }
}