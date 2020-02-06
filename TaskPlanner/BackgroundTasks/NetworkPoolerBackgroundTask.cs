using System;
using System.Threading;
using Windows.ApplicationModel.Background;
using Windows.Storage;

namespace BackgroundTasks
{
    /// <summary>
    /// This background task should be responsible for listening and/or call web
    /// services which allowed for the task list item synchronization.
    /// It has been created as an async operation because of the networking 
    /// functionality for which it should be responsible of.
    /// 
    /// There are two possibilities for its responsibility:
    /// 
    /// - The background task stores the sync info in an XML file and save it to 
    ///   the path where the sync result would be stored so that the application 
    ///   may process it when executing.
    /// 
    /// - The background task stores the task items in the DB itself so that the
    ///   application will read data from one source only.
    /// 
    /// Given that this functionality has been left out of scope for the current
    /// iteration due to lack of time, it will instead create a task item with
    /// random values and save it to the database so that the application may 
    /// process it when executing.
    /// </summary>
    /// <seealso cref="Windows.ApplicationModel.Background.IBackgroundTask" />
    public sealed class NetworkPoolerBackgroundTask : IBackgroundTask
    {
        private const string StatusLocalSettingsName = "Status";
        private CancellationTokenSource cancellationTokenSource = null;

        /// <inheritdoc />
        async void IBackgroundTask.Run(IBackgroundTaskInstance taskInstance)
        {
            var deferral = taskInstance.GetDeferral();

            try
            {
                // Associate a cancellation handler with the background task. 
                taskInstance.Canceled += this.OnCanceled;

                // Get the cancellation token 
                if (this.cancellationTokenSource == null)
                {
                    this.cancellationTokenSource = new CancellationTokenSource();
                }

                CancellationToken token = cancellationTokenSource.Token;

                /// TODO: Implement network code here.
                /// NOTE: Outside of this version scope.
                this.CreateNewTaskItem();
            }
            catch (Exception ex)
            {
                this.WriteStatusToAppData(ex.ToString());
            }
            finally
            {
                this.cancellationTokenSource = null;
                deferral.Complete();
            }
        }

        private void CreateNewTaskItem()
        {
            var taskItem = new DataModels.TaskItem()
            {
                Title = "NetworkPoolerBackgroundTask",
                Notes = $"This task was created by the 'NetworkPoolerBackgroundTask' at {DateTime.Now}",
                DueDate = new DateTimeOffset(DateTime.Today),
                Completed = false,
            };

            var dataAccess = new DataAccess.SQLiteRepository();
            dataAccess.Create<DataModels.TaskItem>();
            dataAccess.Insert(taskItem);
        }

        private void WriteStatusToAppData(string status)
        {
            var settings = ApplicationData.Current.LocalSettings;
            settings.Values[NetworkPoolerBackgroundTask.StatusLocalSettingsName] = status;
        }

        private void OnCanceled(IBackgroundTaskInstance sender, BackgroundTaskCancellationReason reason)
        {
            if (this.cancellationTokenSource != null)
            {
                this.cancellationTokenSource.Cancel();
                this.cancellationTokenSource = null;
            }
        }
    }
}
