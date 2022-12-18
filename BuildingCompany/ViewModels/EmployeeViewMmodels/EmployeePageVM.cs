using BuildingCompany.Connection;
using BuildingCompany.Utilities;
using BuildingCompany.ViewModels.SupplierViewModels;
using BuildingCompany.Windows.EditDialogWindow;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows;
using System.Linq;
using System.Data.Entity;

namespace BuildingCompany.ViewModels.EmployeeViewMmodels
{
    public class EmployeePageVM : ViewModelBase
    {
        public static readonly List<Sorting> Sortings = new List<Sorting>()
        {
            new Sorting("По табельному номеру", "ID"),
            new Sorting("По фамилии", "Surname"),
            new Sorting("По имени", "Name"),
            new Sorting("По отчеству", "Patronymic")
        };
        public static readonly List<Filter<EmployeeVM>> Filters = Positions.AllPositions
            .Select(position => new Filter<EmployeeVM>(position.Name, arg => arg.Position == position))
            .Prepend(new Filter<EmployeeVM>("Все", arg => true))
            .ToList();

        private string _searchText = "";
        private Sorting _sorting = Sortings[0];
        private Filter<EmployeeVM> _filter = Filters[0];
        private bool _isAscending = true;

        private RelayCommand _addEmployeeCommand;
        private RelayCommand _removeEmployeeCommand;
        public RelayCommand AddEmployeeCommand =>
            _addEmployeeCommand ?? (_addEmployeeCommand = new RelayCommand(arg => AddEmployee()));
        public RelayCommand RemoveEmployeeCommand =>
            _removeEmployeeCommand ?? (_removeEmployeeCommand = new RelayCommand(arg => RemoveEmployee((arg as IList<object>).Cast<EmployeeVM>()), arg => arg is IList<object> list && list.Count > 0));

        public ICollectionView CollectionView { get; set; }
        public ObservableCollection<EmployeeVM> Employees =>
            new ObservableCollection<EmployeeVM>(DatabaseContext.Entities.Employee.Local.Where(employee => !employee.IsDeleted)
                                                                                        .Select(employee => new EmployeeVM(employee)));

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
        public Filter<EmployeeVM> Filter
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

        public EmployeePageVM()
        {
            LoadDatabaseTables();

            InitializeCollectionView();
        }

        private void InitializeCollectionView()
        {
            CollectionView = CollectionViewSource.GetDefaultView(Employees);
            CollectionView.Filter = (arg) =>
            {
                EmployeeVM employee = arg as EmployeeVM;
                return $"{employee.Surname}{employee.Name}{employee.Patronymic}".ToLower().Trim().Contains(SearchText.ToLower().Trim()) &&
                       Filter.Predicate(employee);
            };
            Sort();
            CollectionView.Refresh();
            OnPropertyChanged(nameof(CollectionView));
        }

        private void AddEmployee()
        {
            new EditDialogWindow(new EditEmployeeVM(null)).ShowDialog();
            InitializeCollectionView();
        }

        private void RemoveEmployee(IEnumerable<EmployeeVM> employees)
        {
            var result = MessageBox.Show($"Вы действительно хотите удалить {employees.Count()} записей?", "Уведомление", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.No)
                return;

            foreach (var employee in employees)
                employee.Delete();

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
            DatabaseContext.Entities.Employee.Load();
        }
    }
}
