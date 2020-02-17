using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Fluente.Arquitetura.Extensoes
{
    public static class DbContextExtensions
    {
        public static bool HasPendingChanges(this DbContext context)
        {
            return context?.ChangeTracker?.Entries()?.Any(e => e.State == EntityState.Added || e.State == EntityState.Deleted || e.State == EntityState.Modified) ?? false;
        }
    }
}
