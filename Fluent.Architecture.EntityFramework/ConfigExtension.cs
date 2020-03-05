using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

#if NETCOREAPP3_1

using Microsoft.EntityFrameworkCore.Query;

#else

using Remotion.Linq.Parsing.ExpressionVisitors;

#endif

namespace dn32.infra.EntityFramework
{
    public static class ConfigExtension
    {
        public static DnConfiguracoesGlobais UseEntityFramework(this DnConfiguracoesGlobais configClass)
        {
            return configClass.DefinirFabricaDeRepositorio(new RepositoryFactory());
        }

        internal static void AddQueryFilter(this EntityTypeBuilder entityTypeBuilder, LambdaExpression expression)
        {
            var parameterType = Expression.Parameter(entityTypeBuilder.Metadata.ClrType);
            var expressionFilter = ReplacingExpressionVisitor.Replace(expression.Parameters.Single(), parameterType, expression.Body);

            //var internalEntityTypeBuilder = entityTypeBuilder.GetInternalEntityTypeBuilder();
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
            var internalEntityTypeBuilder = typeof(EntityTypeBuilder)
                .GetProperty("Builder", BindingFlags.NonPublic | BindingFlags.Instance)?
                .GetValue(entityTypeBuilder) as InternalEntityTypeBuilder;

            return internalEntityTypeBuilder;
        }
    }
}
