using dn32.infra.Extensoes;
using dn32.infra.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using dn32.infra.dados;
using dn32.infra.extensoes;

namespace dn32.infra.Nucleo.Extensoes
{
    public static class SpecExtension2
    {
        public static IQueryable<T> GetInclusions<T>(this IQueryable<T> query, bool list) where T : FluenteEntidade
        {
            //typeof(T)
            //    .GetProperties()
            //    .Where(x => x.GetCustomAttributeAny<FluenteCompositionAttribute>())
            //    .ToList()
            //    .ForEach(x => { query = query.Include(x.Name); });

            if (typeof(T).Is(typeof(IFluenteInclusionEntity)))
            {
                var entity = Activator.CreateInstance(typeof(T)).FluenteCast<IFluenteInclusionEntity>();
                var inclusions = list ? entity.InclusionsForList : entity.InclusionsForOne;
                inclusions.ToList().ForEach(x => { query = query.Include(x); });
            }

            return query;
        }
    }
}
