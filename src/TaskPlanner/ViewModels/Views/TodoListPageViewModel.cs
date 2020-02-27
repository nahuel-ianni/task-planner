using DataAccess;
using DataAccess.Interfaces;
using DataModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SystemIntegration.Managers;
using Template10.Mvvm;

namespace TaskPlanner.ViewModels
{
    /// <summary>
    /// The to-do list page view model.
    /// </summary>
    /// <seealso cref="Template10.Mvvm.ViewModelBase" />
    public class TodoListPageViewModel : ViewModelBase, ITodoListPageViewModel
    {
        private const string TaskItemCompletedPropertyName = "Completed";
        private IDataAccessRepository dataAccess;
        private TodoListControlViewModel completedTodoListControlViewModel;
        private TodoListControlViewModel pendingTodoListControlViewModel;
        private ITaskItem selectedTaskItem;

        /// <inheritdoc />
        public TodoListControlViewModel CompletedTodoListControlViewModel
        {
            get { return this.completedTodoListControlViewModel; }
            set { this.Set(ref this.completedTodoListControlViewModel, value); }
        }

        /// <inheritdoc />
        public TodoListControlViewModel PendingTodoListControlViewModel
        {
            get { return this.pendingTodoListControlViewModel; }
            set { this.Set(ref this.pendingTodoListControlViewModel, value); }
        }

        /// <inheritdoc />
        public ITaskItem SelectedTaskItem
        {
            get { return this.selectedTaskItem; }
            set { this.Set(ref this.selectedTaskItem, value); }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TodoListPageViewModel"/> class.
        /// </summary>
        public TodoListPageViewModel()
        {
            this.dataAccess = new SQLiteRepository();
            this.dataAccess.Create<TaskItem>();

            this.InitializeTodoLists();

            this.SubscribeToTaskItemPropertyChanged(this.PendingTodoListControlViewModel.TaskItem);
            this.SubscribeToTaskItemPropertyChanged(this.CompletedTodoListControlViewModel.TaskItem);

            this.PendingTodoListControlViewModel.ShowDetailsCalled += TodoListControlViewModel_ShowDetailsCalled;
            this.CompletedTodoListControlViewModel.ShowDetailsCalled += TodoListControlViewModel_ShowDetailsCalled;
        }

        /// <inheritdoc />
        public void SaveChanges()
        {
            this.dataAccess.Update<ITaskItem>(this.PendingTodoListControlViewModel.Tasks);
            this.dataAccess.Update<ITaskItem>(this.CompletedTodoListControlViewModel.Tasks);
            this.UpdateCollections();
        }

        /// <inheritdoc />
        public void CancelChanges() => this.UpdateCollections();

        /// <inheritdoc />
        public void DeleteSelectedItem()
        {
            if (this.SelectedTaskItem == null)
                return;

            // The task item should be on the database.
            if (this.SelectedTaskItem.Id != 0)
                this.dataAccess.Remove<TaskItem>(this.SelectedTaskItem.Id);

            this.RemoveTaskFromCollection(this.PendingTodoListControlViewModel.Tasks, this.SelectedTaskItem);
            this.RemoveTaskFromCollection(this.CompletedTodoListControlViewModel.Tasks, this.SelectedTaskItem);
        }

        /// <inheritdoc />
        public void CleanSelectedItem() => this.SelectedTaskItem = null;

        /// <inheritdoc />
        public async void SynchronizePendingTaskWithCalendar()
        {
            await this.SynchronizeTaskWithCalendar(this.SelectedTaskItem);
        }

        /// <inheritdoc />
        public async void SynchronizePendingTasksWithCalendar()
        {
            var items = this.PendingTodoListControlViewModel?.Tasks?.Where(taskItem => !taskItem.Completed);

            foreach (var item in items)
                await this.SynchronizeTaskWithCalendar(item);
        }

        private void InitializeTodoLists()
        {
            this.PendingTodoListControlViewModel = new TodoListControlViewModel();
            this.CompletedTodoListControlViewModel = new TodoListControlViewModel() { ShowTasks = false };

            this.UpdateCollections();
        }

        private void UpdateCollections()
        {
            var items = this.dataAccess.GetItems<TaskItem>();
            this.FillTaskCollection(items);
        }

        private void RemoveTaskFromCollection(IList<ITaskItem> items, ITaskItem item)
        {
            if (items.FirstOrDefault(taskItem => taskItem == this.SelectedTaskItem) != null)
                items.Remove(this.SelectedTaskItem);
        }

        private void SubscribeToTaskItemPropertyChanged(ITaskItem taskItem)
        {
            var castedTaskItem = taskItem as TaskItem;
            if (castedTaskItem != null)
                castedTaskItem.PropertyChanged += TaskItem_PropertyChanged;
        }

        private void FillTaskCollection(IEnumerable<ITaskItem> taskItems)
        {
            foreach (var item in taskItems)
                this.SubscribeToTaskItemPropertyChanged(item);

            this.PendingTodoListControlViewModel.UpdateTasks(taskItems.Where(taskItem => !taskItem.Completed).OrderBy(taskItem => taskItem.OrderPosition));
            this.CompletedTodoListControlViewModel.UpdateTasks(taskItems.Where(taskItem => taskItem.Completed).OrderBy(taskItem => taskItem.OrderPosition));
        }

        private async Task<string> SynchronizeTaskWithCalendar(ITaskItem taskItem)
        {
            if (taskItem == null)
                return string.Empty;

            var appointment = CalendarManager.GetAppointment(taskItem.Title, taskItem.DueDate.Date);
            appointment.Details = taskItem.Notes;

            return await CalendarManager.EditNewAppointmentAsync(appointment);
        }

        private void TaskItem_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == TodoListPageViewModel.TaskItemCompletedPropertyName)
            {
                var taskItems = this.PendingTodoListControlViewModel.Tasks.ToList();
                taskItems.AddRange(this.CompletedTodoListControlViewModel.Tasks.ToList());

                this.FillTaskCollection(taskItems);
            }
        }

        private void TodoListControlViewModel_ShowDetailsCalled(object sender, ITaskItem e)
        {
            this.SelectedTaskItem = e;
        }
    }
}
