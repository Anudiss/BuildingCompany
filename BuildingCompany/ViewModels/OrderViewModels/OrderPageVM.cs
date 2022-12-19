using BuildingCompany.Connection;
using BuildingCompany.Utilities;
using BuildingCompany.ViewModels.MaterialViewModels;
using BuildingCompany.Windows.EditDialogWindow;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace BuildingCompany.ViewModels.OrderViewModels
{
    public class OrderPageVM : ViewModelBase
    {
        public static readonly List<Sorting> Sortings = new List<Sorting>()
        {
            new Sorting("По индентификатору", "ID"),
            new Sorting("По клиенту", "Client.FullName"),
            new Sorting("По ответственному", "Executor.FullName"),
            new Sorting("По дому", "House.Name")
        };
        public static readonly List<Filter<OrderVM>> Filters = Stages.AllStages.Select(s => new Filter<OrderVM>(s.Name, order => order.Stage == s))
                                                                               .Prepend(new Filter<OrderVM>("Все", order => true))
                                                                               .ToList();

        private string _searchText = "";
        private Sorting _sorting = Sortings[0];
        private Filter<OrderVM> _filter = Filters[0];
        private bool _isAscending = true;

        private RelayCommand _addOrderCommand;
        private RelayCommand _removeOrderCommand;
        public RelayCommand AddOrderCommand =>
            _addOrderCommand ?? (_addOrderCommand = new RelayCommand(arg => AddOrder()));
        public RelayCommand RemoveOrderCommand =>
            _removeOrderCommand ?? (_removeOrderCommand = new RelayCommand(arg => RemoveOrder((arg as IList<object>).Cast<OrderVM>()), arg => arg is IList<object> list && list.Count > 0));

        public ICollectionView CollectionView { get; set; }
        public ObservableCollection<OrderVM> Orders =>
            new ObservableCollection<OrderVM>(DatabaseContext.Entities.Order.Local.Where(order => !order.IsDeleted)
                                                                                  .Where(order => order.Client == UserData.UserData.Instance.User.UserClient || (order.Employee == UserData.UserData.Instance.User.UserEmployee || order.Employee == null) || (UserData.UserData.Instance.User.UserEmployee?.Position == Positions.Storekeeper && (order.Stage == Stages.Building || order.Stage == Stages.Paid)))
                                                                                  .Select(order => new OrderVM(order)));

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
        public Filter<OrderVM> Filter
        {
            get => _filter;
            set
            {
                _filter = value;
                CollectionView.Refresh();
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

        public OrderPageVM()
        {
            LoadDatabaseTables();

            InitializeCollectionView();
        }

        private void InitializeCollectionView()
        {
            CollectionView = CollectionViewSource.GetDefaultView(Orders);
            CollectionView.Filter = (arg) =>
            {
                OrderVM order = arg as OrderVM;
                return ($"{order.Client.Surname}{order.Client.Name}{order.Client.Patronymic}".ToLower().Trim().Contains(SearchText.ToLower().Trim()) ||
                        $"{order.Executor.Surname}{order.Executor.Name}{order.Executor.Patronymic}".ToLower().Trim().Contains(SearchText.ToLower().Trim())) &&
                        Filter.Predicate(order);
            };
            Sort();
            CollectionView.Refresh();
            OnPropertyChanged(nameof(CollectionView));
        }

        private void AddOrder()
        {
            new EditDialogWindow(new EditOrderVM(null)).ShowDialog();
            InitializeCollectionView();
        }

        private void RemoveOrder(IEnumerable<OrderVM> orders)
        {
            var result = MessageBox.Show($"Вы действительно хотите удалить {orders.Count()} записей?", "Уведомление", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.No)
                return;

            foreach (var order in orders)
                order.Delete();

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
            DatabaseContext.Entities.Order.Load();
        }
    }
}
