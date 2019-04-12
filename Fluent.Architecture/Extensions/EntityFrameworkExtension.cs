
using System;
using System.Linq;
#if NET461
using System.Data.Entity;


#else
using Microsoft.EntityFrameworkCore;

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
