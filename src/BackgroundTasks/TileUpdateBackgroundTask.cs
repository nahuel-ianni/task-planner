using DataModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Windows.ApplicationModel.Background;
using Windows.Data.Xml.Dom;
using Windows.Storage;
using Windows.UI.Notifications;

namespace BackgroundTasks
{
    /// <summary>
    /// This background task should be responsible of updating the primary tile
    /// every 15 minutes to show the immediate next task that needs to be done.
    /// </summary>
    /// <seealso cref="Windows.ApplicationModel.Background.IBackgroundTask" />
    public sealed class TileUpdateBackgroundTask : IBackgroundTask
    {
        private const string TextTagName = "text";
        private const string BindingTagName = "binding";
        private const string VisualTagName = "visual";
        private const string StatusLocalSettingsName = "Status";

        /// <inheritdoc />
        public void Run(IBackgroundTaskInstance taskInstance)
        {
            var items = this.GetPendingTasks();
            var taskItem = this.GetTileContent(items);

            if (taskItem != null)
            {
                var tileTemplates = this.GetTileTemplates(taskItem.Title, taskItem.DueDate.Date.ToLocalTime().ToString(CultureInfo.CurrentUICulture));

                // Update the tile content.
                var updater = TileUpdateManager.CreateTileUpdaterForApplication();
                var tileNotification = new TileNotification(tileTemplates);
                updater.Update(tileNotification);
            }
        }

        private IEnumerable<TaskItem> GetPendingTasks()
        {
            var tileContentItems = new List<TaskItem>();

            try
            {
                var dataAccess = new DataAccess.SQLiteRepository();
                var items = dataAccess.GetItems<TaskItem>();

                if (items != null)
                {
                    tileContentItems.AddRange(items.Where(item => !item.Completed));
                }
            }
            catch (Exception ex)
            {
                this.WriteStatusToAppData(ex.Message);
            }

            return tileContentItems;
        }

        private TaskItem GetTileContent(IEnumerable<TaskItem> items)
        {
            var taskItem = items.OrderBy(item => item.DueDate).FirstOrDefault(item => !item.Completed);

            return taskItem;
        }

        private XmlDocument GetTileTemplates(string title, string content)
        {
            // Set the wide tile text.
            var wideTileXmlDocument = this.GetTileXmlDocument(TileTemplateType.TileWide310x150Text02, title, content);

            // Set the medium tile text.
            var mediumTileXmlDocument = this.GetTileXmlDocument(TileTemplateType.TileSquare150x150Text02, title, content);

            // Include the medium tile in the notification.
            var node = wideTileXmlDocument.ImportNode(mediumTileXmlDocument.GetElementsByTagName(TileUpdateBackgroundTask.BindingTagName).Item(0), true);
            wideTileXmlDocument.GetElementsByTagName(TileUpdateBackgroundTask.VisualTagName).Item(0).AppendChild(node);

            return wideTileXmlDocument;
        }

        private XmlDocument GetTileXmlDocument(TileTemplateType tileTemplate, string title, string content)
        {
            var tileTemplateXmlDocument = TileUpdateManager.GetTemplateContent(tileTemplate);
            var tileTextAttributes = tileTemplateXmlDocument.GetElementsByTagName(TileUpdateBackgroundTask.TextTagName);
            tileTextAttributes[0].AppendChild(tileTemplateXmlDocument.CreateTextNode(title));
            tileTextAttributes[1].AppendChild(tileTemplateXmlDocument.CreateTextNode(content));

            return tileTemplateXmlDocument;
        }

        private void WriteStatusToAppData(string status)
        {
            var settings = ApplicationData.Current.LocalSettings;
            settings.Values[TileUpdateBackgroundTask.StatusLocalSettingsName] = status;
        }
    }
}
