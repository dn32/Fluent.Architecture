using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Fluent.Architecture.Extensions
{
    public static class ExpressionExtension
    {
        public static PropertyInfo GetPropertyInfo<TSource, TProperty>(this Expression<Func<TSource, TProperty>> propertyLambda)
        {
            var type = typeof(TSource);

            if (!(propertyLambda.Body is MemberExpression member))
            {
                throw new ArgumentException($"Expression '{propertyLambda.ToString()}' refers to a method, not a property.");
            }

            var propInfo = member.Member as PropertyInfo;
            if (propInfo == null)
            {
                throw new ArgumentException($"Expression '{propertyLambda.ToString()}' refers to a field, not a property.");
            }

            if (type != propInfo.ReflectedType && !type.IsSubclassOf(propInfo.ReflectedType))
            {
                throw new ArgumentException($"Expresion '{propertyLambda.ToString()}' refers to a property that is not from type {type}.");
            }

            return propInfo;
        }

        public static List<Tuple<string, Type>> ValideExpression<T>(this Expression<Func<T, object>> par, bool valide = true)
        {
            var propertiesList = new List<Tuple<string, Type>>();
            var members = ((NewExpression)par.Body).Members as IReadOnlyCollection<MemberInfo>;
            var originalType = typeof(T);

            foreach (PropertyInfo member in members)
            {
                var propertyType = member.PropertyType;
                var parameterName = member.Name;
                var properties = parameterName.Split('_');

                var stringName = string.Empty;

                for (int i = 0; i < properties.Count(); i++)
                {
                    var name = properties[i];
                    stringName += string.IsNullOrWhiteSpace(stringName) ? name : "." + name;
                    var property = originalType.GetProperty(name);
                    if (property == null)
                    {
                        if (!valide)
                        {
                            continue;
                        }

                        throw new Exception($"Could not find a property with path {stringName} in type {originalType.Name}. Check the {parameterName} element as it is likely to be spelled incorrectly.");
                    }
                    if (properties.Count() == i + 1)
                    {
                        if (property.PropertyType != propertyType)
                        {
                            throw new Exception($"The type found in the {originalType.Name} property was not found in the path {stringName}. the type informed is {propertyType} and the type found was {property.PropertyType}");
                        }
                    }

                    originalType = property.PropertyType;
                }

                propertiesList.Add(new Tuple<string, Type>(stringName, propertyType));
            }

            return propertiesList;
        }

        public static object GetPropertyValue(this string propertyName, object obj, out Type propertyType)
        {
            propertyType = null;
            if (propertyName == null)
            {
                return null;
            }

            var properties = propertyName.Split('.');
            var originalType = obj.GetType();
            var originalValue = obj;

            for (int i = 0; i < properties.Count(); i++)
            {
                var nome = properties[i];
                var propriedade = originalType.GetProperty(nome);
                originalValue = propriedade.GetValue(originalValue);

                if (properties.Count() == i + 1)
                {
                    propertyType = propriedade.PropertyType;
                    return originalValue;
                }

                originalType = propriedade.PropertyType;
            }

            return null;
        }

        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> a, Expression<Func<T, bool>> b)
        {
            var p = a.Parameters[0];
            var visitor = new SubstExpressionVisitor { Subst = { [b.Parameters[0]] = p } };
            var body = Expression.AndAlso(a.Body, visitor.Visit(b.Body));
            return Expression.Lambda<Func<T, bool>>(body, p);
        }

        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> a, Expression<Func<T, bool>> b)
        {
            var p = a.Parameters[0];
            var visitor = new SubstExpressionVisitor { Subst = { [b.Parameters[0]] = p } };
            Expression body = Expression.OrElse(a.Body, visitor.Visit(b.Body));
            return Expression.Lambda<Func<T, bool>>(body, p);
        }

        internal class SubstExpressionVisitor : ExpressionVisitor
        {
            public Dictionary<Expression, Expression> Subst = new Dictionary<Expression, Expression>();

            protected override Expression VisitParameter(ParameterExpression node)
            {
                return Subst.TryGetValue(node, out Expression newValue) ? newValue : node;
            }
        }

        public static Expression<Func<T, bool>> Contains<T>(string nomePropriedade, string valor, Type tipo)
        {
            if (tipo == typeof(string))
            {
                var parameter = Expression.Parameter(typeof(T), "x");
                var property = Expression.Property(parameter, nomePropriedade);

                var containsCall = Expression.Call(
                    property, "Contains",
                    null, Expression.Constant(valor));

                return Expression.Lambda<Func<T, bool>>(containsCall, parameter);
            }
            else
            {
                var parameter = Expression.Parameter(typeof(T), "x");
                var property = Expression.Property(parameter, nomePropriedade);
                var toStringCall = Expression.Call(
                    property, "ToString",
                    null, Expression.Constant("D"));

                var containsCall = Expression.Call(
                    toStringCall, "Contains",
                    null, Expression.Constant(valor));

                return Expression.Lambda<Func<T, bool>>(containsCall, parameter);
            }
        }

        public static Expression<Func<T, bool>> Igual<T>(string nomePropriedade, object valor)
        {
            var conditions = ToExpression<T>("and", nomePropriedade, "==", valor);
            return conditions;
        }

        public static Expression<Func<T, bool>> ToExpression<T>(string andOrOperator, string propName, string opr, object value, Expression<Func<T, bool>> expr = null)
        {
            Expression<Func<T, bool>> func = null;

            ParameterExpression paramExpr = Expression.Parameter(typeof(T));
            var arrProp = propName.Split('.').ToList();
            Expression binExpr = null;
            string partName = null;
            arrProp.ForEach(x =>
            {
                Expression tempExpr = null;
                partName = string.IsNullOrWhiteSpace(partName) ? x : partName + "." + x;
                if (partName == propName)
                {
                    var member = NestedExprProp(paramExpr, partName);
                    var type = member.Type.Name == "Nullable`1" ? Nullable.GetUnderlyingType(member.Type) : member.Type;
                    tempExpr = ApplyFilter(opr, member, Expression.Convert(Expression.Constant(value), member.Type));
                }
                else
                    tempExpr = ApplyFilter("!=", NestedExprProp(paramExpr, partName), Expression.Constant(null));
                if (binExpr != null)
                    binExpr = Expression.AndAlso(binExpr, tempExpr);
                else
                    binExpr = tempExpr;
            });

            Expression<Func<T, bool>> innerExpr = Expression.Lambda<Func<T, bool>>(binExpr, paramExpr);
            if (expr != null)
                innerExpr = (andOrOperator == null || andOrOperator == "And" || andOrOperator == "AND" || andOrOperator == "&&") ? innerExpr.And(expr) : innerExpr.Or(expr);
            func = innerExpr;

            return func;
        }

        private static MemberExpression NestedExprProp(Expression expr, string propName)
        {
            string[] arrProp = propName.Split('.');
            int arrPropCount = arrProp.Length;
            return (arrPropCount > 1) ? Expression.Property(NestedExprProp(expr, arrProp.Take(arrPropCount - 1).Aggregate((a, i) => a + "." + i)), arrProp[arrPropCount - 1]) : Expression.Property(expr, propName);
        }

        private static Expression ApplyFilter(string opr, Expression left, Expression right)
        {
            Expression InnerLambda = null;
            switch (opr)
            {
                case "==":
                case "=":
                    InnerLambda = Expression.Equal(left, right);
                    break;
                case "<":
                    InnerLambda = Expression.LessThan(left, right);
                    break;
                case ">":
                    InnerLambda = Expression.GreaterThan(left, right);
                    break;
                case ">=":
                    InnerLambda = Expression.GreaterThanOrEqual(left, right);
                    break;
                case "<=":
                    InnerLambda = Expression.LessThanOrEqual(left, right);
                    break;
                case "!=":
                    InnerLambda = Expression.NotEqual(left, right);
                    break;
                case "&&":
                    InnerLambda = Expression.And(left, right);
                    break;
                case "||":
                    InnerLambda = Expression.Or(left, right);
                    break;
                case "LIKE":
                    InnerLambda = Expression.Call(left, typeof(string).GetMethod("Contains", new Type[] { typeof(string) }), right);
                    break;
                case "NOTLIKE":
                    InnerLambda = Expression.Not(Expression.Call(left, typeof(string).GetMethod("Contains", new Type[] { typeof(string) }), right));
                    break;
            }

            return InnerLambda;
        }
    }
}
