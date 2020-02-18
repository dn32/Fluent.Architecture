using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq.Expressions;

namespace dn32.infra.Extensoes
{
    public static class ExpressionUtil
    {
        //public static PropertyInfo GetPropertyInfo<TSource, TProperty>(Expression<Func<TSource, TProperty>> propertyLambda)
        //{
        //    Type type = typeof(TSource);

        //    if (!(propertyLambda.Body is MemberExpression member))
        //    {
        //        throw new ArgumentException($"Expression '{propertyLambda}' refers to a method, not a property.");
        //    }

        //    var propInfo = member.Member as PropertyInfo;
        //    if (propInfo == null)
        //    {
        //        throw new ArgumentException($"Expression '{propertyLambda}' refers to a field, not a property.");
        //    }

        //    if (type != propInfo.ReflectedType && !type.IsSubclassOf(propInfo.ReflectedType))
        //    {
        //        throw new ArgumentException($"Expresion '{propertyLambda}' refers to a property that is not from type {type}.");
        //    }

        //    return propInfo;
        //}

        //public static List<Tuple<string, Type>> ValideExpression<T>(Expression<Func<T, object>> par, bool valide = true) where T : BaseEntity, new()
        //{
        //    var listaDepropriedades = new List<Tuple<string, Type>>();
        //    var membros = ((NewExpression)par.Body).Members as IReadOnlyCollection<MemberInfo>;
        //    var typeOriginal = typeof(T);

        //    foreach (PropertyInfo membro in membros)
        //    {
        //        var typeInformado = membro.PropertyType;
        //        var nomeDoParametro = membro.Name;
        //        var propriedades = nomeDoParametro.Split('_');

        //        var nomeConcatenadoDasPropriedades = string.Empty;

        //        for (int i = 0; i < propriedades.Count(); i++)
        //        {
        //            var nome = propriedades[i];
        //            nomeConcatenadoDasPropriedades += string.IsNullOrEmpty(nomeConcatenadoDasPropriedades) ? nome : "." + nome;
        //            var propriedade = typeOriginal.GetProperty(nome);
        //            if (propriedade == null)
        //            {
        //                if (!valide)
        //                {
        //                    continue;
        //                }

        //                throw new Exception($"Não foi encontrado uma property com caminho {nomeConcatenadoDasPropriedades} no type {typeOriginal.Name}. Confira o elemento {nomeDoParametro}, pois é provável que esteja escrito incorretamente.");
        //            }
        //            if (propriedades.Count() == i + 1)
        //            {
        //                if (propriedade.PropertyType != typeInformado)
        //                {
        //                    throw new Exception($"O type encontrado na propriedade {typeOriginal.Name} não foi encontrado no caminho {nomeConcatenadoDasPropriedades}. o type informado é {typeInformado} e o type encontrado foi {propriedade.PropertyType}");
        //                }
        //            }

        //            typeOriginal = propriedade.PropertyType;
        //        }

        //        listaDepropriedades.Add(new Tuple<string, Type>(nomeConcatenadoDasPropriedades, typeInformado));
        //    }

        //    return listaDepropriedades;
        //}

        //public static object ObtenhavaluePorPropriedade(string propriedadeInformada, object p, out Type typeDaPropriedade)
        //{
        //    typeDaPropriedade = null;
        //    if (propriedadeInformada == null)
        //    {
        //        return null;
        //    }

        //    var propriedades = propriedadeInformada.Split('.');
        //    var typeOriginal = p.GetType();
        //    var valueOriginal = p;
        //    var nomeConcatenadoDaspropriedades = string.Empty;

        //    for (int i = 0; i < propriedades.Count(); i++)
        //    {
        //        var nome = propriedades[i];
        //        nomeConcatenadoDaspropriedades += string.IsNullOrEmpty(nomeConcatenadoDaspropriedades) ? nome : "." + nome;
        //        var propriedade = typeOriginal.GetProperty(nome);
        //        valueOriginal = propriedade.GetValue(valueOriginal);

        //        if (propriedades.Count() == i + 1)
        //        {
        //            typeDaPropriedade = propriedade.PropertyType;
        //            return valueOriginal;
        //        }

        //        typeOriginal = propriedade.PropertyType;
        //    }

