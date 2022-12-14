using FontAwesome.WPF;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace BuildingCompany.Components
{
    public class NavigationButton : RadioButton
    {
        #region Title
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Title.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(NavigationButton));
        #endregion
        #region Text color
        public SolidColorBrush TextColor
        {
            get { return (SolidColorBrush)GetValue(solidColorBrushProperty); }
            set { SetValue(solidColorBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for solidColorBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty solidColorBrushProperty =
            DependencyProperty.Register("TextColor", typeof(SolidColorBrush), typeof(NavigationButton));
        #endregion
        #region Icon
        public FontAwesomeIcon Icon
        {
            get { return (FontAwesomeIcon)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register("Icon", typeof(FontAwesomeIcon), typeof(NavigationButton));
        #endregion
        #region Icon brush
        public SolidColorBrush IconBrush
        {
            get { return (SolidColorBrush)GetValue(IconBrushProperty); }
            set { SetValue(IconBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IconBrushProperty =
            DependencyProperty.Register("IconBrush", typeof(SolidColorBrush), typeof(NavigationButton));
        #endregion
        #region Indicator color
        public SolidColorBrush IndicatorColor
        {
            get { return (SolidColorBrush)GetValue(IndicatorColorProperty); }
            set { SetValue(IndicatorColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IndicatorColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IndicatorColorProperty =
            DependencyProperty.Register("IndicatorColor", typeof(SolidColorBrush), typeof(NavigationButton));
        #endregion

        static NavigationButton() =>
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NavigationButton),
                                                     new FrameworkPropertyMetadata(typeof(NavigationButton)));
    }
}
