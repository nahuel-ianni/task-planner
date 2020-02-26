using DataModels;

namespace TaskPlanner.ViewModels
{
    /// <summary>
    /// Interface for the todo list page view model.
    /// </summary>
    public interface ITodoListPageViewModel
    {
        /// <summary>
        /// Gets or sets the completed todo list control view model.
        /// </summary>
        /// <value>
        /// The completed todo list control view model.
        /// </value>
        TodoListControlViewModel CompletedTodoListControlViewModel { get; set; }

        /// <summary>
        /// Gets or sets the pending todo list control view model.
        /// </summary>
        /// <value>
        /// The pending todo list control view model.
        /// </value>
        TodoListControlViewModel PendingTodoListControlViewModel { get; set; }

        /// <summary>
        /// Gets or sets the selected task item.
        /// </summary>
        /// <value>
        /// The selected task item.
        /// </value>
        ITaskItem SelectedTaskItem { get; set; }

        /// <summary>
        /// Saves the changes on the tasks items.
        /// </summary>
        void SaveChanges();

        /// <summary>
        /// Cancels the changes.
        /// </summary>
        void CancelChanges();

        /// <summary>
        /// Deletes the selected item.
        /// </summary>
        void DeleteSelectedItem();

        /// <summary>
        /// Cleans the selected item.
        /// </summary>
        void CleanSelectedItem();

        /// <summary>
        /// Synchronizes the selected task with calendar.
        /// </summary>
        void SynchronizePendingTaskWithCalendar();

        /// <summary>
        /// Synchronizes the pending tasks with calendar.
        /// </summary>
        void SynchronizePendingTasksWithCalendar();
    }
}
