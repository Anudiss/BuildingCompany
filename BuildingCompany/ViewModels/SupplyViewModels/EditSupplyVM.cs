using BuildingCompany.Connection;
using BuildingCompany.Utilities;
using BuildingCompany.Windows.SelectorDialog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Windows;

namespace BuildingCompany.ViewModels.SupplyViewModels
{
    public class EditSupplyVM : DocumentViewModelBase
    {
        #region Commands
        private RelayCommand _addMaterialCommand;
        private RelayCommand _removeMaterialCommand;

        public override RelayCommand CloseCommand =>
            _closeCommand ?? (_closeCommand = new RelayCommand(arg => Close()));

        public override RelayCommand CancelCommand =>
            _cancelCommand ?? (_cancelCommand = new RelayCommand(arg => Cancel()));

        public override RelayCommand SaveCommand =>
            _saveCommand ?? (_saveCommand = new RelayCommand(arg => Save()));

        public RelayCommand AddMaterialCommand =>
            _addMaterialCommand ?? (_addMaterialCommand = new RelayCommand(arg => AddMaterial()));
        public RelayCommand RemoveMaterialCommand =>
            _removeMaterialCommand ?? (_removeMaterialCommand = new RelayCommand(arg => RemoveMaterial((arg as IList<object>).Cast<SupplyMaterialVM>()), arg => arg is IList<object> list && list.Count > 0));
        #endregion

        private readonly Supply _supply;

        public override int ID => _supply.ID;
        public override DateTime Date => _supply.Date;
        public Supplier Supplier
        {
            get => _supply.Supplier;
            set
            {
                _supply.Supplier = value;
                OnPropertyChanged();
            }
        }
        public decimal Sum => Materials.Sum(e => e.Sum);
        public bool IsReadOnly => !IsEnabled;
        public bool IsEnabled => UserData.UserData.Instance.User.HasPermission(Permissions.Permission.EditSupply);

        public ObservableCollection<SupplyMaterialVM> Materials =>
            new ObservableCollection<SupplyMaterialVM>(_supply.Supply_Material.Select(e =>
            {
                var supplyMaterial = new SupplyMaterialVM(e);
                supplyMaterial.PropertyChanged += (sender, arg) => OnPropertyChanged(nameof(Sum));
                return supplyMaterial;
            }));
        

        public bool IsNew { get; }

        public EditSupplyVM(Supply supply)
            : base("Документ о поставке")
        {
            _supply = supply ?? new Supply()
            {
                ID = DatabaseContext.Entities.Supply.Local.Last().ID + 1,
                Supplier = DatabaseContext.Entities.Supplier.Local.First(),
                Date = DateTime.Now
            };
            IsNew = supply == null;

            DatabaseContext.Entities.Material.Load();
        }

        private void AddMaterial()
        {
            SelectorDialogWindow selectorDialog = new SelectorDialogWindow(DatabaseContext.Entities.Material.Local.Except(_supply.Supply_Material.Select(e => e.Material)))
            {
                DisplayMemberPath = "Name"
            };
            if (selectorDialog.ShowDialog() != true)
                return;

            foreach (Material material in selectorDialog.SelectedElements.Cast<Material>())
                DatabaseContext.Entities.Supply_Material.Local.Add(new Supply_Material()
                {
                    Supply = _supply,
                    Material = material,
                    Cost = material.Cost,
                    Count = 1
                });

            OnPropertyChanged(nameof(Materials));
            OnPropertyChanged(nameof(Sum));
        }

        private void RemoveMaterial(IEnumerable<SupplyMaterialVM> materials)
        {
            if (materials == null || materials.Count() <= 0)
                return;

            foreach(Supply_Material supply_Material in materials.Select(e => e.SupplyMaterial))
                DatabaseContext.Entities.Supply_Material.Local.Remove(supply_Material);

            OnPropertyChanged(nameof(Materials));
            OnPropertyChanged(nameof(Sum));
        }

        private void Close()
        {
            if (DatabaseContext.Entities.ChangeTracker.HasChanges() || IsNew)
            {
                MessageBoxResult result = MessageBox.Show("Сохранить изменения?", "Уведомление", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                switch (result)
                {
                    case MessageBoxResult.Yes:
                        Save();
                        break;
                    case MessageBoxResult.No:
                        DatabaseContext.CancelChanges();
                        break;
                    case MessageBoxResult.Cancel:
                        return;
                }
            }

            CloseWindow();
        }

        private void Cancel()
        {
            if (!DatabaseContext.Entities.ChangeTracker.HasChanges())
                return;

            if (MessageBox.Show("Вы действительно хотите отменить изменения?", "Уведомление", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                return;

            DatabaseContext.CancelChanges();
            UpdateProperties();
        }

        private void UpdateProperties()
        {
            
        }

        public void Save()
        {
            if (IsNew)
                DatabaseContext.Entities.Supply.Local.Add(_supply);

            DatabaseContext.Entities.SaveChanges();
        }

        public override void ConductDocument()
        {
            foreach (var material in DatabaseContext.Entities.Material.Local)
                material.Count = DatabaseContext.Entities.Supply_Material.Local.Where(s => s.Material == material)
                                                                               .Sum(s => s.Count) -
                    DatabaseContext.Entities.Order.Local.Where(order => order.Stage == Stages.Building || order.Stage == Stages.Done)
                                                        .Sum(order => order.House.House_Material.Where(h => h.Material == material).Sum(houseMaterial => houseMaterial.Material.Count));
            Save();
            _supply.IsConduct = true;
        }
    }
}
