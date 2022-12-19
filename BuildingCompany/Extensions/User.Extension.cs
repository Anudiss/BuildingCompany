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

        public string FullName => UserEmployee != null ? $"{UserEmployee.FullName}\n{UserEmployee.Position.Name}" :
                                  UserClient != null ? $"{UserClient.FullName}\n{Role.Name}" : Role.Name;

        public byte[] Photo => UserEmployee?.Photo ?? SystemImage.GetImageByName("user");

        public Employee UserEmployee => Employee.FirstOrDefault(e => !e.IsDeleted);
        public Client UserClient => Client.FirstOrDefault(e => !e.IsDeleted);
        public Employee Manager => UserEmployee?.Position == Positions.Manager ? UserEmployee : null;

        public bool HasPermission(Permission permission) =>
            (AllowPermissions.AllowRolePermissions.ContainsKey(Role) && AllowPermissions.AllowRolePermissions[Role].Contains(permission)) ||
            (UserEmployee != null && AllowPermissions.AllowPositionPermissions.ContainsKey(UserEmployee.Position) && AllowPermissions.AllowPositionPermissions[UserEmployee.Position].Contains(permission));
    }
}
