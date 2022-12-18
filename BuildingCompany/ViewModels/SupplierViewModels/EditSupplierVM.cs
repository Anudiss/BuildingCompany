using BuildingCompany.Connection;
using BuildingCompany.Utilities;
using System;
using System.Text.RegularExpressions;
using System.Windows;

namespace BuildingCompany.ViewModels.SupplierViewModels
{
    public class EditSupplierVM : WindowViewModelBase
    {
        #region Commands
        public override RelayCommand CloseCommand =>
            _closeCommand ?? (_closeCommand = new RelayCommand(arg => Close()));

        public override RelayCommand CancelCommand =>
            _cancelCommand ?? (_cancelCommand = new RelayCommand(arg => Cancel()));

        public override RelayCommand SaveCommand =>
            _saveCommand ?? (_saveCommand = new RelayCommand(arg => Save()));
        #endregion

        private readonly Supplier _supplier;

        public string Name
        {
            get => _supplier.Name;
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new ArgumentException("Название не может быть пустым");
                if (Regex.IsMatch(value.Trim(), @"[^\w\d\- ""]"))
                    throw new ArgumentException("Название содержит недопустимые символы");

                _supplier.Name = value.Trim();
                OnPropertyChanged();
            }
        }
        public bool IsNew => _supplier.ID == 0;


        public EditSupplierVM(Supplier supplier) =>
            _supplier = supplier ?? new Supplier()
            {
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
        }

        public void Save()
        {
            if (string.IsNullOrEmpty(Name))
            {
                MessageBox.Show("Не все поля заполены");
                return;
            }

            if (IsNew)
                DatabaseContext.Entities.Supplier.Local.Add(_supplier);

            DatabaseContext.Entities.SaveChanges();
        }
    }
}
