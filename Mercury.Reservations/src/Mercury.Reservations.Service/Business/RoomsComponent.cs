using Mercury.Common.Business;
using Mercury.Reservations.Service.Entities;

namespace Mercury.Reservations.Service.Business
{
    public class RoomsComponent : BaseComponent<Room>
    {
        public RoomsComponent(Common.IRepository<Room> repository) : base(repository)
        { }
    }
}