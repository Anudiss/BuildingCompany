using BuildingCompany.Connection;
using BuildingCompany.Utilities;
using System;
using System.Linq;
using System.Windows;

namespace BuildingCompany.ViewModels.ClientViewModels
{
    public class EditClientVM : WindowViewModelBase
    {
        #region Commands
        public override RelayCommand CloseCommand =>
            _closeCommand ?? (_closeCommand = new RelayCommand(arg => Close()));

        public override RelayCommand CancelCommand =>
            _cancelCommand ?? (_cancelCommand = new RelayCommand(arg => Cancel()));

        public override RelayCommand SaveCommand =>
            _saveCommand ?? (_saveCommand = new RelayCommand(arg => Save()));
        #endregion

        private readonly Client _client;
        private User _user;

        public string Surname
        {
            get => _client.Surname;
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new ArgumentException("Фамилия не может быть пустой");
                if (value.Trim().Length > 50)
                    throw new ArgumentOutOfRangeException("Фамилия не может быть больше 50 символов");

                _client.Surname = value;
                OnPropertyChanged();
            }
        }
        public string Name
        {
            get => _client.Name;
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new ArgumentException("Имя не может быть пустой");
                if (value.Trim().Length > 50)
                    throw new ArgumentOutOfRangeException("Имя не может быть больше 50 символов");

                _client.Name = value;
                OnPropertyChanged();
            }
        }
        public string Patronymic
        {
            get => _client.Patronymic;
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new ArgumentException("Отчество не может быть пустой");
                if (value.Trim().Length > 50)
                    throw new ArgumentOutOfRangeException("Отчество не может быть больше 50 символов");

                _client.Patronymic = value;
                OnPropertyChanged();
            }
        }
        public string Phone
        {
            get => _client.Phone;
            set
            {
                _client.Phone = value;
                OnPropertyChanged();
            }
        }
        public string Login
        {
            get => _user?.Login;
            set
            {
                _user.Login = value.Trim();
                OnPropertyChanged();
            }
        }
        public string Password
        {
            get => _user?.Password;
            set
            {
                _user.Password = value.Trim();
                OnPropertyChanged();
            }
        }
        public bool IsNew => _client.ID == 0;

        public bool IsEnabled => UserData.UserData.Instance.User.HasPermission(Permissions.Permission.EditClient);

        public EditClientVM(Client client)
        {
            _user = client?.User ?? new User()
            {
                Role = Roles.Client
            };
            _client = client ?? new Client()
            {
                User = _user,
                IsDeleted = false
            };
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
            OnPropertyChanged(nameof(Surname));
            OnPropertyChanged(nameof(Name));
            OnPropertyChanged(nameof(Patronymic));
            OnPropertyChanged(nameof(Phone));
            OnPropertyChanged(nameof(Login));
            OnPropertyChanged(nameof(Password));
        }

        public void Save()
        {
            if (ValidateInputFields())
            {
                MessageBox.Show("Не все поля заполены");
                return;
            }

            if (IsNew)
                DatabaseContext.Entities.Client.Local.Add(_client);

            if (_user.ID == 0)
            {
                if (HasSameLogin())
                {
                    MessageBox.Show("Пользователь с таким логином уже существует");
                    return;
                }

                DatabaseContext.Entities.User.Local.Add(_user);
            }

            DatabaseContext.Entities.SaveChanges();
        }

        private bool ValidateInputFields() =>
            string.IsNullOrEmpty(Surname) || string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(Patronymic) ||
           (string.IsNullOrEmpty(Phone) && (string.IsNullOrEmpty(Login) || string.IsNullOrEmpty(Password)));

        private bool HasSameLogin() =>
            DatabaseContext.Entities.User.Any(user => user.Login == Login);
    }
}
