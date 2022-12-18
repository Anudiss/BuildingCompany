using BuildingCompany.ViewModels;
using System;

namespace BuildingCompany.Utilities
{
    public class Filter<T> where T : ViewModelBase
    {
        public string Name { get; }
        public Predicate<T> Predicate { get; }

        public Filter(string name, Predicate<T> predicate)
        {
            Name = name;
            Predicate = predicate;
        }
    }
}
