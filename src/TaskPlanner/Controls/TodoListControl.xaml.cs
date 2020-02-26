using TaskPlanner.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace TaskPlanner.Controls
{
    public sealed partial class TodoListControl : UserControl
    {
        /// <summary>
        /// Gets or sets the creation controls visibility.
        /// </summary>
        /// <value>
        /// The creation visibility.
        /// </value>
        public Visibility CreationVisibility
        {
            get { return (Visibility)this.GetValue(CreationVisibilityProperty); }
            set { this.SetValue(CreationVisibilityProperty, value); }
        }

        /// <summary>
        /// The creation visibility dependency property
        /// </summary>
        public static readonly DependencyProperty CreationVisibilityProperty =
            DependencyProperty.Register("CreationVisibility", typeof(Visibility), typeof(TodoListControl), new PropertyMetadata(0));

        /// <summary>
        /// Gets or sets the toggle button visibility.
        /// </summary>
        /// <value>
        /// The toggle button visibility.
        /// </value>
        public Visibility ToggleButtonVisibility
        {
            get { return (Visibility)this.GetValue(ToggleButtonVisibilityProperty); }
            set { this.SetValue(ToggleButtonVisibilityProperty, value); }
        }

        /// <summary>
        /// The toggle button visibility dependency property
        /// </summary>
        public static readonly DependencyProperty ToggleButtonVisibilityProperty =
            DependencyProperty.Register("ToggleButtonVisibility", typeof(Visibility), typeof(TodoListControl), new PropertyMetadata(0));

        /// <summary>
        /// Gets or sets the view model.
        /// </summary>
        /// <value>
        /// The view model.
        /// </value>
        public TodoListControlViewModel ViewModel
        {
            get { return (TodoListControlViewModel)this.GetValue(ViewModelProperty); }
            set { this.SetValue(ViewModelProperty, value); }
        }

        /// <summary>
        /// The view model dependency property
        /// </summary>
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel", typeof(TodoListControlViewModel), typeof(TodoListControl), new PropertyMetadata(0));

        /// <summary>
        /// Initializes a new instance of the <see cref="TodoListControl"/> class.
        /// </summary>
        public TodoListControl()
        {
            this.InitializeComponent();
        }
    }
}
