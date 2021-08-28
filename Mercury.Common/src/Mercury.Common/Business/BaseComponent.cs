using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Mercury.Common.Business
{
    public class BaseComponent<T> : IBaseComponent<T> where T : IEntity
    {
        private readonly IRepository<T> repository;
        public BaseComponent(IRepository<T> repository)
        {
            this.repository = repository;
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            var entities = await repository.GetAllAsync();
            return entities;
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> filter)
        {
            var entities = await repository.GetAllAsync(filter);
            
            return entities;
        }

        public virtual async Task<T> GetAsync(Guid id)
        {
            return await repository.GetAsync(id);
        }

        public virtual async Task<T> GetAsync(Expression<Func<T, bool>> filter)
        {
            return await repository.GetAsync(filter);
        }

        public virtual async Task<T> CreateAsync(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            await repository.CreateAsync(entity);

            return entity;
        }

        public virtual async Task<T> UpdateAsync(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            await repository.UpdateAsync(entity);

            return entity;
        }
        public virtual async Task DeleteAsync(Guid id)
        {
            await repository.DeleteAsync(id);
        }



    }
}