using BuildingCompany.Connection;
using BuildingCompany.Utilities;
using BuildingCompany.Windows.EditDialogWindow;

namespace BuildingCompany.ViewModels.ClientViewModels
{
    public class ClientVM : ViewModelBase
    {
        private RelayCommand _editClient;

        public RelayCommand EditCommand =>
            _editClient ?? (_editClient = new RelayCommand(arg => EditClient(), arg => UserData.UserData.Instance.User.HasPermission(Permissions.Permission.ShowClient)));

        private readonly Client _client;

        public int ID => _client.ID;
        public string Surname => _client.Surname;
        public string Name => _client.Name;
        public string Patronymic => _client.Patronymic;
        public string Phone => _client.Phone;
        public string FullName => _client.FullName;
        public bool IsDeleted => _client.IsDeleted;

        public ClientVM(Client client) =>
            _client = client;

        private void EditClient()
        {
            EditDialogWindow editDialog = new EditDialogWindow(new EditClientVM(_client));
            editDialog.ShowDialog();

            OnPropertyChanged(nameof(ID));
            OnPropertyChanged(nameof(Surname));
            OnPropertyChanged(nameof(Name));
            OnPropertyChanged(nameof(Patronymic));
            OnPropertyChanged(nameof(FullName));
        }

        public void Delete() => _client.Delete();
    }
}
