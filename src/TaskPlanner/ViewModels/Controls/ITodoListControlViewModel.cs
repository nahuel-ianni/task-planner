using DataModels;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace TaskPlanner.ViewModels
{
    public interface ITodoListControlViewModel
    {
        /// <summary>
        /// Gets the tasks.
        /// </summary>
        /// <value>
        /// The tasks.
        /// </value>
        ObservableCollection<ITaskItem> Tasks { get; set; }

        /// <summary>
        /// Gets or sets the task item.
        /// </summary>
        /// <value>
        /// The task item.
        /// </value>
        ITaskItem TaskItem { get; set; }

        /// <summary>
        /// Gets or sets the selected task item.
        /// </summary>
        /// <value>
        /// The selected task item.
        /// </value>
        ITaskItem SelectedTaskItem { get; set; }

        /// <summary>
        /// Occurs when [show details called].
        /// </summary>
        event System.EventHandler<ITaskItem> ShowDetailsCalled;

        /// <summary>
        /// Gets or sets the new name of the task.
        /// </summary>
        /// <value>
        /// The new name of the task.
        /// </value>
        string NewTaskName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether it should show the completed tasks.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [show completed tasks]; otherwise, <c>false</c>.
        /// </value>
        bool ShowTasks { get; set; }

        /// <summary>
        /// Adds the new task to the todo task collection.
        /// </summary>
        void AddNewTask();

        /// <summary>
        /// Replaces the tasks collection items with the new ones.
        /// </summary>
        /// <param name="taskItems">The new task items.</param>
        void UpdateTasks(IEnumerable<ITaskItem> taskItems);

        /// <summary>
        /// Calls for the ShowDetails event passing the selected item.
        /// </summary>
        void ShowDetails();

        /// <summary>
        /// Calls for the ShowDetails event passing a null item.
        /// </summary>
        void HideDetails();
    }
}
