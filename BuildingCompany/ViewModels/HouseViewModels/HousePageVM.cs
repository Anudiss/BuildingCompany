using BuildingCompany.Connection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Windows.Data;

namespace BuildingCompany.ViewModels.HouseViewModels
{
    public class HousePageVM : ViewModelBase
    {
        public static List<Sorting> Sortings { get; }

        public static HousePageVM Instance;
        private Sorting _sorting = Sortings[0];
        private string _searchText;

        public ICollectionView HouseView { get; set; }

        public Sorting Sorting
        {
            get => _sorting;
            set
            {
                _sorting = value;
                UpdateHouseView();
                OnPropertyChanged();
            }
        }
        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                UpdateHouseView();
                OnPropertyChanged();
            }
        }

        static HousePageVM()
        {
            Sortings = new List<Sorting>()
            {
                new Sorting("По названию в алфавитном порядке", "Name", ListSortDirection.Descending),
                new Sorting("По названию не алфавитном порядке", "Name", ListSortDirection.Ascending),
                new Sorting("По возрастанию цены", "Cost", ListSortDirection.Ascending),
                new Sorting("По убыванию цены", "Cost", ListSortDirection.Descending),
                new Sorting("По возрастанию площади", "Area", ListSortDirection.Ascending),
                new Sorting("По убыванию площади", "Area", ListSortDirection.Descending)
            };
        }

        public HousePageVM()
        {
            Instance = this;
            LoadDatabaseTables();
            InitializeHouseView();
            OnPropertyChanged(nameof(Sortings));
        }

        public void InitializeHouseView()
        {
            HouseView = CollectionViewSource.GetDefaultView(DatabaseContext.Entities.House.Local.Where(e => e.IsDeleted == false)
                                                                                                         .Select(e => new HouseVM(e)));
            InitializeFilter();
            Sort();
            OnPropertyChanged(nameof(HouseView));
        }

        private void Sort()
        {
            if (HouseView == null)
                return;

            HouseView.SortDescriptions.Clear();
            HouseView.SortDescriptions.Add(Sorting.SortDescription);
        }

        private void InitializeFilter()
        {
            if (HouseView == null)
                return;

            HouseView.Filter = (arg) =>
            {
                HouseVM house = arg as HouseVM;
                return SearchText == null || house.Name.Contains(SearchText);
            };
        }

        private void UpdateHouseView()
        {
            if (HouseView == null)
                return;

            Sort();
            HouseView.Refresh();
            OnPropertyChanged(nameof(HouseView));
        }

        private static void LoadDatabaseTables()
        {
            DatabaseContext.Entities.House.Load();
        }
    }

    public class Sorting
    {
        public string Name { get; }
        public SortDescription SortDescription { get; }

        public Sorting(string name, string propertyPath, ListSortDirection direction)
        {
            Name = name;
            SortDescription = new SortDescription()
            {
                PropertyName = propertyPath,
                Direction = direction
            };
        }
    }
}
