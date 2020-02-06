using DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml.Data;

namespace TaskPlanner.Converters
{
    /// <summary>
    /// Filters a list of task items and returns a filtered version of it.
    /// </summary>
    /// <seealso cref="Windows.UI.Xaml.Data.IValueConverter" />
    public class FullTaskListToFilteredListConverter : IValueConverter
    {
        /// <summary>
        /// Enum indicating the type of content to return.
        /// </summary>
        public enum ListFilter
        {
            PendingTasks,
            CompletedTasks,
        }

        /// <summary>
        /// Filters a list of task items and returns a filtered version of it.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="parameter">The parameter.</param>
        /// <returns>The completed/uncompleted items on the passed collection depending on the parameters.</returns>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null || parameter == null)
            {
                return value;
            }

            ListFilter listFilter;
            var collection = value as IEnumerable<ITaskItem>;
            var conversionSuccessful = Enum.TryParse(parameter.ToString(), out listFilter);
            IEnumerable<ITaskItem> taskItems = null;

            if (conversionSuccessful && collection != null)
            {
                switch (listFilter)
                {
                    case ListFilter.PendingTasks:
                        taskItems = collection.Where(taskItem => !taskItem.Completed);
                        break;

                    case ListFilter.CompletedTasks:
                        taskItems = collection.Where(taskItem => taskItem.Completed);
                        break;
                }
            }

            return taskItems;
        }

        /// <summary>
        /// Converts the value back.
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return value;
        }
    }
}