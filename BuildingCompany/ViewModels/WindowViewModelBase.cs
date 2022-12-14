using BuildingCompany.Utilities;
using System;

namespace BuildingCompany.ViewModels
{
    public abstract class WindowViewModelBase : ViewModelBase
    {
        protected RelayCommand _closeCommand;
        protected RelayCommand _cancelCommand;
        protected RelayCommand _saveCommand;

        public abstract RelayCommand CloseCommand { get; }
        public abstract RelayCommand CancelCommand { get; }
        public abstract RelayCommand SaveCommand { get; }

        public Action WindowClose;

        public void CloseWindow() => WindowClose?.Invoke();
    }
}
