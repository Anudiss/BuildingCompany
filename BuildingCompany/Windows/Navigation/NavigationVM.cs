using BuildingCompany.Utilities;
using BuildingCompany.ViewModels;
using BuildingCompany.ViewModels.ClientViewModels;
using BuildingCompany.ViewModels.EmployeeViewMmodels;
using BuildingCompany.ViewModels.HouseViewModels;
using BuildingCompany.ViewModels.MaterialViewModels;
using BuildingCompany.ViewModels.OrderViewModels;
using BuildingCompany.ViewModels.SupplierViewModels;
using BuildingCompany.ViewModels.SupplyViewModels;
using BuildingCompany.Windows.Auth.View;
using System;

namespace BuildingCompany.Windows.Navigation
{
    public class NavigationVM : ViewModelBase
    {
        private object _currentView;

        public Action Close;
        public Action Minimize;

        private RelayCommand _closeCommand;
        private RelayCommand _signOutCommand;
        private RelayCommand _minimizeeCommand;
        private RelayCommand _houseCommand;
        private RelayCommand _materialsCommand;
        private RelayCommand _supplierCommand;
        private RelayCommand _employeeCommand;
        private RelayCommand _supplyCommand;
        private RelayCommand _clientCommand;
        private RelayCommand _orderCommand;

        public RelayCommand HouseCommand =>
            _houseCommand ?? (_houseCommand = new RelayCommand((arg) => CurrentView = new HousePageVM()));
        public RelayCommand CloseCommand =>
            _closeCommand ?? (_closeCommand = new RelayCommand(arg => Close?.Invoke()));
        public RelayCommand MinimizeCommand =>
            _minimizeeCommand ?? (_minimizeeCommand = new RelayCommand(arg => Minimize?.Invoke()));
        public RelayCommand SignOutCommand =>
            _signOutCommand ?? (_signOutCommand = new RelayCommand(arg => SignOut()));
        public RelayCommand MaterialsCommand =>
            _materialsCommand ?? (_materialsCommand = new RelayCommand(arg => CurrentView = new MaterialPageVM()));
        public RelayCommand SupplierCommand =>
            _supplierCommand ?? (_supplierCommand = new RelayCommand(arg => CurrentView = new SupplierPageVM()));
        public RelayCommand EmployeeCommand =>
            _employeeCommand ?? (_employeeCommand = new RelayCommand(arg => CurrentView = new EmployeePageVM()));
        public RelayCommand SupplyCommand =>
            _supplyCommand ?? (_supplyCommand = new RelayCommand(arg => CurrentView = new SupplyPageVM()));
        public RelayCommand ClientCommand =>
            _clientCommand ?? (_clientCommand = new RelayCommand(arg => CurrentView = new ClientPageVM()));
        public RelayCommand OrderCommand =>
            _orderCommand ?? (_orderCommand = new RelayCommand(arg => CurrentView = new OrderPageVM()));

        public object CurrentView
        {
            get => _currentView;
            set
            {
                _currentView = value;
                OnPropertyChanged();
            }
        }

        private void SignOut()
        {
            new AuthWindow().Show();
            Close?.Invoke();
        }
    }
}
