using BuildingCompany.Connection;
using BuildingCompany.Utilities;
using BuildingCompany.ViewModels.SupplierViewModels;
using BuildingCompany.Windows.EditDialogWindow;

namespace BuildingCompany.ViewModels.EmployeeViewMmodels
{
    public class EmployeeVM : ViewModelBase
    {
        private RelayCommand _editCommand;

        public RelayCommand EditCommand =>
            _editCommand ?? (_editCommand = new RelayCommand(arg => EditEmployee()));

        private readonly Employee _employee;

        public int ID => _employee.ID;
        public string Surname => _employee.Surname;
        public string Name => _employee.Name;
        public string Patronymic => _employee.Patronymic;
        public string FullName => _employee.FullName;
        public Position Position => _employee.Position;
        public bool IsDeleted => _employee.IsDeleted;

        public EmployeeVM(Employee employee) =>
            _employee = employee;

        private void EditEmployee()
        {
            EditDialogWindow editDialog = new EditDialogWindow(new EditEmployeeVM(_employee));
            editDialog.ShowDialog();

            OnPropertyChanged(nameof(ID));
            OnPropertyChanged(nameof(FullName));
            OnPropertyChanged(nameof(Position));
        }

        public void Delete() => _employee.IsDeleted = true;
    }
}
