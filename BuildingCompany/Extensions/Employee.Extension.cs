namespace BuildingCompany.Connection
{
    public partial class Employee
    {
        public string FullName => $"{Surname} {Name[0]}. {Patronymic[0]}.";

        public Position Position
        {
            get => Positions.AllPositions[Position_id];
            set => Position_id = Positions.AllPositions.IndexOf(value);
        }

        public Gender Gender
        {
            get => Genders.AllGenders[Gender_id];
            set => Gender_id = Genders.AllGenders.IndexOf(value);
        }
    }
}
