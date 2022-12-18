using BuildingCompany.Connection;

namespace BuildingCompany.ViewModels.SupplyViewModels
{
    public class SupplyMaterialVM : ViewModelBase
    {
        public Supply_Material SupplyMaterial { get; }

        public Material Material
        {
            get => SupplyMaterial.Material;
            set
            {
                SupplyMaterial.Material = value;
                OnPropertyChanged();
            }
        }
        public decimal Cost
        {
            get => SupplyMaterial.Cost;
            set
            {
                SupplyMaterial.Cost = value;
                OnPropertyChanged();
            }
        }
        public decimal Count
        {
            get => SupplyMaterial.Count;
            set
            {
                SupplyMaterial.Count = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Sum));
            }
        }
        public decimal Sum => Cost * Count;

        public SupplyMaterialVM(Supply_Material supply_Material) =>
            SupplyMaterial = supply_Material;
    }
}
