namespace DataAccess.Interfaces
{
    /// <summary>
    /// Interface for entities used in databases.
    /// </summary>
    public interface IEntity
    {
        /// <summary>
        /// Gets the entity identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        int Id { get; }
    }
}
