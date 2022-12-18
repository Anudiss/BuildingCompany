using BuildingCompany.Utilities;
using BuildingCompany.ViewModels;
using System;

namespace BuildingCompany.Windows.Auth.ViewModel
{
    public class AuthNavigationVM : ViewModelBase
    {
        #region SingleTon
        private static AuthNavigationVM _instance;

        public static AuthNavigationVM Instance =>
            _instance ?? (_instance = new AuthNavigationVM()
            {
                CurrentView = new LoginVM()
            });
        #endregion
        private RelayCommand _closeCommand;
        private RelayCommand _minimizeCommand;
        private RelayCommand _openLoginCommand;
        private RelayCommand _openRegisterCommand;
        private object _currentView;

        public RelayCommand CloseCommand =>
            _closeCommand ?? (_closeCommand = new RelayCommand(arg => Close?.Invoke()));
        public RelayCommand MinimizeCommand =>
            _minimizeCommand ?? (_minimizeCommand = new RelayCommand(arg => Minimize?.Invoke()));
        public RelayCommand OpenLoginCommand =>
            _openLoginCommand ?? (_openLoginCommand = new RelayCommand(arg => CurrentView = new LoginVM()));
        public RelayCommand OpenRegisterCommand =>
            _openRegisterCommand ?? (_openRegisterCommand = new RelayCommand(arg => CurrentView = new RegisterVM()));

        public Action Close;
        public Action Minimize;

        public object CurrentView
        {
            get => _currentView;
            set
            {
                _currentView = value;
                OnPropertyChanged();
            }
        }

        private AuthNavigationVM()
        {
        }
    }
}
