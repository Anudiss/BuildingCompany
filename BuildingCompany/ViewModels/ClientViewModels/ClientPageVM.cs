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

namespace BuildingCompany.ViewModels.ClientViewModels
{
    public class ClientPageVM : ViewModelBase
    {
        public static readonly List<Sorting> Sortings = new List<Sorting>()
        {
            new Sorting("По номеру", "ID"),
            new Sorting("По фамилии", "Surname"),
            new Sorting("По имени", "Name"),
            new Sorting("По отчеству", "Patronymic")
        };

        private string _searchText = "";
        private Sorting _sorting = Sortings[0];
        private bool _isAscending = true;

        private RelayCommand _addClientCommand;
        private RelayCommand _removeClientCommand;
        public RelayCommand AddClientCommand =>
            _addClientCommand ?? (_addClientCommand = new RelayCommand(arg => AddClient()));
        public RelayCommand RemoveClientCommand =>
            _removeClientCommand ?? (_removeClientCommand = new RelayCommand(arg => RemoveClient((arg as IList<object>).Cast<ClientVM>()), arg => arg is IList<object> list && list.Count > 0));

        public ICollectionView CollectionView { get; set; }
        public ObservableCollection<ClientVM> Clients =>
            new ObservableCollection<ClientVM>(DatabaseContext.Entities.Client.Local.Where(client => !client.IsDeleted)
                                                                                        .Select(client => new ClientVM(client)));

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

        public ClientPageVM()
        {
            LoadDatabaseTables();

            InitializeCollectionView();
        }

        private void InitializeCollectionView()
        {
            CollectionView = CollectionViewSource.GetDefaultView(Clients);
            CollectionView.Filter = (arg) =>
            {
                ClientVM client = arg as ClientVM;
                return $"{client.Surname}{client.Name}{client.Patronymic}".ToLower().Trim().Contains(SearchText.ToLower().Trim());
            };
            Sort();
            CollectionView.Refresh();
            OnPropertyChanged(nameof(CollectionView));
        }

        private void AddClient()
        {
            new EditDialogWindow(new EditClientVM(null)).ShowDialog();
            InitializeCollectionView();
        }

        private void RemoveClient(IEnumerable<ClientVM> clients)
        {
            var result = MessageBox.Show($"Вы действительно хотите удалить {clients.Count()} записей?", "Уведомление", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.No)
                return;

            foreach (var client in clients)
                client.Delete();

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
            DatabaseContext.Entities.Client.Load();
        }
    }
}
