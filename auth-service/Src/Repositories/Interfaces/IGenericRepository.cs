using System.Linq.Expressions;


namespace auth_service.Repositories.Interfaces
{



    public interface IGenericRepository<TEntity> where TEntity : class
    {
        /// <summary>
        /// Retrieves a list of entities based on the provided filter, order, and inclusion criteria.
        /// </summary>
        /// <param name="filter">A predicate to filter entities, or null for no filtering.</param>
        /// <param name="orderBy">A function to specify the order of entities, or null for no ordering.</param>
        /// <param name="includeProperties">A comma-separated list of navigation properties to include, or an empty string for no inclusion.</param>
        /// <returns>A list of entities that meet the criteria.</returns>
        Task<List<TEntity>> Get(
            Expression<Func<TEntity, bool>>? filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            string includeProperties = "");

        /// <summary>
        /// Retrieves an entity by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the entity to retrieve, usually integer.</param>
        /// <returns>The entity with the specified unique identifier, or null if not found.</returns>
        Task<TEntity?> GetByID(object id);

        /// <summary>
        /// Inserts a new entity into the repository.
        /// </summary>
        /// <param name="entity">The entity to be inserted.</param>
        /// <returns>The inserted entity.</returns>
        Task<TEntity> Insert(TEntity entity);

        /// <summary>
        /// Updates an existing entity in the repository.
        /// </summary>
        /// <param name="entityToUpdate">The entity to be updated.</param>
        /// <returns>The updated entity.</returns>
        Task<TEntity> Update(TEntity entityToUpdate);
    }
}