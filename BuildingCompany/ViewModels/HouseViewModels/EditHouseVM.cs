﻿using BuildingCompany.Connection;
using BuildingCompany.Utilities;
using BuildingCompany.ViewModels.MaterialViewModels;
using BuildingCompany.Windows.SelectorDialog;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text.RegularExpressions;
using System.Windows;

namespace BuildingCompany.ViewModels.HouseViewModels
{
    public class EditHouseVM : WindowViewModelBase
    {
        #region Commands
        private RelayCommand _addMaterialsCommand;
        private RelayCommand _removeMaterialsCommand;

        public override RelayCommand CloseCommand =>
            _closeCommand ?? (_closeCommand = new RelayCommand((arg) => Close()));
        public override RelayCommand CancelCommand =>
            _cancelCommand ?? (_cancelCommand = new RelayCommand((arg) => Cancel()));
        public override RelayCommand SaveCommand =>
            _saveCommand ?? (_saveCommand = new RelayCommand((arg) => Save()));
        public RelayCommand AddMaterialsCommand =>
            _addMaterialsCommand ?? (_addMaterialsCommand = new RelayCommand((arg) => AddMaterials()));
        public RelayCommand RemoveMaterialsCommand =>
            _removeMaterialsCommand ?? (_removeMaterialsCommand = new RelayCommand((arg) => RemoveMaterials((arg as IList<object>).Cast<MaterialGridRowVM>())));
        #endregion

        private House _house;

        public string Name
        {
            get => _house.Name;
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new ArgumentException("Поле название не может быть пустым");
                if (Regex.IsMatch(value, @"[^\w\-\,\.\d ]"))
                    throw new ArgumentException("Поле название содержит недопустимые символы");
                if (value.Length > 100)
                    throw new ArgumentException("Поле название не может быть больше 100 символов");

                _house.Name = value;
                OnPropertyChanged();
            }
        }
        public byte[] Photo
        {
            get => _house.Photo ?? SystemImage.GetImageByName("noimage");
            set
            {
                _house.Photo = value;
                OnPropertyChanged();
            }
        }
        public decimal Area
        {
            get => _house.Area;
            set
            {
                if (value < 0)
                    throw new ArgumentException("Площадь не может быть отрицательной");
                if (value > 9999)
                    throw new ArgumentException("Слишком большое значение");

                _house.Area = value;
                OnPropertyChanged();
            }
        }
        public decimal Floors
        {
            get => _house.Floors;
            set
            {
                if (value < 1)
                    throw new ArgumentException("Этажность не может быть меньше 1");
                if (value > 99)
                    throw new ArgumentException("Слишком большое значение");

                _house.Floors = value;
                OnPropertyChanged();
            }
        }
        public decimal Cost
        {
            get => _house.Cost;
            set
            {
                if (value < 0)
                    throw new ArgumentException("Цена не может быть отрицательной");
                if (value > 9999999999)
                    throw new ArgumentException("Слишком большое значение");

                _house.Cost = value;
                OnPropertyChanged();
            }
        }

        public List<MaterialGridRowVM> Materials =>
            _house.House_Material.Select(houseMaterial => new MaterialGridRowVM(houseMaterial)).ToList();

        public EditHouseVM(House house = null)
        {
            LoadDatabaseTables();
            _house = house ?? new House()
            {
                IsDeleted = false
            };
        }

        private void LoadDatabaseTables()
        {
            DatabaseContext.Entities.Material.Load();
        }

        private void AddMaterials()
        {
            SelectorDialogWindow selectorDialog = new SelectorDialogWindow(DatabaseContext.Entities.Material.Local.Except(_house.House_Material.Select(houseMaterial => houseMaterial.Material)))
            {
                DisplayMemberPath = "Name"
            };
            if (selectorDialog.ShowDialog() != true)
                return;

            foreach (var material in selectorDialog.SelectedElements.Cast<Material>())
                DatabaseContext.Entities.House_Material.Local.Add(new House_Material()
                {
                    House = _house,
                    Material = material,
                    Count = 1
                });
            OnPropertyChanged(nameof(Materials));
        }

        private void RemoveMaterials(IEnumerable<MaterialGridRowVM> houseMaterialsVM)
        {
            if (houseMaterialsVM == null || houseMaterialsVM.Count() == 0)
                return;

            foreach (var houseMaterial in houseMaterialsVM.Select(e => e.HouseMaterial))
                DatabaseContext.Entities.House_Material.Local.Remove(houseMaterial);

            OnPropertyChanged(nameof(Materials));
        }

        private void Close()
        {
            if (HasChanges())
            {
                MessageBoxResult result = MessageBox.Show("Сохранить изменения?", "Уведомление", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                switch (result)
                {
                    case MessageBoxResult.Yes:
                        Save();
                        break;
                    case MessageBoxResult.No:
                        CancelHouseChanges();
                        break;
                    case MessageBoxResult.Cancel:
                        return;
                }
            }

            CloseWindow();
        }

        private void Cancel()
        {
            if (!HasChanges())
                return;

            if (MessageBox.Show("Вы действительно хотите отменить изменения?", "Уведомление", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                return;

            CancelHouseChanges();
        }

        private static bool HasChanges() =>
            DatabaseContext.Entities.ChangeTracker.HasChanges();

        private void CancelHouseChanges()
        {
            DatabaseContext.CancelChanges();

            OnPropertyChanged(nameof(Name));
            OnPropertyChanged(nameof(Cost));
            OnPropertyChanged(nameof(Floors));
            OnPropertyChanged(nameof(Area));
            OnPropertyChanged(nameof(Photo));
            OnPropertyChanged(nameof(Materials));
        }

        private void Save()
        {
            DatabaseContext.Entities.SaveChanges();
            HousePageVM.Instance.InitializeHouseView();
        }
    }
}
