namespace BuildingCompany.Connection
{
    public partial class Employee
    {
        public string FullName => $"{Surname} {Name[0]}. {Patronymic[0]}.";

        public EmployeePosition EmployeePosition
        {
            get => (EmployeePosition)Position_id;
            set => Position_id = (int)value;
        }

        public EmployeeGender EmployeeGender
        {
            get => (EmployeeGender)Gender_id;
            set => Gender_id = (int)value;
        }
    }
}
