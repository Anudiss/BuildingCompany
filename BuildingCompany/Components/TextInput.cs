using System.Windows;
using System.Windows.Controls;

namespace BuildingCompany.Components
{
    public class TextInput : TextBox
    {
        #region Placeholder
        public string Placeholder
        {
            get { return (string)GetValue(PlaceholderProperty); }
            set { SetValue(PlaceholderProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Placeholder.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PlaceholderProperty =
            DependencyProperty.Register("Placeholder", typeof(string), typeof(TextInput));
        #endregion

        public TextInput() =>
            OverridesDefaultStyleProperty.OverrideMetadata(typeof(TextInput), new PropertyMetadata(typeof(TextInput)));
    }
}
