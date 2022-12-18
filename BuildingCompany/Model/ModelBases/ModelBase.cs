using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BuildingCompany.Model.ModelBases
{
    public class CollectionModelBase<T> : INotifyCollectionChanged
    {
        public event NotifyCollectionChangedEventHandler CollectionChanged;
        public void OnCollectionChanged(NotifyCollectionChangedEventArgs args) =>
            CollectionChanged?.Invoke(this, args);

        public void OnItemAdded(T item) =>
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item));
        public void OnItemRemoved(T item) =>
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item));
        public void OnCollectionReset() =>
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));

    }

    public class ModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propName = "") =>
            PropertyChanged(this, new PropertyChangedEventArgs(propName));
    }
}
