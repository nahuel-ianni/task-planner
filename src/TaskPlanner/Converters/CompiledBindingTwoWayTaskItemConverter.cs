using DataModels;
using System;
using Windows.UI.Xaml.Data;

namespace TaskPlanner.Converters
{
    /// <summary>
    /// Workaround used when working with compiled bindings and objects in a two way binding
    /// mode which requires the binded object to be casted to its actual type.
    /// This is because of a bug in XAML in which it doesn't recognize the type correctly when on two way 
    /// binding mode.
    /// </summary>
    /// <seealso cref="Windows.UI.Xaml.Data.IValueConverter" />
    public class CompiledBindingTwoWayTaskItemConverter : IValueConverter
    {
        /// <summary>
        /// Returns an object as an observable collection of task items.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>An observable collection of task items.</returns>
        public object Convert(object value, Type targetType, object parameter, string language) => value as ITaskItem;

        /// <summary>
        /// Converts the value back.
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, string language) => value;
    }
}