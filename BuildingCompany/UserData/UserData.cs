using BuildingCompany.Connection;
using BuildingCompany.ViewModels;

namespace BuildingCompany.UserData
{
    public class UserData : ViewModelBase
    {
        #region Singleton
        private static UserData _userData;

        public static UserData Instance =>
            _userData ?? (_userData = new UserData());
        #endregion

        private UserData() { }
        private User _user;

        public User User
        {
            get => _user;
            set
            {
                _user = value;
                OnPropertyChanged();
            }
        }
    }
}
