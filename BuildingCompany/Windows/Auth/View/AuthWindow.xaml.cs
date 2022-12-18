using BuildingCompany.Windows.Auth.ViewModel;
using System.Windows;

namespace BuildingCompany.Windows.Auth.View
{
    /// <summary>
    /// Логика взаимодействия для AuthWindow.xaml
    /// </summary>
    public partial class AuthWindow : Window
    {
        public AuthWindow()
        {
            InitializeComponent();

            DataContext = AuthNavigationVM.Instance;
            AuthNavigationVM.Instance.CurrentView = new LoginVM();
            AuthNavigationVM.Instance.Close = Close;
            AuthNavigationVM.Instance.Minimize = () => WindowState = WindowState.Minimized;
        }

        private void AuthWindowRoot_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.ChangedButton == System.Windows.Input.MouseButton.Left)
                DragMove();
        }
    }
}
