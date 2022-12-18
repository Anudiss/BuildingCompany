using BuildingCompany.Connection;
using BuildingCompany.Properties;
using BuildingCompany.Utilities;
using BuildingCompany.ViewModels;
using System.Data.Entity;
using System.Linq;

namespace BuildingCompany.Windows.Auth.ViewModel
{
    public class LoginVM : ViewModelBase
    {
        private RelayCommand _authorizateCommand;

        private string _login;
        private string _password;
        private bool _rememberMe = true;

        public RelayCommand AuthorizateCommand =>
            _authorizateCommand ?? (_authorizateCommand = new RelayCommand(arg => TryAuthorizateUser()));
        public string Login
        {
            get => _login;
            set
            {
                _login = value.Trim();
                OnPropertyChanged();
            }
        }
        public string Password
        {
            get => _password;
            set
            {
                _password = value.Trim();
                OnPropertyChanged();
            }
        }
        public bool RememberMe
        {
            get => _rememberMe;
            set
            {
                _rememberMe = value;
                OnPropertyChanged();
            }
        }

        public LoginVM()
        {
            LoadDatabaseTables();
            TryLoadLastUser();
        }

        private static void LoadDatabaseTables()
        {
            DatabaseContext.Entities.User.Load();
        }

        private string LastLogin
        {
            get => Settings.Default.Login;
            set
            {
                Settings.Default.Login = value;
                Settings.Default.Save();
            }
        }
        private string LastPassword
        {
            get => Settings.Default.Password;
            set
            {
                Settings.Default.Password = value;
                Settings.Default.Save();
            }
        }

        #region Loading last user

        private bool HasLastUserSaved => !string.IsNullOrEmpty(LastLogin);

        private void TryLoadLastUser()
        {
            if (HasLastUserSaved)
                LoadLastUser();
        }

        private void LoadLastUser()
        {
            Login = LastLogin;
            Password = LastPassword;
        }
        #endregion
        #region Authorizate user
        private void TryAuthorizateUser()
        {
            User authorizatedUser = DatabaseContext.Entities.User.Local.FirstOrDefault(user => user.Login == Login && user.Password == Password);
            if (authorizatedUser == null)
            {
                /* Ошибка: нет такого пользователя */
                return;
            }

            AuthorizateUser(authorizatedUser);
        }

        private void AuthorizateUser(User user)
        {
            UserData.UserData.Instance.User = user;
            TryToRememberUser();

            OpenMainWindow();
        }

        private static void OpenMainWindow()
        {
            new MainWindow().Show();
            AuthNavigationVM.Instance.Close?.Invoke();
        }
        #endregion
        #region Remember user
        private void TryToRememberUser()
        {
            if (RememberMe)
                RememberUser();
        }

        private void RememberUser()
        {
            LastLogin = Login;
            LastPassword = Password;
        }
        #endregion
    }
}
