using BuildingCompany.Connection;
using BuildingCompany.Utilities;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace BuildingCompany.ViewModels.OrderViewModels
{
    public class EditOrderVM : WindowViewModelBase
    {
        #region Commands
        private RelayCommand _acceptCommand;
        private RelayCommand _denyCommand;
        private RelayCommand _payCommand;
        private RelayCommand _beginExecutionCommand;
        private RelayCommand _finishCommand;

        public override RelayCommand CloseCommand =>
            _closeCommand ?? (_closeCommand = new RelayCommand(arg => Close()));

        public override RelayCommand CancelCommand =>
            _cancelCommand ?? (_cancelCommand = new RelayCommand(arg => Cancel()));

        public override RelayCommand SaveCommand =>
            _saveCommand ?? (_saveCommand = new RelayCommand(arg => Save()));
        public RelayCommand AcceptCommand =>
            _acceptCommand ?? (_acceptCommand = new RelayCommand(arg => AcceptOrder(), arg => _order.Employee?.Position == Positions.Manager));
        public RelayCommand DenyCommand =>
            _denyCommand ?? (_denyCommand = new RelayCommand(arg => DenyOrder(), arg => _order.Employee?.Position == Positions.Manager));
        public RelayCommand PayCommand =>
            _payCommand ?? (_payCommand = new RelayCommand(arg => PayOrder()));
        public RelayCommand BeginExecutionCommand =>
            _beginExecutionCommand ?? (_beginExecutionCommand = new RelayCommand(arg => BeginExecution()));
        public RelayCommand FinishCommand =>
            _finishCommand ?? (_finishCommand = new RelayCommand(arg => Finish(), arg => _order.Employee?.Position == Positions.Storekeeper || _order.Employee?.Position == Positions.Manager));
        #endregion

        public static readonly List<Client> Clients = DatabaseContext.Entities.Client.Local.Where(client => !client.IsDeleted).ToList();
        public static readonly List<Employee> Executors = DatabaseContext.Entities.Employee.Local.Where(employee => !employee.IsDeleted && employee.Position == Positions.Manager).ToList();
        public static readonly List<House> Houses = DatabaseContext.Entities.House.Local.Where(house => !house.IsDeleted).ToList();

        private readonly Order _order;
        
        public Client Client
        {
            get => _order.Client;
            set
            {
                _order.Client = value;
                OnPropertyChanged();
            }
        }
        public Employee Executor
        {
            get => _order.Employee;
            set
            {
                _order.Employee = value;
                OnPropertyChanged();
            }
        }      
        public House House
        {
            get => _order.House;
            set
            {
                _order.House = value;
                OnPropertyChanged();
            }
        }
        public Stage Stage
        {
            get => _order.Stage;
            set
            {
                _order.Stage = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsProcessing));
            }
        }

        public bool IsEnabled => Stage == Stages.New;
        public bool IsProcessing => Stage == Stages.Processing;
        public bool IsShouldToPay => Stage == Stages.ToPay;
        public bool IsPaid => Stage == Stages.Paid;
        public bool IsExecution => Stage == Stages.Building;
        public bool IsNew => _order.ID == 0;

        public EditOrderVM(Order order)
        {
            _order = order ?? new Order()
            {
                Client = UserData.UserData.Instance.User.UserClient,
                Employee = UserData.UserData.Instance.User.Manager,
                Stage = Stages.New,
                IsDeleted = false
            };

            if (Stage == Stages.New && UserData.UserData.Instance.User.UserEmployee?.Position == Positions.Manager && !IsNew)
            {
                _order.Employee = UserData.UserData.Instance.User.UserEmployee;
                _order.Stage = Stages.Processing;
            }

            if (_order.Client?.IsDeleted == true)
                Clients.Add(_order.Client);

            if (_order.Employee?.IsDeleted == true)
                Executors.Add(_order.Employee);

            OnPropertyChanged(nameof(Clients));
            OnPropertyChanged(nameof(Executors));
            OnPropertyChanged(nameof(Houses));
            OnPropertyChanged(nameof(IsEnabled));
            OnPropertyChanged(nameof(IsProcessing));
        }

        private void AcceptOrder()
        {
            Stage = Stages.ToPay;
            SaveAndClose();
        }

        private void DenyOrder()
        {
            Stage = Stages.Denied;
            SaveAndClose();
        }

        private void PayOrder()
        {
            Stage = Stages.Paid;
            SaveAndClose();
        }

        private void BeginExecution()
        {
            if (House.House_Material.Any(houseMaterial => houseMaterial.Count > houseMaterial.Material.Count))
            {
                MessageBox.Show("Недостаточно материалов");
                return;
            }

            foreach (var houseMaterial in House.House_Material)
                houseMaterial.Material.Count -= houseMaterial.Count;

            Stage = Stages.Building;
            SaveAndClose();
        }

        private void Finish()
        {
            Stage = Stages.Done;
            SaveAndClose();
        }

        private void SaveAndClose()
        {
            Save();
            Close();
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
            if (Client == null || House == null)
            {
                MessageBox.Show("Не все поля заполены");
                return;
            }

            if (IsProcessing)
                Stage = Stages.New;

            if (IsNew)
                DatabaseContext.Entities.Order.Local.Add(_order);

            DatabaseContext.Entities.SaveChanges();
        }
    }
}
