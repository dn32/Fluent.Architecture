using Fluent.Architecture.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Fluent.Architecture.Extensions
{
    public static class UtilitarioDeExpression
    {
        public static PropertyInfo GetPropertyInfo<TSource, TProperty>(Expression<Func<TSource, TProperty>> propertyLambda)
        {
            Type type = typeof(TSource);

            MemberExpression member = propertyLambda.Body as MemberExpression;
            if (member == null)
                throw new ArgumentException(string.Format(
                    "Expression '{0}' refers to a method, not a property.",
                    propertyLambda.ToString()));

            PropertyInfo propInfo = member.Member as PropertyInfo;
            if (propInfo == null)
                throw new ArgumentException(string.Format(
                    "Expression '{0}' refers to a field, not a property.",
                    propertyLambda.ToString()));

            if (type != propInfo.ReflectedType &&
                !type.IsSubclassOf(propInfo.ReflectedType))
                throw new ArgumentException(string.Format(
                    "Expresion '{0}' refers to a property that is not from type {1}.",
                    propertyLambda.ToString(),
                    type));

            return propInfo;
        }

        public static List<Tuple<string, Type>> ValideExpression<T>(Expression<Func<T, object>> par, bool valide = true) where T : BaseEntity, new()
        {
            var listaDepropriedades = new List<Tuple<string, Type>>();
            var membros = ((NewExpression)par.Body).Members as IReadOnlyCollection<MemberInfo>;
            var tipoOriginal = typeof(T);

            foreach (PropertyInfo membro in membros)
            {
                var tipoInformado = membro.PropertyType;
                var nomeDoParametro = membro.Name;
                var propriedades = nomeDoParametro.Split('_');

                var nomeConcatenadoDasPropriedades = string.Empty;

                for (int i = 0; i < propriedades.Count(); i++)
                {
                    var nome = propriedades[i];
                    nomeConcatenadoDasPropriedades += nomeConcatenadoDasPropriedades == string.Empty ? nome : "." + nome;
                    var propriedade = tipoOriginal.GetProperty(nome);
                    if (propriedade == null)
                    {
                        if (!valide)
                        {
                            continue;
                        }

                        throw new Exception($"Não foi encontrado uma property com caminho {nomeConcatenadoDasPropriedades} no tipo {tipoOriginal.Name}. Confira o elemento {nomeDoParametro}, pois é provável que esteja escrito incorretamente.");
                    }
                    if (propriedades.Count() == i + 1)
                    {
                        if (propriedade.PropertyType != tipoInformado)
                        {
                            throw new Exception($"O tipo encontrado na propriedade {tipoOriginal.Name} não foi encontrado no caminho {nomeConcatenadoDasPropriedades}. o tipo informado é {tipoInformado} e o tipo encontrado foi {propriedade.PropertyType}");
                        }
                    }

                    tipoOriginal = propriedade.PropertyType;
                }

                listaDepropriedades.Add(new Tuple<string, Type>(nomeConcatenadoDasPropriedades, tipoInformado));
            }

            return listaDepropriedades;
        }

        public static object ObtenhaValorPorPropriedade(string propriedadeInformada, object p, out Type tipoDaPropriedade)
        {
            tipoDaPropriedade = null;
            if (propriedadeInformada == null)
            {
                return null;
            }

            var propriedades = propriedadeInformada.Split('.');
            var tipoOriginal = p.GetType();
            var valorOriginal = p;
            var nomeConcatenadoDaspropriedades = string.Empty;

            for (int i = 0; i < propriedades.Count(); i++)
            {
                var nome = propriedades[i];
                nomeConcatenadoDaspropriedades += nomeConcatenadoDaspropriedades == string.Empty ? nome : "." + nome;
                var propriedade = tipoOriginal.GetProperty(nome);
                valorOriginal = propriedade.GetValue(valorOriginal);

                if (propriedades.Count() == i + 1)
                {
                    tipoDaPropriedade = propriedade.PropertyType;
                    return valorOriginal;
                }

                tipoOriginal = propriedade.PropertyType;
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
            if(a == null)
            {
                return b;
            }

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
                Expression newValue;
                return Subst.TryGetValue(node, out newValue) ? newValue : node;
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

        public static Expression<Func<T, bool>> Equals<T>(string nomePropriedade, object valor)
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
