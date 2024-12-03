using System.Linq.Expressions;
using auth_service.Data;
using auth_service.Models;
using auth_service.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace auth_service.Repositories;

public abstract class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
{
        protected DataContext context;
        protected DbSet<TEntity> dbSet;

        public GenericRepository(DataContext context)
        {
            this.context = context;
            this.dbSet = context.Set<TEntity>();
        }

        public virtual async Task<List<TEntity>> Get(
            Expression<Func<TEntity, bool>>? filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            string includeProperties = "")
        {
            IQueryable<TEntity> query = dbSet;


            if (typeof(BaseModel).IsAssignableFrom(typeof(TEntity)))
            {
#pragma warning disable CS8602 // Already checked in if statement
                query = query.Where(x => (x as BaseModel).DeletedAt == null);
#pragma warning restore CS8602
            }

            if (filter is not null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy is not null)
            {
                return await orderBy(query).ToListAsync();
            }
            return await query.ToListAsync();
        }

        public virtual async Task<TEntity?> GetByID(object id)
        {
            var entity = await dbSet.FindAsync(id);
            if (entity is BaseModel baseModel && baseModel.DeletedAt is not null)
            {
                return null;
            }
            return entity;

        }

        public virtual async Task<TEntity> Insert(TEntity entity)
        {
            await dbSet.AddAsync(entity);

            await context.SaveChangesAsync();
            return entity;
        }

        public virtual async Task<TEntity> Update(TEntity entityToUpdate)
        {
            if (entityToUpdate is BaseModel baseModel)
            {
                baseModel.UpdatedAt = DateTime.Now;
                baseModel.Version++;
            }

            dbSet.Attach(entityToUpdate);
            context.Entry(entityToUpdate).State = EntityState.Modified;

            await context.SaveChangesAsync();
            return entityToUpdate;
        }
    
}