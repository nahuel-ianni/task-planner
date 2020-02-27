using SQLite.Net.Attributes;
using System;
using Template10.Mvvm;

namespace DataModels
{
    /// <summary>
    /// A task item representing an item on a to-do list.
    /// </summary>
    /// <seealso cref="Template10.Mvvm.BindableBase" />
    public class TaskItem : BindableBase, ITaskItem
    {
        private int id;
        private int orderPosition;
        private string title = string.Empty;
        private string notes = string.Empty;
        private bool completed;
        private DateTimeOffset dueDate;

        /// <inheritdoc />
        [PrimaryKey, AutoIncrement]
        public int Id
        {
            get { return this.id; }
            private set { this.Set(ref this.id, value); }
        }

        /// <inheritdoc />
        public int OrderPosition
        {
            get { return this.orderPosition; }
            set { this.Set(ref this.orderPosition, value); }
        }

        /// <inheritdoc />
        public string Title
        {
            get { return this.title; }
            set { this.Set(ref this.title, value); }
        }

        /// <inheritdoc />
        public string Notes
        {
            get { return this.notes; }
            set { this.Set(ref this.notes, value); }
        }

        /// <inheritdoc />
        public bool Completed
        {
            get { return this.completed; }
            set { this.Set(ref this.completed, value); }
        }

        /// <inheritdoc />
        public DateTimeOffset DueDate
        {
            get { return this.dueDate.ToLocalTime(); }
            set { this.Set(ref this.dueDate, value); }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskItem"/> class.
        /// Needed in order to work with SQLite.
        /// </summary>
        public TaskItem() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskItem"/> class.
        /// </summary>
        /// <param name="title">The title of the task item.</param>
        public TaskItem(string title)
        {
            this.Title = title;
            this.DueDate = new DateTimeOffset(DateTime.Today).UtcDateTime;
        }

        /// <inheritdoc />
        public ITaskItem CloneItem() => this.MemberwiseClone() as ITaskItem;        
    }
}
