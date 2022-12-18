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

namespace BuildingCompany.ViewModels.MaterialViewModels
{
    public class MaterialPageVM : ViewModelBase
    {
        public static readonly List<Sorting> Sortings = new List<Sorting>()
        {
            new Sorting("По индентификатору", "ID"),
            new Sorting("По названию", "Name"),
            new Sorting("По цене", "Cost"),
            new Sorting("По количеству", "Count")
        };

        private string _searchText = "";
        private Sorting _sorting = Sortings[0];
        private bool _isAscending = true;

        private RelayCommand _addMaterialCommand;
        private RelayCommand _removeMaterialCommand;
        public RelayCommand AddMaterialCommand =>
            _addMaterialCommand ?? (_addMaterialCommand = new RelayCommand(arg => AddMaterial()));
        public RelayCommand RemoveMaterialCommand =>
            _removeMaterialCommand ?? (_removeMaterialCommand = new RelayCommand(arg => RemoveMaterials((arg as IList<object>).Cast<MaterialVM>()), arg => arg is IList<object> list && list.Count > 0));

        public ICollectionView CollectionView { get; set; }
        public ObservableCollection<MaterialVM> Materials =>
            new ObservableCollection<MaterialVM>(DatabaseContext.Entities.Material.Local.Where(material => !material.IsDeleted)
                                                                                  .Select(material => new MaterialVM(material)));

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

        public MaterialPageVM()
        {
            LoadDatabaseTables();

            InitializeCollectionView();
        }

        private void InitializeCollectionView()
        {
            CollectionView = CollectionViewSource.GetDefaultView(Materials);
            CollectionView.Filter = (arg) =>
            {
                MaterialVM house = arg as MaterialVM;
                return house.Name.ToLower().Trim().Contains(SearchText.ToLower().Trim());
            };
            Sort();
            CollectionView.Refresh();
            OnPropertyChanged(nameof(CollectionView));
        }

        private void AddMaterial()
        {
            new EditDialogWindow(new EditMaterialVM(null)).ShowDialog();
            InitializeCollectionView();
        }

        private void RemoveMaterials(IEnumerable<MaterialVM> materials)
        {
            var result = MessageBox.Show($"Вы действительно хотите удалить {materials.Count()} записей?", "Уведомление", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.No)
                return;

            foreach (var material in materials)
                material.Delete();

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
            DatabaseContext.Entities.Material.Load();
        }
    }
}
