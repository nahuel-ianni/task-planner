using DataAccess.Interfaces;
using System;

namespace DataModels
{
    /// <summary>
    /// A task item representing an item on a to-do list.
    /// </summary>
    /// <seealso cref="Template10.Mvvm.BindableBase" />
    public interface ITaskItem : IEntity
    {
        /// <summary>
        /// Gets or sets the order position.
        /// </summary>
        /// <value>
        /// The order position.
        /// </value>
        int OrderPosition { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        string Title { get; set; }

        /// <summary>
        /// Gets or sets the notes.
        /// </summary>
        /// <value>
        /// The notes.
        /// </value>
        string Notes { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="TaskItem"/> is completed.
        /// </summary>
        /// <value>
        ///   <c>true</c> if completed; otherwise, <c>false</c>.
        /// </value>
        bool Completed { get; set; }

        /// <summary>
        /// Gets or sets the due date.
        /// </summary>
        /// <value>
        /// The due date.
        /// </value>
        DateTimeOffset DueDate { get; set; }

        /// <summary>
        /// Clones the item.
        /// </summary>
        /// <returns>A clean copy of this item.</returns>
        ITaskItem CloneItem();
    }
}
