using System;
using System.Collections.Generic;
using System.Linq.Expressions;
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

        public async Task<IEnumerable<Room>> GetAllAsync(string active)
        {
            IEnumerable<Room> entities;
            
            if(string.IsNullOrEmpty(active))
            {
                entities = await base.GetAllAsync();
            }
            else
            {
                Expression<Func<Room, bool>> filter;
                if(active == "true")
                {
                    filter = x => x.ExpiresAt > DateTimeOffset.Now;
                }
                else
                {
                    filter = x => x.ExpiresAt <= DateTimeOffset.Now;
                }
                
                entities = await base.GetAllAsync(filter);
            }
            
            return entities;
        }

        public override async Task<Room> CreateAsync(Room entity)
        {
            // Unique Title and Unique Id
            var repeatedRoom = await _repository.GetAsync(room => room.Id == entity.Id);
            if(repeatedRoom != null)
            {
                entity.IsValid = false;
                entity.Errors.Add("Id", new object[] { "The Field Id is repeated" });
            }
            
            repeatedRoom = null;
            repeatedRoom = await _repository.GetAsync(room => room.Title == entity.Title);
            if(repeatedRoom != null)
            {
                entity.IsValid = false;
                entity.Errors.Add("Title", new object[] { "The Field Title is repeated" });
            }

            if(entity.IsValid)
                await base.CreateAsync(entity);

            return entity;
        }
    }
}