        //    return null;
        //}

        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> a, Expression<Func<T, bool>> b)
        {
            if (a == null)
            {
                return b;
            }

            if (b == null)
            {
                return a;
            }

            var p = a.Parameters[0];
            var visitor = new SubstExpressionVisitor { Subst = { [b.Parameters[0]] = p } };
            var body = Expression.AndAlso(a.Body, visitor.Visit(b.Body));
            return Expression.Lambda<Func<T, bool>>(body, p);
        }

        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> a, Expression<Func<T, bool>> b)
        {
            if (a == null)
            {
                return b;
            }

            if (b == null)
            {
                return a;
            }

            var p = a.Parameters[0];
            var visitor = new SubstExpressionVisitor { Subst = { [b.Parameters[0]] = p } };
            var body = Expression.OrElse(a.Body, visitor.Visit(b.Body));
            return Expression.Lambda<Func<T, bool>>(body, p);
        }

        public static Expression<Func<T, bool>> Not<T>(this Expression<Func<T, bool>> a)
        {
            if (a == null)
            {
                return a;
            }
            var p = a.Parameters[0];
            var visitor = new SubstExpressionVisitor { Subst = { [a.Parameters[0]] = p } };
            var body = Expression.Not(a.Body);
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

        public static Expression<Func<T, bool>> StartWith<T>(string propertyName, string value, Type type)
        {
            if (type != typeof(string))
            {
                throw new InvalidOperationException($"Filter type StartWith can only be used for string and field {propertyName} is not string");
            }

            var parameter = Expression.Parameter(typeof(T), "x");
            var property = Expression.Property(parameter, propertyName);
            var containsCall = Expression.Call(property, "StartsWith", null, Expression.Constant(value, typeof(string)), Expression.Constant(StringComparison.InvariantCultureIgnoreCase));
            return Expression.Lambda<Func<T, bool>>(containsCall, parameter);
        }


        public static Expression<Func<T, bool>> EndsWith<T>(string propertyName, string value, Type type)
        {
            if (type != typeof(string))
            {
                throw new InvalidOperationException($"Filter type EndsWith can only be used for string and field {propertyName} is not string");
            }

            var parameter = Expression.Parameter(typeof(T), "x");
            var property = Expression.Property(parameter, propertyName);
            var containsCall = Expression.Call(property, "EndsWith", null, Expression.Constant(value, typeof(string)), Expression.Constant(StringComparison.InvariantCultureIgnoreCase));
            return Expression.Lambda<Func<T, bool>>(containsCall, parameter);
        }

        public static Expression<Func<T, bool>> Contains<T>(string propertyName, string value, Type type)
        {
            var parameter = Expression.Parameter(typeof(T), "x");
            var property = Expression.Property(parameter, propertyName);
            MethodCallExpression containsCall;

            if (type == typeof(string))
            {
                containsCall = Expression.Call(property, "Contains", null, Expression.Constant(value, typeof(string)), Expression.Constant(StringComparison.InvariantCultureIgnoreCase));
            }
            else
            {
                var toStringCall = Expression.Call(property, "ToString", null, Expression.Constant("D"));
                containsCall = Expression.Call(toStringCall, "Contains", null, Expression.Constant(value, typeof(string)), Expression.Constant(StringComparison.InvariantCultureIgnoreCase));
            }

            return Expression.Lambda<Func<T, bool>>(containsCall, parameter);
        }

        public static Expression<Func<T, bool>> Equals<T>(string propertyName, string value, Type type)
        {
            if (value is null) { throw new ArgumentNullException(nameof(value)); }
            type = type.GetNonNullableType();

            var parameter = Expression.Parameter(typeof(T), "x");
            Expression equalsExpression;

            if (type == typeof(bool) || type == typeof(bool?))
            {
                if (bool.TryParse(value, out bool boolValue))
                {
                    return IsTrueOrFalse<T>(propertyName, boolValue, type);
                }
                else
                {
                    throw new InvalidOperationException($"The value '{value}' for the filter is not boolean as the type is. Preferably when using EnumFilterType.FALSE or EnumFilterType.FALSE for boolean operations.");
                }
            }
            if (type == typeof(string))
            {
                var property = Expression.Property(parameter, propertyName);
                var constant = Expression.Constant(value, type);
                var methodInfo = typeof(string).GetMethod("ToUpper", new Type[] { });
                var expression = Expression.Call(property, methodInfo);
                equalsExpression = Expression.Equal(constant, expression);
            }
            else if (type.IsNumeric())
            {
                var property = Expression.Property(parameter, propertyName);
                var numberValue = Convert.ChangeType(value, type);
                var constant = Expression.Constant(numberValue, type);
                equalsExpression = Expression.Equal(property, constant);
            }
            else if (type.IsNullableEnum())
            {
                var localType = type.GetTypeByNullType();
                if (Enum.TryParse(localType, value, out object enumObject))
                {
                    var property = Expression.Property(parameter, propertyName);
                    var constant = Expression.Constant(enumObject, type);
                    equalsExpression = Expression.Equal(constant, property);
                }
                else
                {
                    throw new InvalidOperationException($"The value '{value}' for the filter is not valid enum for type {type.Name}");
                }
            }
            else
            {
                var property = Expression.Property(parameter, propertyName);
                var toStringCall = Expression.Call(property, "ToString", null, Expression.Constant("D"));
                var constant = Expression.Constant(value, typeof(string));
                equalsExpression = Expression.Equal(toStringCall, constant);
            }

            return Expression.Lambda<Func<T, bool>>(equalsExpression, parameter);
        }

        public static Expression<Func<T, bool>> IsTrue<T>(string propertyName, Type type)
        {
            return IsTrueOrFalse<T>(propertyName, true, type);
        }

        public static Expression<Func<T, bool>> IsFalse<T>(string propertyName, Type type)
        {
            return IsTrueOrFalse<T>(propertyName, false, type);
        }

        private static Expression<Func<T, bool>> IsTrueOrFalse<T>(string propertyName, bool expectedvalue, Type type)
        {
            var parameter = Expression.Parameter(typeof(T), "x");
            var property = Expression.Property(parameter, propertyName);
            if (type != typeof(bool) && type != typeof(bool?)) { throw new InvalidOperationException($"The property {propertyName} is not boolean"); }
            var containsCall = Expression.Equal(property, Expression.Constant(expectedvalue, type));
            return Expression.Lambda<Func<T, bool>>(containsCall, parameter);
        }

        public static Expression<Func<T, bool>> Smaller<T>(string propertyName, string value, bool including, Type type)
        {
            return SmallerOrGreater<T>(propertyName, value, including, type ?? null, false);
        }

        public static Expression<Func<T, bool>> Greate<T>(string propertyName, string value, bool including, Type type)
        {
            return SmallerOrGreater<T>(propertyName, value, including, type ?? null, true);
        }

        private static Expression<Func<T, bool>> SmallerOrGreater<T>(
            string propertyName,
            string value,
            bool including,
            Type type,
            bool greater
            )
        {
            var parameter = Expression.Parameter(typeof(T), "x");
            var property = Expression.Property(parameter, propertyName);
            object valueObj = null;
            BinaryExpression containsCall = null;

            if (type == typeof(DateTime) || type == typeof(DateTime?))
            {
                if (DateTime.TryParseExact(value, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime outValue))
                {
                    valueObj = outValue;
                }
                else
                {
                    throw new InvalidOperationException($"The value '{value}' for the filter is not valid DateTime as the type is");
                }
            }
            else if (type.IsNumeric())
            {
                try
                {
                    valueObj = Convert.ChangeType(value, type, CultureInfo.InvariantCulture);
                }
                catch (Exception)
                {
                    throw new InvalidOperationException($"The value '{value}' for the filter is not valid number as the type is");
                }
            }
            else
            {
                throw new InvalidOperationException($"Type {type.Name} in property {propertyName} does not support filter type GREATER/SMALLER");
            }

            if (greater)
            {
                if (including)
                {
                    containsCall = Expression.GreaterThanOrEqual(property, Expression.Constant(valueObj, type));
                }
                else
                {
                    containsCall = Expression.GreaterThan(property, Expression.Constant(valueObj, type));
                }
            }
            else
            {
                if (including)
                {
                    containsCall = Expression.LessThanOrEqual(property, Expression.Constant(valueObj, type));
                }
                else
                {
                    containsCall = Expression.LessThan(property, Expression.Constant(valueObj, type));
                }
            }

            return Expression.Lambda<Func<T, bool>>(containsCall, parameter);
        }

        public static Expression<Func<T, bool>> IsNull<T>(string propertyName)
        {
            var parameter = Expression.Parameter(typeof(T), "x");
            var property = Expression.Property(parameter, propertyName);
            var nullCheck = Expression.Equal(property, Expression.Constant(null, typeof(object)));
            return Expression.Lambda<Func<T, bool>>(nullCheck, parameter);
        }

        public static Expression<Func<T, bool>> IsNotNull<T>(string propertyName)
        {
            var parameter = Expression.Parameter(typeof(T), "x");
            var property = Expression.Property(parameter, propertyName);
            var nullCheck = Expression.NotEqual(property, Expression.Constant(null, typeof(object)));
            return Expression.Lambda<Func<T, bool>>(nullCheck, parameter);
        }

        //public static Expression<Func<T, bool>> ToExpression<T>(string andOrOperator, string propName, string opr, object value, Expression<Func<T, bool>> expr = null)
        //{
        //    Expression<Func<T, bool>> func = null;

        //    ParameterExpression paramExpr = Expression.Parameter(typeof(T));
        //    var arrProp = propName.Split('.').ToList();
        //    Expression binExpr = null;
        //    string partName = null;
        //    arrProp.ForEach(x =>
        //    {
        //        Expression tempExpr = null;
        //        partName = string.IsNullOrWhiteSpace(partName) ? x : partName + "." + x;
        //        if (partName == propName)
        //        {
        //            var member = NestedExprProp(paramExpr, partName);
        //            var type = member.Type.Name == "Nullable`1" ? Nullable.GetUnderlyingType(member.Type) : member.Type;
        //            tempExpr = ApplyFilter(opr, member, Expression.Convert(Expression.Constant(value), member.Type));
        //        }
        //        else
        //            tempExpr = ApplyFilter("!=", NestedExprProp(paramExpr, partName), Expression.Constant(null));
        //        if (binExpr != null)
        //            binExpr = Expression.AndAlso(binExpr, tempExpr);
        //        else
        //            binExpr = tempExpr;
        //    });

        //    Expression<Func<T, bool>> innerExpr = Expression.Lambda<Func<T, bool>>(binExpr, paramExpr);
        //    if (expr != null)
        //        innerExpr = (andOrOperator == null || andOrOperator == "And" || andOrOperator == "AND" || andOrOperator == "&&") ? innerExpr.And(expr) : innerExpr.Or(expr);
        //    func = innerExpr;

        //    return func;
        //}

        //private static MemberExpression NestedExprProp(Expression expr, string propName)
        //{
        //    string[] arrProp = propName.Split('.');
        //    int arrPropCount = arrProp.Length;
        //    return (arrPropCount > 1) ? Expression.Property(NestedExprProp(expr, arrProp.Take(arrPropCount - 1).Aggregate((a, i) => a + "." + i)), arrProp[arrPropCount - 1]) : Expression.Property(expr, propName);
        //}

        //private static Expression ApplyFilter(string opr, Expression left, Expression right)
        //{
        //    Expression InnerLambda = null;
        //    switch (opr)
        //    {
        //        case "==":
        //        case "=":
        //            InnerLambda = Expression.Equal(left, right);
        //            break;
        //        case "<":
        //            InnerLambda = Expression.LessThan(left, right);
        //            break;
        //        case ">":
        //            InnerLambda = Expression.GreaterThan(left, right);
        //            break;
        //        case ">=":
        //            InnerLambda = Expression.GreaterThanOrEqual(left, right);
        //            break;
        //        case "<=":
        //            InnerLambda = Expression.LessThanOrEqual(left, right);
        //            break;
        //        case "!=":
        //            InnerLambda = Expression.NotEqual(left, right);
        //            break;
        //        case "&&":
        //            InnerLambda = Expression.And(left, right);
        //            break;
        //        case "||":
        //            InnerLambda = Expression.Or(left, right);
        //            break;
        //        case "LIKE":
        //            InnerLambda = Expression.Call(left, typeof(string).GetMethod("Contains", new Type[] { typeof(string) }), right);
        //            break;
        //        case "NOTLIKE":
        //            InnerLambda = Expression.Not(Expression.Call(left, typeof(string).GetMethod("Contains", new Type[] { typeof(string) }), right));
        //            break;
        //    }

        //    return InnerLambda;
        //}
    }
}
