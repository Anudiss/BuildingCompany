using BuildingCompany.Connection;
using BuildingCompany.Utilities;
using BuildingCompany.Windows.EditDialogWindow;

namespace BuildingCompany.ViewModels.SupplierViewModels
{
    public class SupplierVM : ViewModelBase
    {
        private RelayCommand _editCommand;

        public RelayCommand EditCommand =>
            _editCommand ?? (_editCommand = new RelayCommand(arg => EditMaterial(), arg => UserData.UserData.Instance.User.HasPermission(Permissions.Permission.ShowSupplier)));

        private readonly Supplier _supplier;

        public int ID => _supplier.ID;
        public string Name => _supplier.Name;
        public bool IsDeleted => _supplier.IsDeleted;

        public SupplierVM(Supplier supplier) =>
            _supplier = supplier;

        private void EditMaterial()
        {
            EditDialogWindow editDialog = new EditDialogWindow(new EditSupplierVM(_supplier));
            editDialog.ShowDialog();

            OnPropertyChanged(nameof(ID));
            OnPropertyChanged(nameof(Name));
        }

        public void Delete() => _supplier.Delete();
    }
}
