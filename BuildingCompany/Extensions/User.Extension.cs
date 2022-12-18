using BuildingCompany.Permissions;
using System.Linq;

namespace BuildingCompany.Connection
{
    public partial class User
    {
        public Role Role
        {
            get => Roles.AllRoles[Role_ID];
            set => Role_ID = Roles.AllRoles.IndexOf(value);
        }

        public string FullName => UserEmployee == null ? UserClient.FullName : UserEmployee.FullName;

        public byte[] Photo => UserEmployee?.Photo ?? SystemImage.GetImageByName("user");

        public Employee UserEmployee => Employee.FirstOrDefault();
        public Client UserClient => Client.FirstOrDefault();

        public bool HasPermission(Permission permission) =>
            (AllowPermissions.AllowRolePermissions.ContainsKey(Role) && AllowPermissions.AllowRolePermissions[Role].Contains(permission)) ||
            (UserEmployee != null && AllowPermissions.AllowPositionPermissions.ContainsKey(UserEmployee.Position) && AllowPermissions.AllowPositionPermissions[UserEmployee.Position].Contains(permission));
    }
}
