using BuildingCompany.Connection;
using BuildingCompany.Utilities;
using BuildingCompany.ViewModels.SupplierViewModels;
using BuildingCompany.Windows.EditDialogWindow;
using BuildingCompany.Windows.EditDocumentDialog;
using System;
using System.Linq;

namespace BuildingCompany.ViewModels.SupplyViewModels
{
    public class SupplyVM : ViewModelBase
    {
        private RelayCommand _editCommand;

        public RelayCommand EditCommand =>
            _editCommand ?? (_editCommand = new RelayCommand(arg => EditSupply(), arg => UserData.UserData.Instance.User.HasPermission(Permissions.Permission.ShowSupply)));

        private readonly Supply _supply;

        public int ID => _supply.ID;
        public DateTime Date => _supply.Date;
        public Supplier Supplier => _supply.Supplier;
        public bool IsConducted => _supply.IsConduct;
        public decimal Sum => _supply.Supply_Material.Sum(e => e.Count * e.Cost);

        public SupplyVM(Supply supply) =>
            _supply = supply;

        private void EditSupply()
        {
            EditDocumentDialogWindow editDialog = new EditDocumentDialogWindow(new EditSupplyVM(_supply));
            editDialog.ShowDialog();

            OnPropertyChanged(nameof(ID));
            OnPropertyChanged(nameof(Date));
            OnPropertyChanged(nameof(Supplier));
            OnPropertyChanged(nameof(Sum));
            OnPropertyChanged(nameof(IsConducted));
        }

        public void Delete() => _supply.Delete();
    }
}
