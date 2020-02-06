using System;
using Windows.UI.Xaml;
using System.Threading.Tasks;
using TaskPlanner.Services.SettingsServices;
using Windows.ApplicationModel.Activation;
using System.Collections.Generic;
using Windows.ApplicationModel.Background;
using System.Linq;

namespace TaskPlanner
{
    /// Documentation on APIs used in this page:
    /// https://github.com/Windows-XAML/Template10/wiki
    sealed partial class App : Template10.Common.BootStrapper
    {
        private const uint TimeTriggerFreshnessTime = 15;

        // List with background task implementations.
        private IEnumerable<Type> backgroundTasks = new List<Type>()
        {
            typeof(BackgroundTasks.NetworkPoolerBackgroundTask),
            typeof(BackgroundTasks.TileUpdateBackgroundTask),
        };

        public App()
        {
            InitializeComponent();
            SplashFactory = (e) => new Views.Splash(e);

            #region App settings
            var _settings = SettingsService.Instance;
            RequestedTheme = _settings.AppTheme;
            CacheMaxDuration = _settings.CacheMaxDuration;
            ShowShellBackButton = _settings.UseShellBackButton;
            #endregion

            this.RegisterBackgroundTasks();
        }

        // Runs even if restored from state
        public override Task OnInitializeAsync(IActivatedEventArgs args)
        {
            // content may already be shell when resuming
            if ((Window.Current.Content as Views.Shell) == null)
            {
                // setup hamburger shell
                var nav = NavigationServiceFactory(BackButton.Attach, ExistingContent.Include);
                Window.Current.Content = new Views.Shell(nav);
            }
            return Task.CompletedTask;
        }

        // Runs only when not restored from state
        public override Task OnStartAsync(StartKind startKind, IActivatedEventArgs args)
        {
            NavigationService.Navigate(typeof(Views.MainPage));
            return Task.CompletedTask;
        }

        private async void RegisterBackgroundTasks()
        {
            /// Before registering the tasks, we must ask the OS permission
            /// to do so, and it may be approve or denied dependind on the
            /// user configuration.
            var background = await BackgroundExecutionManager.RequestAccessAsync();

            switch (background)
            {
                /// The OS has denied permission of background task execution either
                /// because of available resources or because the user has refused such 
                /// functionality.
                case BackgroundAccessStatus.Denied:
                case BackgroundAccessStatus.DeniedBySystemPolicy:
                case BackgroundAccessStatus.DeniedByUser:
                case BackgroundAccessStatus.Unspecified:
                    return;

                /// The OS has allowed the application to register and use backgroun tasks.
                case BackgroundAccessStatus.AllowedWithAlwaysOnRealTimeConnectivity:
                case BackgroundAccessStatus.AllowedMayUseActiveRealTimeConnectivity:
                case BackgroundAccessStatus.AllowedSubjectToSystemPolicy:
                case BackgroundAccessStatus.AlwaysAllowed:
                    foreach (var item in this.backgroundTasks)
                    {
                        /// Ensure the task registration is performed only once.
                        if (BackgroundTaskRegistration.AllTasks.Values.FirstOrDefault(task => task.Name == item.Name) == null)
                        {
                            this.RegisterTask(item, new TimeTrigger(App.TimeTriggerFreshnessTime, false));
                        }
                    }
                    break;
            }
        }

        private BackgroundTaskRegistration RegisterTask(
            Type taskType,
            IBackgroundTrigger trigger,
            SystemConditionType systemConditionType = SystemConditionType.Invalid)
        {
            var builder = new BackgroundTaskBuilder();

            /// A string identifier for the background task.
            builder.Name = taskType.Name;

            /// The entry point of the task.
            /// This HAS to be the full name of the background task: {Namespace}.{Class name}
            builder.TaskEntryPoint = taskType.FullName;

            /// The specific trigger event that will fire the task on our application.
            builder.SetTrigger(trigger);

            /// A condition for the task to run.
            /// If specified, after the event trigger is fired, the OS will wait for
            /// the condition situation to happen before executing the task.
            if (systemConditionType != SystemConditionType.Invalid)
            {
                builder.AddCondition(new SystemCondition(systemConditionType));
            }

            /// Register the task and returns the registration output.
            return builder.Register();
        }
    }
}

