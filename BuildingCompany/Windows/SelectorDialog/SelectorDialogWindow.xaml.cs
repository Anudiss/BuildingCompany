using BuildingCompany.Utilities;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;

namespace BuildingCompany.Windows.SelectorDialog
{
    /// <summary>
    /// Логика взаимодействия для SelectorDialogWindow.xaml
    /// </summary>
    public partial class SelectorDialogWindow : Window, INotifyPropertyChanged
    {
        private RelayCommand _selectCommand;
        private RelayCommand _cancelCommand;
        private RelayCommand _closeCommand;
        private IEnumerable<object> _sourceElements;

        public RelayCommand SelectCommand =>
            _selectCommand ?? (_selectCommand = new RelayCommand((arg) => Select(), (arg) => !SelectedEmpty));

        public RelayCommand CancelCommand =>
            _cancelCommand ?? (_cancelCommand = new RelayCommand((arg) => Cancel()));

        public RelayCommand CloseCommand =>
            _closeCommand ?? (_closeCommand = new RelayCommand((arg) => Close()));

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));

        public IEnumerable<object> SourceElements
        {
            get => _sourceElements;
            set
            {
                _sourceElements = value;
                OnPropertyChanged();
            }
        }
        public IEnumerable<object> SelectedElements => ElementsContainer.SelectedItems.Cast<object>();
        public bool SelectedEmpty => SelectedElements == null || SelectedElements.Count() == 0;

        public string DisplayMemberPath
        {
            get { return (string)GetValue(DisplayMemberPathProperty); }
            set { SetValue(DisplayMemberPathProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DisplayBinding.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DisplayMemberPathProperty =
            DependencyProperty.Register("DisplayMemberPath", typeof(string), typeof(SelectorDialogWindow), new PropertyMetadata(""));

        public SelectorDialogWindow(IEnumerable<object> selectableElements)
        {
            InitializeComponent();

            SourceElements = selectableElements;
        }

        private void Select()
        {
            DialogResult = true;
            Close();
        }

        private void Cancel()
        {
            DialogResult = false;
            Close();
        }

        private void Window_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.ChangedButton == System.Windows.Input.MouseButton.Left)
                DragMove();
        }
    }
}
