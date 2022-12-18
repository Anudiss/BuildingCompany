namespace BuildingCompany.Connection
{
    public partial class House
    {
        public void Delete()
        {
            IsDeleted = true;
            foreach (var material in House_Material)
                material.Delete();
        }
    }
}
