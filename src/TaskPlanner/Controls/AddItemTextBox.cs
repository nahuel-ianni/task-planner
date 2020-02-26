using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

namespace TaskPlanner.Controls
{
    /// <summary>
    /// A custom text box that changes its style depending on the state of the focus/content of the control.
    /// </summary>
    /// <seealso cref="Windows.UI.Xaml.Controls.TextBox" />
    public class AddItemTextBox : TextBox
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AddItemTextBox"/> class.
        /// </summary>
        public AddItemTextBox()
        {
            this.InputScope = new InputScope();
            this.InputScope.Names.Add(new InputScopeName(InputScopeNameValue.Text));
        }

        /// <summary>
        /// Raises the <see cref="E:GotFocus" /> event.
        /// </summary>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        protected override void OnGotFocus(RoutedEventArgs e)
        {
            this.Background = new SolidColorBrush(Colors.White);
            base.OnGotFocus(e);
        }

        /// <summary>
        /// Raises the <see cref="E:LostFocus" /> event.
        /// </summary>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        protected override void OnLostFocus(RoutedEventArgs e)
        {
            this.Background = new SolidColorBrush(Colors.Transparent);
            base.OnLostFocus(e);
        }
    }
}
