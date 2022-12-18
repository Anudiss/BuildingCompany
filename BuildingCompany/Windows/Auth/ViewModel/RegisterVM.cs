using BuildingCompany.Connection;
using BuildingCompany.Utilities;
using BuildingCompany.ViewModels;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace BuildingCompany.Windows.Auth.ViewModel
{
    public class RegisterVM : ViewModelBase
    {
        private RelayCommand _registerCommand;

        public RelayCommand RegisterCommand =>
            _registerCommand ?? (_registerCommand = new RelayCommand(arg => TryRegisterUser()));


        private string _surname;
        private string _name;
        private string _patronymic;
        private string _phone;
        private string _login;
        private string _password;
        private string _repeatPassword;

        public string Surname
        {
            get => _surname;
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new ArgumentException("Поле фамилия обязательно к запонению");
                if (Regex.IsMatch(value.Trim(), @"[^\w\d \-]"))
                    throw new ArgumentException("Поле содержит недопустимые символы");
                _surname = value.Trim();
                OnPropertyChanged();
            }
        }
        public string Name
        {
            get => _name;
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new ArgumentException("Поле имя обязательно к запонению");
                if (Regex.IsMatch(value.Trim(), @"[^\w\d \-]"))
                    throw new ArgumentException("Поле имя недопустимые символы");
                _name = value;
                OnPropertyChanged();
            }
        }
        public string Patronymic
        {
            get => _patronymic;
            set
            {
                if (Regex.IsMatch(value.Trim(), @"[^\w\d \-]"))
                    throw new ArgumentException("Поле содержит недопустимые символы");
                _patronymic = value;
                OnPropertyChanged();
            }
        }
        public string Phone
        {
            get => _phone;
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new ArgumentException("Поле телефон не может быть пустым");
                if (!Regex.IsMatch(value.Trim(), @"^(\+?\d)(\d{3})(\d{3})(\d{2})(\d{2})"))
                    throw new ArgumentException("Неверный формат телефона");
                _phone = value.Trim();
                OnPropertyChanged();
            }
        }
        public string Login
        {
            get => _login;
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new ArgumentException("Поле логин обязательно к заполнению");
                if (Regex.IsMatch(value.Trim(), @" "))
                    throw new ArgumentException("Поле содержит недопустимые символы");
                _login = value.Trim();
                OnPropertyChanged();
            }
        }
        public string Password
        {
            get => _password;
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new ArgumentException("Поле пароль не может быть пустым");
                if (Regex.IsMatch(value.Trim(), @" "))
                    throw new ArgumentException("Поле содержит недопустимые символы");
                _password = value;
                OnPropertyChanged();
            }
        }
        public string RepeatPassword
        {
            get => _repeatPassword;
            set
            {
                if (value.Trim() != Password)
                    throw new ArgumentException("Пароли не совпадают");
                _repeatPassword = value;
                OnPropertyChanged();
            }
        }

        private bool HasSameLogin => DatabaseContext.Entities.User.Local.Any(user => user.Login == Login);

        private void TryRegisterUser()
        {
            if (HasSameLogin)
            {
                /* Ошибка, чел с таким логином уже существует */

                return;
            }

            RegisterUser();
        }

        private void RegisterUser()
        {
            User user = new User()
            {
                Login = Login,
                Password = Password,
                Role = Roles.Client
            };

            Client client = new Client()
            {
                Surname = Surname,
                Name = Name,
                Patronymic = Patronymic,
                Phone = Phone,
                User = user
            };

            DatabaseContext.Entities.User.Local.Add(user);
            DatabaseContext.Entities.Client.Local.Add(client);

            DatabaseContext.Entities.SaveChanges();

            UserData.UserData.Instance.User = user;
            OpenMainWindow();
        }

        private void OpenMainWindow()
        {
            new MainWindow().Show();
            AuthNavigationVM.Instance.Close?.Invoke();
        }
    }
}
