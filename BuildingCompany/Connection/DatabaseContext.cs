using System.Data.Entity;

namespace BuildingCompany.Connection
{
    public class DatabaseContext : DbContext
    {
        public static BuildingCompanyEntities Entities { get; }

        static DatabaseContext()
        {
            Entities = new BuildingCompanyEntities();

            Entities.SystemImage.Load();
            Entities.Supplier.Load();
            Entities.Client.Load();
            Entities.Employee.Load();
            Entities.House.Load();
        }

        public static void CancelChanges()
        {
            foreach (var entry in Entities.ChangeTracker.Entries())
            {
                switch (entry.State)
                {
                    case EntityState.Modified:
                        entry.CurrentValues.SetValues(entry.OriginalValues);
                        entry.State = EntityState.Unchanged;
                        break;
                    case EntityState.Added:
                        entry.State = EntityState.Detached;
                        break;
                    case EntityState.Deleted:
                        entry.State = EntityState.Unchanged;
                        break;

                }
            }
        }

    }
}
