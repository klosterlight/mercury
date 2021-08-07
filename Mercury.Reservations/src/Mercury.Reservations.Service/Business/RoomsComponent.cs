using System.Threading.Tasks;
using Mercury.Common;
using Mercury.Common.Business;
using Mercury.Reservations.Service.Entities;

namespace Mercury.Reservations.Service.Business
{
    public class RoomsComponent : BaseComponent<Room>
    {
        public RoomsComponent(IRepository<Room> repository) : base(repository)
        { }
    }
}