using BuildingCompany.Connection;
using BuildingCompany.Utilities;
using BuildingCompany.ViewModels.SupplierViewModels;
using BuildingCompany.Windows.EditDialogWindow;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows;
using System.Data.Entity;
using System.Linq;
using BuildingCompany.Windows.EditDocumentDialog;

namespace BuildingCompany.ViewModels.SupplyViewModels
{
    public class SupplyPageVM : ViewModelBase
    {
        public static readonly List<Sorting> Sortings = new List<Sorting>()
        {
            new Sorting("По номеру", "ID"),
            new Sorting("По дате", "Date"),
            new Sorting("По поставщику", "Supplier.Name")
        };
        public static readonly List<Filter<SupplyVM>> Filters =
            DatabaseContext.Entities.Supplier.Local.Where(s => !s.IsDeleted)
                                                   .Select(s => new Filter<SupplyVM>(s.Name, supply => supply.Supplier == s))
                                                   .Prepend(new Filter<SupplyVM>("Все", supply => true))
                                                   .Append(new Filter<SupplyVM>("Проведён", supply => supply.IsConducted))
                                                   .Append(new Filter<SupplyVM>("Не проведён", supply => !supply.IsConducted))
                                                   .ToList();

        private string _searchText = "";
        private Sorting _sorting = Sortings[0];
        private Filter<SupplyVM> _filter = Filters[0];
        private bool _isAscending = true;

        private RelayCommand _addSupplyCommand;
        private RelayCommand _removeSupplyCommand;
        public RelayCommand AddSupplierCommand =>
            _addSupplyCommand ?? (_addSupplyCommand = new RelayCommand(arg => AddSupply()));
        public RelayCommand RemoveSupplyCommand =>
            _removeSupplyCommand ?? (_removeSupplyCommand = new RelayCommand(arg => RemoveSupply((arg as IList<object>).Cast<SupplyVM>()), arg => arg is IList<object> list && list.Count > 0));

        public ICollectionView CollectionView { get; set; }
        public ObservableCollection<SupplyVM> Supplies =>
            new ObservableCollection<SupplyVM>(DatabaseContext.Entities.Supply.Local.Where(e => !e.IsDeleted).Select(supply => new SupplyVM(supply)));

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
        public Filter<SupplyVM> Filter
        {
            get => _filter;
            set
            {
                _filter = value;
                OnPropertyChanged();
                CollectionView.Refresh();
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

        static SupplyPageVM() => LoadDatabaseTables();

        public SupplyPageVM()
        {
            InitializeCollectionView();
        }

        private void InitializeCollectionView()
        {
            CollectionView = CollectionViewSource.GetDefaultView(Supplies);
            CollectionView.Filter = (arg) =>
            {
                SupplyVM supply = arg as SupplyVM;
                return supply.Supplier.Name.ToLower().Trim().Contains(SearchText.ToLower().Trim()) &&
                       Filter.Predicate(supply);
            };
            Sort();
            CollectionView.Refresh();
            OnPropertyChanged(nameof(CollectionView));
        }

        private void AddSupply()
        {
            new EditDocumentDialogWindow(new EditSupplyVM(null)).ShowDialog();
            InitializeCollectionView();
        }

        private void RemoveSupply(IEnumerable<SupplyVM> supplies)
        {
            var result = MessageBox.Show($"Вы действительно хотите удалить {supplies.Count()} записей?", "Уведомление", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.No)
                return;

            foreach (var supply in supplies)
                supply.Delete();

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

        private static void LoadDatabaseTables()
        {
            DatabaseContext.Entities.Supply.Load();
            DatabaseContext.Entities.Supplier.Load();
        }
    }
}
