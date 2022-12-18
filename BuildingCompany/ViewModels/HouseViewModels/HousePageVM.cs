using BuildingCompany.Connection;
using BuildingCompany.Utilities;
using BuildingCompany.Windows.EditDialogWindow;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace BuildingCompany.ViewModels.HouseViewModels
{
    public class HousePageVM : ViewModelBase
    {
        public static readonly List<Sorting> Sortings = new List<Sorting>()
        {
            new Sorting("По названию", "Name"),
            new Sorting("По площади", "Area"),
            new Sorting("По цене", "Cost")
        };

        private string _searchText = "";
        private Sorting _sorting = Sortings[0];
        private bool _isAscending = true;

        private RelayCommand _addHouseCommand;
        private RelayCommand _removeHouseCommand;
        public RelayCommand AddHouseCommand =>
            _addHouseCommand ?? (_addHouseCommand = new RelayCommand(arg => AddHouse()));
        public RelayCommand RemoveHouseCommand =>
            _removeHouseCommand ?? (_removeHouseCommand = new RelayCommand(arg => RemoveHouses((arg as IList<object>).Cast<HouseVM>()), arg => arg is IList<object> list && list.Count > 0));

        public ICollectionView CollectionView { get; set; }
        public ObservableCollection<HouseVM> Houses =>
            new ObservableCollection<HouseVM>(DatabaseContext.Entities.House.Local.Where(house => !house.IsDeleted)
                                                                                  .Select(house => new HouseVM(house)));

        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                CollectionView.Refresh();
                OnPropertyChanged();
            }
        }
        public Sorting Sorting
        {
            get => _sorting;
            set
            {
                _sorting = value;
                Sort();
                OnPropertyChanged();
            }
        }
        public bool IsAscending
        {
            get => _isAscending;
            set
            {
                _isAscending = value;
                Sort();
                OnPropertyChanged();
            }
        }
        private ListSortDirection SortingDirection => IsAscending ? ListSortDirection.Ascending : ListSortDirection.Descending;

        public HousePageVM()
        {
            LoadDatabaseTables();

            InitializeCollectionView();
        }

        private void InitializeCollectionView()
        {
            CollectionView = CollectionViewSource.GetDefaultView(Houses);
            CollectionView.Filter = (arg) =>
            {
                HouseVM house = arg as HouseVM;
                return house.Name.ToLower().Trim().Contains(SearchText.ToLower().Trim());
            };
            Sort();
            CollectionView.Refresh();
            OnPropertyChanged(nameof(CollectionView));
        }

        private void AddHouse()
        {
            new EditDialogWindow(new EditHouseVM(null)).ShowDialog();
            InitializeCollectionView();
        }

        private void RemoveHouses(IEnumerable<HouseVM> houses)
        {
            var result = MessageBox.Show($"Вы действительно хотите удалить {houses.Count()} записей?", "Уведомление", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.No)
                return;

            foreach (var house in houses)
                house.Delete();

            DatabaseContext.Entities.SaveChanges();
            InitializeCollectionView();
        }

        private void Sort()
        {
            CollectionView.SortDescriptions.Clear();
            CollectionView.SortDescriptions.Add(new SortDescription()
            {
                PropertyName = Sorting.PropertyName,
                Direction = SortingDirection
            });
        }

        private void LoadDatabaseTables()
        {
            DatabaseContext.Entities.House.Load();
        }
    }
}
