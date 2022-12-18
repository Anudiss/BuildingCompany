using BuildingCompany.Connection;
using System;

namespace BuildingCompany.ViewModels.HouseViewModels
{
    public class MaterialGridRowVM : ViewModelBase
    {
        public House_Material HouseMaterial;

        public string Name
        {
            get => HouseMaterial.Material.Name;
        }
        public decimal Cost
        {
            get => HouseMaterial.Material.Cost;
        }
        public decimal Count
        {
            get => HouseMaterial.Count;
            set
            {
                if (value < 1)
                    throw new ArgumentException("Количество не может быть меньше 1");
                HouseMaterial.Count = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Sum));
            }
        }
        public decimal Sum => Cost * Count;

        public MaterialGridRowVM(House_Material houseMaterial) =>
            HouseMaterial = houseMaterial;
    }
}
