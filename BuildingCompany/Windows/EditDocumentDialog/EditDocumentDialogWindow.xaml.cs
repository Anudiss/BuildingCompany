using BuildingCompany.ViewModels;
using System.Windows;

namespace BuildingCompany.Windows.EditDocumentDialog
{
    /// <summary>
    /// Логика взаимодействия для EditDocumentDialogWindow.xaml
    /// </summary>
    public partial class EditDocumentDialogWindow : Window
    {
        private DocumentViewModelBase _documentViewModelBase;

        public EditDocumentDialogWindow(DocumentViewModelBase windowViewModelBase)
        {
            DataContext = _documentViewModelBase = windowViewModelBase;
            _documentViewModelBase.WindowClose = Close;

            InitializeComponent();
        }

        private void EditDialogWindowRoot_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.ChangedButton == System.Windows.Input.MouseButton.Left)
                DragMove();
        }
    }
}
