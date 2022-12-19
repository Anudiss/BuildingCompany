using BuildingCompany.Connection;
using BuildingCompany.Utilities;
using BuildingCompany.Windows.EditDialogWindow;

namespace BuildingCompany.ViewModels.OrderViewModels
{
    public class OrderVM : ViewModelBase
    {
        private RelayCommand _editCommand;

        public RelayCommand EditCommand =>
            _editCommand ?? (_editCommand = new RelayCommand(arg => EditMaterial(), arg => UserData.UserData.Instance.User.HasPermission(Permissions.Permission.ShowOrder)));

        private readonly Order _order;

        public int ID => _order.ID;
        public Client Client => _order.Client;
        public Employee Executor => _order.Employee;
        public House House => _order.House;
        public Stage Stage => _order.Stage;
        public bool IsDeleted => _order.IsDeleted;

        public OrderVM(Order order) =>
            _order = order;

        private void EditMaterial()
        {
            EditDialogWindow editDialog = new EditDialogWindow(new EditOrderVM(_order));
            editDialog.ShowDialog();

            OnPropertyChanged(nameof(ID));
            OnPropertyChanged(nameof(Client));
            OnPropertyChanged(nameof(Executor));
            OnPropertyChanged(nameof(House));
            OnPropertyChanged(nameof(Stage));
        }

        public void Delete() => _order.Delete();
    }
}
