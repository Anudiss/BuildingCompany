namespace BuildingCompany.Connection
{
    public partial class Client
    {
        public string FullName => $"{Surname} {Name[0]}. {Patronymic[0]}.";

        public void Delete()
        {
            IsDeleted = true;
            DatabaseContext.Entities.User.Remove(User);
            User = null;
        }
    }
}
