using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace TaskPlanner.Converters
{
    /// <summary>
    /// Converts a null object value into a visibility enum value.
    /// </summary>
    /// <seealso cref="Windows.UI.Xaml.Data.IValueConverter" />
    public class NullObjectToVisibilityConverter : IValueConverter
    {
        /// <summary>
        /// Returns a visibility value based on a null object.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>Visibility value.</returns>
        public object Convert(object value, Type targetType, object parameter, string language) =>
            value != null 
                ? Visibility.Visible 
                : Visibility.Collapsed;

        /// <summary>
        /// Converts the value back.
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, string language) => null;
    }
}
