namespace BuildingCompany.Connection
{
    public partial class Supply_Material
    {
        public void Delete()
        {
            Material.Count -= Count;
            IsDeleted = true;
        }
    }
}
