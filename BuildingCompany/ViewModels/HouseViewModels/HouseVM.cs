using BuildingCompany.Connection;
using BuildingCompany.Utilities;
using BuildingCompany.Windows.EditDialogWindow;
using System.Windows;

namespace BuildingCompany.ViewModels.HouseViewModels
{
    public class HouseVM : ViewModelBase
    {
        private RelayCommand _editCommand;
        private RelayCommand _removeCommand;

        public RelayCommand EditCommand =>
            _editCommand ?? (_editCommand = new RelayCommand((arg) => OpenEditHouseWindow()));
        public RelayCommand RemoveCommand =>
            _removeCommand ?? (_removeCommand = new RelayCommand((arg) => RemoveHouse()));

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
        }

        private void RemoveHouse()
        {
            
        }
    }
}
