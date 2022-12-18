using BuildingCompany.Windows.Navigation;
using System.Windows;
using System.Windows.Input;

namespace BuildingCompany.Windows
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            DataContext = new NavigationVM()
            {
                Close = Close,
                Minimize = Minimize
            };
        }

        private void Minimize() => WindowState = WindowState.Minimized;

        private void MainWindowRoot_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                DragMove();
        }
    }
}
