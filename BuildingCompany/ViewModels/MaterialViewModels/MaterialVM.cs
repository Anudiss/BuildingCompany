using BuildingCompany.Connection;
using BuildingCompany.Utilities;
using BuildingCompany.Windows.EditDialogWindow;

namespace BuildingCompany.ViewModels.MaterialViewModels
{
    public class MaterialVM : ViewModelBase
    {
        private RelayCommand _editCommand;

        public RelayCommand EditCommand =>
            _editCommand ?? (_editCommand = new RelayCommand(arg => EditMaterial(), arg => UserData.UserData.Instance.User.HasPermission(Permissions.Permission.ShowMaterial)));

        private readonly Material _material;

        public int ID => _material.ID;
        public string Name => _material.Name;
        public decimal Cost => _material.Cost;
        public decimal Count => _material.Count;
        public bool IsDeleted => _material.IsDeleted;

        public MaterialVM(Material material) =>
            _material = material;

        private void EditMaterial()
        {
            EditDialogWindow editDialog = new EditDialogWindow(new EditMaterialVM(_material));
            editDialog.ShowDialog();

            OnPropertyChanged(nameof(ID));
            OnPropertyChanged(nameof(Name));
            OnPropertyChanged(nameof(Cost));
            OnPropertyChanged(nameof(Count));
        }

        public void Delete() => _material.IsDeleted = true;
    }
}
