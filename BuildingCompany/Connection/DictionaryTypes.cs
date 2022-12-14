namespace BuildingCompany.Connection
{
    public enum EmployeePosition
    {
        Worker = 1,
        Storekeeper,
        Manager,
        HR
    }

    public enum UserRole
    {
        Client = 1,
        Admin,
        Employee
    }

    public enum EmployeeGender
    {
        Man = 1,
        Woman
    }

    public enum Stage
    {
        New = 1,
        Processing,
        ToPay,
        Paid,
        Building,
        Done
    }
}
