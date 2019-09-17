using Fluent.Architecture.Extensions;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Remotion.Linq.Parsing.ExpressionVisitors;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Fluent.Architecture.EntityFramework
{
    public static class ConfigExtension
    {
        public static Config UseEntityFramework(this Config configClass)
        {
            return configClass.SetRepositoryFactory(new RepositoryFactory());
        }

        internal static void AddQueryFilter(this EntityTypeBuilder entityTypeBuilder, LambdaExpression expression)
        {
            var parameterType = Expression.Parameter(entityTypeBuilder.Metadata.ClrType);
            var expressionFilter = ReplacingExpressionVisitor.Replace(
                expression.Parameters.Single(), parameterType, expression.Body);

            var internalEntityTypeBuilder = entityTypeBuilder.GetInternalEntityTypeBuilder();
            //if (internalEntityTypeBuilder.Metadata.QueryFilter != null)
            //{
            //    var currentQueryFilter = internalEntityTypeBuilder.Metadata.QueryFilter;
            //    var currentExpressionFilter = ReplacingExpressionVisitor.Replace(
            //        currentQueryFilter.Parameters.Single(), parameterType, currentQueryFilter.Body);
            //    expressionFilter = Expression.AndAlso(currentExpressionFilter, expressionFilter);
            //}

            var lambdaExpression = Expression.Lambda(expressionFilter, parameterType);
            entityTypeBuilder.HasQueryFilter(lambdaExpression);
        }

        internal static InternalEntityTypeBuilder GetInternalEntityTypeBuilder(this EntityTypeBuilder entityTypeBuilder)
        {
            var property = typeof(EntityTypeBuilder).GetProperty("Builder", BindingFlags.NonPublic | BindingFlags.Instance);
            if (property == null) { throw new NotImplementedException($"property Builder not found in {nameof(entityTypeBuilder)}"); }
            var internalEntityTypeBuilder = property.GetValue(entityTypeBuilder)?.FluentCast<InternalEntityTypeBuilder>();
            if (internalEntityTypeBuilder == null) { throw new NotImplementedException($"property Builder has no value"); }
            return internalEntityTypeBuilder;
        }
    }
}
