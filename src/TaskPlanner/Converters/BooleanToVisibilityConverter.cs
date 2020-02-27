using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace TaskPlanner.Converters
{
    /// <summary>
    /// Converts a boolean value into a visibility enum value.
    /// </summary>
    /// <seealso cref="Windows.UI.Xaml.Data.IValueConverter" />
    public class BooleanToVisibilityConverter : IValueConverter
    {
        /// <summary>
        /// Returns a visibility value based on a bool object.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>Visibility value.</returns>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var visibility = Visibility.Collapsed;

            if (value == null)
                return visibility;

            bool boolObject;
            var conversionSuccessful = Boolean.TryParse(value.ToString(), out boolObject);

            if (conversionSuccessful)
                visibility = boolObject ? Visibility.Visible : Visibility.Collapsed;

            return visibility;
        }

        /// <summary>
        /// Converts the value back.
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            var visibility = false;

            if (value == null)
                return visibility;

            Visibility visibilityObject;
            var conversionSuccessful = Visibility.TryParse(value.ToString(), out visibilityObject);

            if (conversionSuccessful)
                visibility = visibilityObject == Visibility.Visible ? true : false;

            return visibility;
        }
    }
}
