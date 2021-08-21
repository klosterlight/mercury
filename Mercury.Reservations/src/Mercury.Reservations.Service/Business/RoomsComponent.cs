using System.Threading.Tasks;
using Mercury.Common;
using Mercury.Common.Business;
using Mercury.Reservations.Service.Entities;

namespace Mercury.Reservations.Service.Business
{
    public class RoomsComponent : BaseComponent<Room>
    {
        private readonly IRepository<Room> _repository;

        public RoomsComponent(IRepository<Room> repository) : base(repository)
        {
            _repository = repository;
        }

        public override async Task<Room> CreateAsync(Room entity)
        {
            // Unique Title and Unique Id
            var repeatedRoom = await _repository.GetAsync(room => room.Title == entity.Title || room.Id == entity.Id);
            if(repeatedRoom != null)
            {
                entity.IsValid = false;
                entity.Errors.Add("Repeated room");
                return entity;
            }
            await base.CreateAsync(entity);
            return entity;
        }
    }
}