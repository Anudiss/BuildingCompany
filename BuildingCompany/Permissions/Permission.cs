using BuildingCompany.Connection;
using System.Collections.Generic;

namespace BuildingCompany.Permissions
{
    public static class AllowPermissions
    {
        public static readonly Dictionary<Role, Permission[]> AllowRolePermissions = new Dictionary<Role, Permission[]>()
        {
            { Roles.Admin, new[]
                {
                    Permission.EditHouse,
                    Permission.ShowHouse,
                    Permission.ShowHouses,
                    
                    Permission.ShowOrders,
                    Permission.ShowOrder,
                    
                    Permission.ShowSupplies,
                    Permission.ShowSupply,
                   
                    Permission.ShowSuppliers,
                    Permission.ShowSupplier,
                    Permission.EditSupplier,

                    Permission.ShowMaterials,
                    Permission.ShowMaterial,
                    Permission.EditMaterial
                }
            },
            { Roles.Client, new[]
                {
                    Permission.ShowHouses,

                    Permission.ShowOrder,
                    Permission.EditOrder,
                    Permission.ShowOrders,
                    Permission.PayOrder
                }
            }
        };

        public static readonly Dictionary<Position, Permission[]> AllowPositionPermissions = new Dictionary<Position, Permission[]>()
        {
            { Positions.Manager, new[]
                {
                    Permission.ShowOrders,
                    Permission.ShowOrder,
                    Permission.EditOrder,

                    Permission.ShowHouses,

                    Permission.ShowClients,
                    Permission.ShowClient,
                    Permission.EditOrderClient,

                    Permission.AcceptOrder,
                    Permission.DenyOrder,
                    Permission.FinishOrder
                }
            },
            { Positions.Storekeeper, new[]
                {
                    Permission.ShowMaterials,
                    Permission.ShowMaterial,
                    Permission.EditMaterial,

                    Permission.ShowSupplies,
                    Permission.ShowSupply,
                    Permission.EditSupply,

                    Permission.ShowHouses,
                    Permission.ShowHouse,

                    Permission.ShowOrders,
                    Permission.ShowOrder,
                    Permission.OrderExecutionBegin,
                    Permission.FinishOrder
                }
            },
            { Positions.HR, new[]
                {
                    Permission.ShowEmployees,
                    Permission.EditEmployee,
                    Permission.ShowEmployee,

                    Permission.ShowClients,
                    Permission.ShowClient,
                    Permission.EditClient
                }
            }
        };
    }

    public enum Permission
    {
        EditHouse,
        ShowHouse,
        ShowHouses,

        EditOrder,
        ShowOrder,
        ShowOrders,
        PayOrder,

        ShowSupplies,
        ShowSupply,
        EditSupply,

        ShowSuppliers,
        ShowSupplier,
        EditSupplier,

        ShowMaterials,
        ShowMaterial,
        EditMaterial,

        ShowEmployees,
        ShowEmployee,
        EditEmployee,

        ShowClients,
        ShowClient,
        EditClient,
        EditOrderClient,
        AcceptOrder,
        DenyOrder,
        OrderExecutionBegin,
        FinishOrder
    }
}
