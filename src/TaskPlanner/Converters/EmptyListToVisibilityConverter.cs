using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace TaskPlanner.Converters
{
    /// <summary>
    /// Checks whether the passed argument is an empty list or not and returns
    /// a visible argument if it is, otherwise a collapsed argument.
    /// </summary>
    /// <seealso cref="Windows.UI.Xaml.Data.IValueConverter" />
    public class EmptyListToVisibilityConverter : IValueConverter
    {
        /// <summary>
        /// Enum indicating the type of visibility desired.
        /// </summary>
        public enum VisibilityCondition
        {
            ShowWhenEmpty,
            HideWhenEmpty,
        }

        /// <summary>
        /// Checks whether the passed argument is an empty list or not and returns
        /// a visible argument if it is, otherwise a collapsed argument.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="parameter">Visibility condition.</param>
        /// <returns>
        /// Visible if the list is not empty, otherwise Collapsed.
        /// </returns>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var visibility = Visibility.Collapsed;

            if (value == null)
            {
                return visibility;
            }

            VisibilityCondition visibilityCondition;
            var conversionSuccessful = Enum.TryParse(parameter.ToString(), out visibilityCondition);
            var collection = value as IEnumerable<object>;

            if (conversionSuccessful &&
                collection != null)
            {
                switch (visibilityCondition)
                {
                    case VisibilityCondition.ShowWhenEmpty:
                        visibility = collection.Any() ? Visibility.Collapsed : Visibility.Visible;
                        break;

                    case VisibilityCondition.HideWhenEmpty:
                        visibility = collection.Any() ? Visibility.Visible : Visibility.Collapsed;
                        break;
                }
            }

            return visibility;
        }

        /// <summary>
        /// Converts the value back.
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return Visibility.Collapsed;
        }
    }
}
