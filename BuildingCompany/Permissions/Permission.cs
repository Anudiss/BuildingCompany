using BuildingCompany.Connection;
using System.Collections.Generic;

namespace BuildingCompany.Permissions
{
    public static class AllowPermissions
    {
        public static readonly Dictionary<Role, Permission[]> AllowRolePermissions = new Dictionary<Role, Permission[]>()
        {

        };

        public static readonly Dictionary<Position, Permission[]> AllowPositionPermissions = new Dictionary<Position, Permission[]>()
        {

        };
    }

    public enum Permission
    {
    }
}
