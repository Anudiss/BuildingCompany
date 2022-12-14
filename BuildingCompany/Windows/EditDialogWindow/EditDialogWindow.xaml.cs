using BuildingCompany.ViewModels;
using System.Windows;

namespace BuildingCompany.Windows.EditDialogWindow
{
    /// <summary>
    /// Логика взаимодействия для EditDialogWindow.xaml
    /// </summary>
    public partial class EditDialogWindow : Window
    {
        private WindowViewModelBase _windowViewModelBase;

        public EditDialogWindow(WindowViewModelBase windowViewModelBase)
        {
            DataContext = _windowViewModelBase = windowViewModelBase;
            _windowViewModelBase.WindowClose = Close;

            InitializeComponent();
        }

        private void EditDialogWindowRoot_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.ChangedButton == System.Windows.Input.MouseButton.Left)
                DragMove();
        }
    }
}
