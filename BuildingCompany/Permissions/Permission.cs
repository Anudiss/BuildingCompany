using BuildingCompany.Connection;
using System.Collections.Generic;

namespace BuildingCompany.Permissions
{
    public static class AllowPermissions
    {
        public static readonly Dictionary<UserRole, Permission[]> AllowRolePermissions = new Dictionary<UserRole, Permission[]>()
        {

        };

        public static readonly Dictionary<EmployeePosition, Permission[]> AllowPositionPermissions = new Dictionary<EmployeePosition, Permission[]>()
        {

        };
    }

    public enum Permission
    {
    }
}
