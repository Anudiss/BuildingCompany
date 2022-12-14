using BuildingCompany.Utilities;
using BuildingCompany.ViewModels;
using BuildingCompany.ViewModels.HouseViewModels;

namespace BuildingCompany.Windows.Navigation
{
	public class NavigationVM : ViewModelBase
    {
		private object _currentView;

		private RelayCommand _houseCommand;

		public RelayCommand HouseCommand =>
			_houseCommand ?? (_houseCommand = new RelayCommand((arg) => CurrentView = new HousePageVM()));

		public object CurrentView
		{
			get => _currentView;
			set
			{
				_currentView = value;
				OnPropertyChanged();
			}
		}
	}
}
