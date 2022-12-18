using BuildingCompany.Connection;
using BuildingCompany.Utilities;
using System;
using System.Text.RegularExpressions;
using System.Windows;

namespace BuildingCompany.ViewModels.MaterialViewModels
{
    public class EditMaterialVM : WindowViewModelBase
    {
        #region Commands
        public override RelayCommand CloseCommand =>
            _closeCommand ?? (_closeCommand = new RelayCommand(arg => Close()));

        public override RelayCommand CancelCommand =>
            _cancelCommand ?? (_cancelCommand = new RelayCommand(arg => Cancel()));

        public override RelayCommand SaveCommand =>
            _saveCommand ?? (_saveCommand = new RelayCommand(arg => Save()));
        #endregion

        private readonly Material _material;

        public string Name
        {
            get => _material.Name;
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new ArgumentException("Название не может быть пустым");
                if (Regex.IsMatch(value.Trim(), @"[^\w\d\- ""]"))
                    throw new ArgumentException("Название содержит недопустимые символы");

                _material.Name = value.Trim();
                OnPropertyChanged();
            }
        }
        public decimal Cost
        {
            get => _material.Cost;
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException("Цена не может быть меньше 0");
                if (value > 999999.99M)
                    throw new ArgumentOutOfRangeException("Слишком большое число");

                _material.Cost = value;
                OnPropertyChanged();
            }
        }
        public decimal Count => _material.Count;

        public bool IsNew => _material.ID == 0;


        public EditMaterialVM(Material material) =>
            _material = material ?? new Material()
            {
                Count = 0,
                IsDeleted = false
            };

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
            OnPropertyChanged(nameof(Name));
            OnPropertyChanged(nameof(Cost));
            OnPropertyChanged(nameof(Count));
        }

        public void Save()
        {
            if (string.IsNullOrEmpty(Name) || Cost == 0)
            {
                MessageBox.Show("Не все поля заполены");
                return;
            }

            if (IsNew)
                DatabaseContext.Entities.Material.Local.Add(_material);

            DatabaseContext.Entities.SaveChanges();
        }
    }
}
