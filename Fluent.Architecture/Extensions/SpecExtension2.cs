using Fluent.Architecture.Core.Models;
using Fluent.Architecture.Extensions;
using Fluent.Architecture.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace Fluent.Architecture.Core.Extensions
{
    public static class SpecExtension2
    {
        public static IQueryable<T> GetInclusions<T>(this IQueryable<T> query, bool list) where T : FluentEntity
        {
            //typeof(T)
            //    .GetProperties()
            //    .Where(x => x.GetCustomAttributeAny<FluentCompositionAttribute>())
            //    .ToList()
            //    .ForEach(x => { query = query.Include(x.Name); });

            if (typeof(T).Is(typeof(IFluentInclusionEntity)))
            {
                var entity = Activator.CreateInstance(typeof(T)).FluentCast<IFluentInclusionEntity>();
                var inclusions = list ? entity.InclusionsForList : entity.InclusionsForOne;
                inclusions.ToList().ForEach(x => { query = query.Include(x); });
            }

            return query;
        }
    }
}
