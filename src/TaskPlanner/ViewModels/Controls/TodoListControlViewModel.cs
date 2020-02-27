using DataModels;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Template10.Mvvm;
using Template10.Utils;

namespace TaskPlanner.ViewModels
{
    /// <summary>
    /// The to-do list control view model.
    /// </summary>
    /// <seealso cref="Template10.Mvvm.ViewModelBase" />
    public class TodoListControlViewModel : ViewModelBase, ITodoListControlViewModel
    {
        private ObservableCollection<ITaskItem> tasks;
        private ITaskItem taskItem;
        private ITaskItem selectedTaskItem;
        private string newTaskName;
        private bool showTasks;

        /// <inheritdoc />
        public ObservableCollection<ITaskItem> Tasks
        {
            get { return this.tasks; }
            set { this.Set(ref this.tasks, value); }
        }

        /// <inheritdoc />
        public ITaskItem TaskItem
        {
            get { return this.taskItem; }
            set { this.Set(ref this.taskItem, value); }
        }

        /// <inheritdoc />
        public ITaskItem SelectedTaskItem
        {
            get { return this.selectedTaskItem; }
            set { this.Set(ref this.selectedTaskItem, value); }
        }

        /// <inheritdoc />
        public event System.EventHandler<ITaskItem> ShowDetailsCalled;

        /// <inheritdoc />
        public string NewTaskName
        {
            get { return this.newTaskName; }
            set { this.Set(ref this.newTaskName, value); }
        }

        /// <inheritdoc />
        public bool ShowTasks
        {
            get { return this.showTasks; }
            set { this.Set(ref this.showTasks, value); }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TodoListControlViewModel"/> class.
        /// Used for referencing the VM directly from a xaml view.
        /// </summary>
        public TodoListControlViewModel() => this.InitializeComponent(null);

        /// <summary>
        /// Initializes a new instance of the <see cref="TodoListControlViewModel" /> class.
        /// </summary>
        /// <param name="taskItems">The task items.</param>
        public TodoListControlViewModel(IEnumerable<ITaskItem> taskItems) => this.InitializeComponent(taskItems);

        /// <inheritdoc />
        public void AddNewTask()
        {
            if (string.IsNullOrEmpty(this.NewTaskName))
                return;

            var item = this.CreateTaskItem();
            item.Title = this.NewTaskName;
            this.NewTaskName = string.Empty;

            this.Tasks.Add(item);

            this.RaisePropertyChanged(() => this.Tasks);
        }

        /// <inheritdoc />
        public void UpdateTasks(IEnumerable<ITaskItem> taskItems)
        {
            this.Tasks.Clear();
            this.Tasks.AddRange(taskItems);

            this.RaisePropertyChanged(() => this.Tasks);
        }

        /// <inheritdoc />
        public void ShowDetails() => this.ShowDetailsCalled?.Invoke(this, this.SelectedTaskItem);

        /// <inheritdoc />
        public void HideDetails() => this.ShowDetailsCalled?.Invoke(this, null);

        private void InitializeComponent(IEnumerable<ITaskItem> taskItems)
        {
            this.Tasks = taskItems == null ? new ObservableCollection<ITaskItem>() : new ObservableCollection<ITaskItem>(taskItems);
            this.Tasks.CollectionChanged += ViewModelTasks_CollectionChanged;

            this.ShowTasks = true;
            this.TaskItem = new TaskItem(string.Empty);
        }

        private ITaskItem CreateTaskItem()
        {
            var taskItem = this.TaskItem.CloneItem();

            taskItem.Title = string.Empty;
            taskItem.Notes = string.Empty;
            taskItem.Completed = false;
            taskItem.DueDate = new System.DateTimeOffset(System.DateTime.Today);

            return taskItem;
        }

        private void ViewModelTasks_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action != System.Collections.Specialized.NotifyCollectionChangedAction.Add)
                return;

            var viewModelTasks = sender as ObservableCollection<ITaskItem>;

            if (viewModelTasks != null)
                foreach (var item in viewModelTasks)
                    item.OrderPosition = viewModelTasks.IndexOf(item);
        }
    }
}
