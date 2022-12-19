using BuildingCompany.Connection;
using BuildingCompany.Utilities;
using BuildingCompany.Windows.EditDialogWindow;

namespace BuildingCompany.ViewModels.HouseViewModels
{
    public class HouseVM : ViewModelBase
    {
        private RelayCommand _editCommand;

        public RelayCommand EditCommand =>
            _editCommand ?? (_editCommand = new RelayCommand((arg) => OpenEditHouseWindow(), arg => UserData.UserData.Instance.User.HasPermission(Permissions.Permission.ShowHouse)));

        private readonly House _house;

        public string Name => _house.Name;
        public decimal Area => _house.Area;
        public decimal Floors => _house.Floors;
        public decimal Cost => _house.Cost;
        public byte[] Photo => _house.Photo ?? SystemImage.GetImageByName("noimage");

        public HouseVM(House house) =>
            _house = house;

        private void OpenEditHouseWindow()
        {
            EditDialogWindow editDialog = new EditDialogWindow(new EditHouseVM(_house));
            editDialog.ShowDialog();
            OnPropertyChanged(nameof(Name));
            OnPropertyChanged(nameof(Area));
            OnPropertyChanged(nameof(Floors));
            OnPropertyChanged(nameof(Cost));
            OnPropertyChanged(nameof(Photo));
        }

        public void Delete() =>
            _house.Delete();
    }
}
