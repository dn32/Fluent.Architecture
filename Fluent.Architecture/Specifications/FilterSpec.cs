using Fluent.Architecture.Core.Filters;
using Fluent.Architecture.Entities;
using Fluent.Architecture.Extensions;
using Fluent.Architecture.Specifications;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Fluent.Architecture.Core.Specifications
{
    public class FilterSpec<T> : FluentSpecification<T> where T : FluentEntity
    {
        private Filter[] Filters { get; set; }

        public FilterSpec<T> SetParameter(Filter[] filters)
        {
            Filters = filters;
            return this;
        }

        public override IQueryable<T> Where(IQueryable<T> query)
        {
            var expression = FiltersToExtression(Filters);
            return query.Where(expression);
        }

        public override IOrderedQueryable<T> Order(IQueryable<T> query)
        {
            return query.OrderBy(x => x);
        }

        private Expression<Func<T, bool>> FiltersToExtression(Filter[] filters)
        {
            var properties = typeof(T).GetProperties().ToList();
            Expression<Func<T, bool>> allExpression = null;
            EnumJunctionType LastJunctionType = EnumJunctionType.OR;

            foreach (var filter in filters)
            {
                //Todo - Como validar se a propriedade existe antes de chegar aqui?
                var property = properties.Single(x => x.Name.Equals(filter.PropertyName, StringComparison.InvariantCultureIgnoreCase));

                Expression<Func<T, bool>> expression;

                switch (filter.FilterType)
                {
                    case EnumFilterType.CONTAINS:
                        expression = ExpressionUtil.Contains<T>(property.Name, filter.Value, property.PropertyType);
                        break;
                    case EnumFilterType.GREATER_THAN:
                        expression = ExpressionUtil.SmallerThan<T>(property.Name, filter.Value, filter.Including);
                        break;
                    case EnumFilterType.SMALLER_THAN:
                        expression = ExpressionUtil.SmallerThan<T>(property.Name, filter.Value, filter.Including);
                        break;
                    case EnumFilterType.START_WITH:
                        expression = ExpressionUtil.StartWith<T>(property.Name, filter.Value, property.PropertyType);
                        break;
                    case EnumFilterType.ENDS_WITH:
                        expression = ExpressionUtil.EndsWith<T>(property.Name, filter.Value, property.PropertyType);
                        break;
                    case EnumFilterType.EQUAL_TO:
                        expression = ExpressionUtil.Equals<T>(property.Name, filter.Value, property.PropertyType);
                        break;
                    case EnumFilterType.TRUE:
                        expression = ExpressionUtil.Equals<T>(property.Name, "true", property.PropertyType);
                        break;
                    case EnumFilterType.FALSE:
                        expression = ExpressionUtil.Equals<T>(property.Name, "false", property.PropertyType);
                        break;
                    case EnumFilterType.NULL:
                        expression = ExpressionUtil.IsNull<T>(property.Name);
                        break;
                    default:
                        throw new NotImplementedException($"{filter.FilterType} filter not found");
                }

                if (filter.IsReverse)
                {
                    expression = expression.Not();
                }

                if (allExpression == null)
                {
                    allExpression = expression;
                }
                else
                {
                    if (LastJunctionType == EnumJunctionType.OR)
                    {
                        allExpression = allExpression.Or(expression);
                    }
                    else
                    {
                        allExpression = allExpression.And(expression);
                    }
                }

                LastJunctionType = filter.JunctionType;
            }

            if (allExpression == null) { allExpression = x => true; }

            return allExpression;
        }
    }
}
