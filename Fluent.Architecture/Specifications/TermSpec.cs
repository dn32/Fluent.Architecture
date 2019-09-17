using Fluent.Architecture.Core.Attributes;
using Fluent.Architecture.Entities;
using Fluent.Architecture.Extensions;
using Fluent.Architecture.Specifications;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Fluent.Architecture.Core.Specifications
{
    public class TermSpec<T> : FluentSpecification<T> where T : FluentEntity
    {
        private string Term { get; set; }

        public TermSpec<T> SetParameter(string term)
        {
            Term = term;
            return this;
        }

        public override IQueryable<T> Where(IQueryable<T> query)
        {
            var expression = TermToExpression(Term);
            return query.Where(expression);
        }

        public override IOrderedQueryable<T> Order(IQueryable<T> query)
        {
            return query.OrderBy(x => x);
        }

        private Expression<Func<T, bool>> TermToExpression(string term)
        {
            Expression<Func<T, bool>> allExpression = x => true;

            if (!string.IsNullOrEmpty(term))
            {
                var properties = typeof(T).GetProperties().Where(x => x.GetCustomAttribute<SearchableAttribute>() != null).ToArray();
                foreach (var property in properties)
                {
                    var expression = ExpressionUtil.Contains<T>(property.Name, term, property.PropertyType);
                    allExpression = allExpression == null ? expression : allExpression.Or(expression);
                }
            }

            return allExpression;
        }
    }
}
