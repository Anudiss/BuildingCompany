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

namespace BuildingCompany.ViewModels.SupplierViewModels
{
    public class SupplierPageVM : ViewModelBase
    {
        public static readonly List<Sorting> Sortings = new List<Sorting>()
        {
            new Sorting("По индентификатору", "ID"),
            new Sorting("По названию", "Name")
        };

        private string _searchText = "";
        private Sorting _sorting = Sortings[0];
        private bool _isAscending = true;

        private RelayCommand _addSupplierCommand;
        private RelayCommand _removeSupplierCommand;
        public RelayCommand AddSupplierCommand =>
            _addSupplierCommand ?? (_addSupplierCommand = new RelayCommand(arg => AddSupplier()));
        public RelayCommand RemoveMaterialCommand =>
            _removeSupplierCommand ?? (_removeSupplierCommand = new RelayCommand(arg => RemoveSuppliers((arg as IList<object>).Cast<SupplierVM>()), arg => arg is IList<object> list && list.Count > 0));

        public ICollectionView CollectionView { get; set; }
        public ObservableCollection<SupplierVM> Suppliers =>
            new ObservableCollection<SupplierVM>(DatabaseContext.Entities.Supplier.Local.Where(supplier => !supplier.IsDeleted)
                                                                                        .Select(supplier => new SupplierVM(supplier)));

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

        public SupplierPageVM()
        {
            LoadDatabaseTables();

            InitializeCollectionView();
        }

        private void InitializeCollectionView()
        {
            CollectionView = CollectionViewSource.GetDefaultView(Suppliers);
            CollectionView.Filter = (arg) =>
            {
                SupplierVM house = arg as SupplierVM;
                return house.Name.ToLower().Trim().Contains(SearchText.ToLower().Trim());
            };
            Sort();
            CollectionView.Refresh();
            OnPropertyChanged(nameof(CollectionView));
        }

        private void AddSupplier()
        {
            new EditDialogWindow(new EditSupplierVM(null)).ShowDialog();
            InitializeCollectionView();
        }

        private void RemoveSuppliers(IEnumerable<SupplierVM> suppliers)
        {
            var result = MessageBox.Show($"Вы действительно хотите удалить {suppliers.Count()} записей?", "Уведомление", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.No)
                return;

            foreach (var supplier in suppliers)
                supplier.Delete();

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
            DatabaseContext.Entities.Supplier.Load();
        }
    }
}
