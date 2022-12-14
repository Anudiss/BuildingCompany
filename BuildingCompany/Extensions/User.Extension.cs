using BuildingCompany.Permissions;
using System.Linq;

namespace BuildingCompany.Connection
{
    public partial class User
    {
        public UserRole UserRole
        {
            get => (UserRole)Role_ID;
            set => Role_ID = (int)value;
        }

        public Employee UserEmployee => Employee.FirstOrDefault();
        public Client UserClient => Client.FirstOrDefault();

        public bool HasPermission(Permission permission) =>
            (AllowPermissions.AllowRolePermissions.ContainsKey(UserRole) && AllowPermissions.AllowRolePermissions[UserRole].Contains(permission)) ||
            (UserEmployee != null && AllowPermissions.AllowPositionPermissions.ContainsKey(UserEmployee.EmployeePosition) && AllowPermissions.AllowPositionPermissions[UserEmployee.EmployeePosition].Contains(permission));
    }
}
