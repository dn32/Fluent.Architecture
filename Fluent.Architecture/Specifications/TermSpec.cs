using Fluent.Architecture.Core.Attributes;
using Fluent.Architecture.Core.Extensions;
using Fluent.Architecture.Core.Models;
using Fluent.Architecture.Extensions;
using Fluent.Architecture.Specifications;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Fluent.Architecture.Core.Specifications
{
    public class TermSpec<T> : FluentSelectSpecification<T, object> where T : FluentEntity
    {
        private string Term { get; set; }

        public bool IsList { get; set; }

        public TermSpec<T> SetParameter(string term, bool isList)
        {
            Term = term;
            IsList = isList;
            return this;
        }

        public override IQueryable<object> Where(IQueryable<T> query)
        {
            var expression = TermToExpression(Term);

            return query
                    .Where(expression)
                    .GetInclusions(IsList)
                    .FluentDynamicProjectTo(Service);

        }

        public override IOrderedQueryable<object> Order(IQueryable<object> query)
        {
            return query.FluentDynamicProjectToOrder(Service);
        }

        private Expression<Func<T, bool>> TermToExpression(string term)
        {
            Expression<Func<T, bool>> allExpression = null;

            if (!string.IsNullOrEmpty(term))
            {
                var properties = typeof(T).GetProperties().Where(x => x.GetCustomAttribute<SearchableAttribute>() != null).ToArray();
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
