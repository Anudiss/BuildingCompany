using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

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
        #region LabelColor
        public SolidColorBrush LabelColor
        {
            get { return (SolidColorBrush)GetValue(LabelColorProperty); }
            set { SetValue(LabelColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LabelColorProperty =
            DependencyProperty.Register("LabelColor", typeof(SolidColorBrush), typeof(TextInput));
        #endregion
        #region Label
        public string Label
        {
            get { return (string)GetValue(LabelProperty); }
            set { SetValue(LabelProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Label.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LabelProperty =
            DependencyProperty.Register("Label", typeof(string), typeof(TextInput));
        #endregion
        #region Label fontSize
        public int LabelFontSize
        {
            get { return (int)GetValue(LabelFontSizeProperty); }
            set { SetValue(LabelFontSizeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LabelFontSize.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LabelFontSizeProperty =
            DependencyProperty.Register("LabelFontSize", typeof(int), typeof(TextInput));
        #endregion
        #region Label fontWeight
        public FontWeight LabelFontWeight
        {
            get { return (FontWeight)GetValue(LabelFontWeightProperty); }
            set { SetValue(LabelFontWeightProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LabelFontWeight.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LabelFontWeightProperty =
            DependencyProperty.Register("LabelFontWeight", typeof(FontWeight), typeof(TextInput));
        #endregion

        public bool HasError => string.IsNullOrEmpty(Text) || (bool)GetValue(Validation.HasErrorProperty);

        static TextInput() =>
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TextInput),
                                                     new FrameworkPropertyMetadata(typeof(TextInput)));
    }
}
