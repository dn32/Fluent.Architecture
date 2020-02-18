using dn32.infra.Extensoes;
using dn32.infra.nucleo.atributos;
using dn32.infra.Nucleo.Extensoes;
using dn32.infra.Specifications;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using dn32.infra.dados;

namespace dn32.infra.Nucleo.Specifications
{
    public class TermSpec<T> : DnSpecification<T> where T : DnEntidade
    {
        private string Term { get; set; }

        public bool IsList { get; set; }

        public TermSpec<T> SetParameter(string term, bool isList)
        {
            Term = term;
            IsList = isList;
            return this;
        }

        public override IQueryable<T> Where(IQueryable<T> query)
        {
            var expression = TermToExpression(Term);

            return query
                    .Where(expression)
                    .GetInclusions(IsList)
                    .DnDynamicProjectTo(Service);

        }

        public override IOrderedQueryable<T> Order(IQueryable<T> query)
        {
            return query.DnDynamicProjectToOrder(Service);
        }

        private Expression<Func<T, bool>> TermToExpression(string term)
        {
            Expression<Func<T, bool>> allExpression = null;

            if (!string.IsNullOrEmpty(term))
            {
                var properties = typeof(T).GetProperties().Where(x => x.GetCustomAttribute<DnBuscavelAtributo>() != null).ToArray();
                foreach (var property in properties)
                {
                    var expression = ExpressionUtil.IsNull<T>(property.Name).Not();
                    expression = expression.And(ExpressionUtil.Contains<T>(property.Name, term, property.PropertyType));
                    allExpression = allExpression == null ? expression : allExpression.Or(expression);
                }
            }

            return allExpression ?? (x => true);
        }
    }
}
