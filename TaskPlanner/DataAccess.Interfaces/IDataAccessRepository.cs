using System.Collections.Generic;

namespace DataAccess.Interfaces
{
    /// <summary>
    /// A generic interface for data access.
    /// </summary>
    public interface IDataAccessRepository
    {
        /// <summary>
        /// Creates the table for the specified entity if it does not exist.
        /// </summary>
        /// <typeparam name="T">An IEntity object.</typeparam>
        void Create<T>() where T : IEntity;

        /// <summary>
        /// Inserts the specified entity.
        /// </summary>
        /// <typeparam name="T">An IEntity object.</typeparam>
        /// <param name="entity">The entity.</param>
        void Insert<T>(T entity) where T : IEntity;

        /// <summary>
        /// Updates the specified entity.
        /// </summary>
        /// <typeparam name="T">An IEntity object.</typeparam>
        /// <param name="entity">The entity.</param>
        void Update<T>(T entity) where T : IEntity;

        /// <summary>
        /// Updates the specified entities.
        /// </summary>
        /// <typeparam name="T">An IEntity object.</typeparam>
        /// <param name="entities">The entities.</param>
        void Update<T>(IEnumerable<T> entities) where T : IEntity;

        /// <summary>
        /// Removes the specified entity by its identifier.
        /// </summary>
        /// <typeparam name="T">An IEntity object.</typeparam>
        /// <param name="id">The identifier.</param>
        void Remove<T>(int id) where T : IEntity;

        /// <summary>
        /// Gets the entity collection.
        /// </summary>
        /// <typeparam name="T">An IEntity object.</typeparam>
        /// <returns>
        /// Collection with all the entities in the specified
        /// type database.
        /// </returns>
        IEnumerable<T> GetItems<T>() where T : class, IEntity;

        /// <summary>
        /// Commits the changes made to the DB.
        /// </summary>
        void CommitChanges();
    }
}