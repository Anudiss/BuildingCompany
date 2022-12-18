using BuildingCompany.Connection;
using BuildingCompany.Utilities;
using System.Text.RegularExpressions;
using System.Windows;
using System;
using Microsoft.Win32;
using System.IO;
using System.Linq;

namespace BuildingCompany.ViewModels.EmployeeViewMmodels
{
    public class EditEmployeeVM : WindowViewModelBase
    {
        #region Commands
        private RelayCommand _genderCommand;
        private RelayCommand _changeImage;

        public override RelayCommand CloseCommand =>
            _closeCommand ?? (_closeCommand = new RelayCommand(arg => Close()));

        public override RelayCommand CancelCommand =>
            _cancelCommand ?? (_cancelCommand = new RelayCommand(arg => Cancel()));

        public override RelayCommand SaveCommand =>
            _saveCommand ?? (_saveCommand = new RelayCommand(arg => Save()));
        public RelayCommand GenderCommnad =>
            _genderCommand ?? (_genderCommand = new RelayCommand(arg => Gender = arg as Gender));
        public RelayCommand ChangeImageCommand =>
            _changeImage ?? (_changeImage = new RelayCommand(arg => ChangeImage()));
        #endregion

        private readonly Employee _employee;
        private User _user;

        public string Surname
        {
            get => _employee.Surname;
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new ArgumentException("Фамилия не может быть пустой");
                if (value.Trim().Length > 50)
                    throw new ArgumentOutOfRangeException("Фамилия не может быть больше 50 символов");

                _employee.Surname = value;
                OnPropertyChanged();
            }
        }
        public string Name
        {
            get => _employee.Name;
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new ArgumentException("Имя не может быть пустой");
                if (value.Trim().Length > 50)
                    throw new ArgumentOutOfRangeException("Имя не может быть больше 50 символов");

                _employee.Name = value;
                OnPropertyChanged();
            }
        }
        public string Patronymic
        {
            get => _employee.Patronymic;
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new ArgumentException("Отчество не может быть пустой");
                if (value.Trim().Length > 50)
                    throw new ArgumentOutOfRangeException("Отчество не может быть больше 50 символов");

                _employee.Patronymic = value;
                OnPropertyChanged();
            }
        }
        public DateTime BirthDay
        {
            get => _employee.BirthDay;
            set
            {
                _employee.BirthDay = value;
                OnPropertyChanged();
            }
        }
        public byte[] Photo
        {
            get => _employee.Photo ?? SystemImage.GetImageByName("user");
            set
            {
                _employee.Photo = value;
                OnPropertyChanged();
            }
        }
        public Position Position
        {
            get => _employee.Position;
            set
            {
                if (value != Positions.Worker)
                {
                    _user = _employee?.User ?? new User()
                    {
                        Role = Roles.Employee
                    };
                    _employee.User = _user;
                }
                _employee.Position = value;
                OnPropertyChanged();
            }
        }
        public Gender Gender
        {
            get => _employee.Gender;
            set
            {
                _employee.Gender = value;
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
        public bool IsNew => _employee.ID == 0;


        public EditEmployeeVM(Employee employee)
        {
            _user = employee?.User;
            _employee = employee ?? new Employee()
            {
                Position = Positions.Worker,
                BirthDay = DateTime.Now,
                User = null,
                IsDeleted = false
            };
        }

        private void ChangeImage()
        {
            if (OpenImage(out byte[] image))
                Photo = image;
        }

        private bool OpenImage(out byte[] image)
        {
            image = null;
            OpenFileDialog fileDialog = new OpenFileDialog()
            {
                Filter = "Изозражения|*.png;*.jpg;*.jpeg;*.bmp",
                DefaultExt = "Изозражения|*.png;*.jpg;*.jpeg;*.bmp",
                CheckFileExists = true,
                Multiselect = false
            };

            if (fileDialog.ShowDialog() == false)
                return false;

            image = File.ReadAllBytes(fileDialog.FileName);
            return true;
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
            OnPropertyChanged(nameof(BirthDay));
            OnPropertyChanged(nameof(Position));
            OnPropertyChanged(nameof(Gender));
            OnPropertyChanged(nameof(Photo));
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
                DatabaseContext.Entities.Employee.Local.Add(_employee);

            if (Position == Positions.Worker && _user != null)
                RemoveUser();

            if (Position != Positions.Worker && _user.ID == 0)
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
           (Position != Positions.Worker && (string.IsNullOrEmpty(Login) || string.IsNullOrEmpty(Password)));

        private bool HasSameLogin() =>
            DatabaseContext.Entities.User.Any(user => user.Login == Login);

        private void RemoveUser()
        {
            DatabaseContext.Entities.User.Local.Remove(_user);
            _user = null;
            _employee.User = null;
            OnPropertyChanged(nameof(Login));
            OnPropertyChanged(nameof(Password));
        }
    }
}
