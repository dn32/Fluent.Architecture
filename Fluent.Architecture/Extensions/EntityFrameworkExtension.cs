
using System;
using System.Linq;

#if NETCOREAPP2_1
using Microsoft.EntityFrameworkCore;

#else
using System.Data.Entity;

#endif


namespace Fluent.Architecture.Extensions
{
    public static class DbContextExtensions
    {
        public static bool HasPendingChanges(this DbContext context)
        {
            return context.ChangeTracker.Entries()
                          .Any(e => e.State == EntityState.Added
                                 || e.State == EntityState.Deleted
                                 || e.State == EntityState.Modified);
        }
    }
}
