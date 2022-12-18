namespace BuildingCompany.Connection
{
    public partial class Supply
    {
        public void Delete()
        {
            IsDeleted = true;

            foreach (var material in Supply_Material)
                material.Delete();
        }
    }
}
