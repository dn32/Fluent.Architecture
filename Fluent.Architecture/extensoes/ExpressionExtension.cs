using System;
using System.Globalization;
using System.Linq.Expressions;
using dn32.infra.extensoes;
using dn32.infra.nucleo.excecoes;

namespace dn32.infra.nucleo.extensoes
{
    public static class DnExpressoesExtensao
    {
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
            var visitante = new DnExpressionVisitor { Dicionario = { [b.Parameters[0]] = p } };
            var corpo = Expression.AndAlso(a.Body, visitante.Visit(b.Body));
            return Expression.Lambda<Func<T, bool>>(corpo, p);
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
            var visitante = new DnExpressionVisitor { Dicionario = { [b.Parameters[0]] = p } };
            var corpo = Expression.OrElse(a.Body, visitante.Visit(b.Body));
            return Expression.Lambda<Func<T, bool>>(corpo, p);
        }

        public static Expression<Func<T, bool>> Not<T>(this Expression<Func<T, bool>> a)
        {
            if (a == null)
            {
                return a;
            }

            var p = a.Parameters[0];
            var corpo = Expression.Not(a.Body);
            return Expression.Lambda<Func<T, bool>>(corpo, p);
        }

        public static Expression<Func<T, bool>> IniciaCom<T>(string nomeDaPropriedade, string valor, Type tipo)
        {
            if (tipo != typeof(string))
            {
                throw new DesenvolvimentoIncorretoException($"O filtro {nameof(IniciaCom)} deve ser usado somente " +
                                                            $"com propriedades do tipo string e '{nomeDaPropriedade}' não é do tipo string.");
            }

            var parametro = Expression.Parameter(typeof(T), "x");
            var propriedade = Expression.Property(parametro, nomeDaPropriedade);
            var chamadaContem = Expression.Call(
                propriedade,
                "StartsWith",
                null,
                Expression.Constant(valor, typeof(string)),
                Expression.Constant(StringComparison.InvariantCultureIgnoreCase));

            return Expression.Lambda<Func<T, bool>>(chamadaContem, parametro);
        }


        public static Expression<Func<T, bool>> TerminaCom<T>(string nomeDaPropriedade, string valor, Type tipo)
        {
            if (tipo != typeof(string))
            {
                throw new DesenvolvimentoIncorretoException($"O filtro {nameof(TerminaCom)} deve ser usado somente com " +
                                                            $"propriedades do tipo string e '{nomeDaPropriedade}' não é do tipo string.");
            }

            var parametro = Expression.Parameter(typeof(T), "x");
            var propriedade = Expression.Property(parametro, nomeDaPropriedade);
            var chamadaContem = Expression.Call(
                propriedade,
                "EndsWith",
                null,
                Expression.Constant(valor, typeof(string)),
                Expression.Constant(StringComparison.InvariantCultureIgnoreCase));

            return Expression.Lambda<Func<T, bool>>(chamadaContem, parametro);
        }

        public static Expression<Func<T, bool>> Contem<T>(string nomeDaPropriedade, string valor, Type tipo)
        {
            var parametro = Expression.Parameter(typeof(T), "x");
            var propriedade = Expression.Property(parametro, nomeDaPropriedade);
            MethodCallExpression chamadaContem;

            if (tipo == typeof(string))
            {
                chamadaContem = Expression.Call(
                    propriedade,
                    "Contains",
                    null,
                    Expression.Constant(valor, typeof(string)),
                    Expression.Constant(StringComparison.InvariantCultureIgnoreCase));
            }
            else
            {
                var chamadaToString = Expression.Call(propriedade, "ToString", null, Expression.Constant("D"));
                chamadaContem = Expression.Call(
                    chamadaToString,
                    "Contains",
                    null,
                    Expression.Constant(valor, typeof(string)),
                    Expression.Constant(StringComparison.InvariantCultureIgnoreCase));
            }

            return Expression.Lambda<Func<T, bool>>(chamadaContem, parametro);
        }

        public static Expression<Func<T, bool>> Igual<T>(string nomeDaPropriedade, string valor, Type tipo)
        {
            if (valor is null)
            {
                throw new ArgumentNullException(nameof(valor));
            }

            tipo = tipo.GetNonNullableType();

            var parametro = Expression.Parameter(typeof(T), "x");
            Expression expressaoIgual;

            if (tipo == typeof(bool) || tipo == typeof(bool?))
            {
                if (bool.TryParse(valor, out var valorBooleano))
                {
                    return EhVerdadeiroOuFalso<T>(nomeDaPropriedade, valorBooleano, tipo);
                }

                throw new DesenvolvimentoIncorretoException($"O valor '{valor}' para o filtro '{nameof(Igual)}' não é um booleano" +
                                                            " e o tipo de propriedade é booleano. É preferível fazer uso do filtro" +
                                                            " do tipo 'Verdadeiro' ou 'Falso' para operações com booleano.");
            }
            if (tipo == typeof(string))
            {
                var propriedade = Expression.Property(parametro, nomeDaPropriedade);
                var constante = Expression.Constant(valor, tipo);
                var metodo = typeof(string).GetMethod("ToUpper", new Type[] { });
                var expressao = Expression.Call(propriedade, metodo);
                expressaoIgual = Expression.Equal(constante, expressao);
            }
            else if (tipo.IsNumeric())
            {
                var propriedade = Expression.Property(parametro, nomeDaPropriedade);
                var valorNumerico = Convert.ChangeType(valor, tipo);
                var constant = Expression.Constant(valorNumerico, tipo);
                expressaoIgual = Expression.Equal(propriedade, constant);
            }
            else if (tipo.IsNullableEnum())
            {
                var tipoLocal = tipo.GetTypeByNullType();
                if (Enum.TryParse(tipoLocal, valor, out var valorEnum))
                {
                    var propriedade = Expression.Property(parametro, nomeDaPropriedade);
                    var constant = Expression.Constant(valorEnum, tipo);
                    expressaoIgual = Expression.Equal(constant, propriedade);
                }
                else
                {
                    throw new DesenvolvimentoIncorretoException($"O valor '{valor}' para o filtro '{nameof(Igual)}' não é um enumerador válido.");
                }
            }
            else
            {
                var propriedade = Expression.Property(parametro, nomeDaPropriedade);
                var chamadaToString = Expression.Call(propriedade, "ToString", null, Expression.Constant("D"));
                var constant = Expression.Constant(valor, typeof(string));
                expressaoIgual = Expression.Equal(chamadaToString, constant);
            }

            return Expression.Lambda<Func<T, bool>>(expressaoIgual, parametro);
        }

        public static Expression<Func<T, bool>> EhVerdadeiro<T>(string nomeDaPropriedade, Type tipo) =>
             EhVerdadeiroOuFalso<T>(nomeDaPropriedade, true, tipo);

        public static Expression<Func<T, bool>> EhValso<T>(string nomeDaPropriedade, Type tipo) =>
             EhVerdadeiroOuFalso<T>(nomeDaPropriedade, false, tipo);

        private static Expression<Func<T, bool>> EhVerdadeiroOuFalso<T>(string nomeDaPropriedade, bool valorEsperado, Type tipo)
        {
            var parametro = Expression.Parameter(typeof(T), "x");
            var propriedade = Expression.Property(parametro, nomeDaPropriedade);
            if (tipo != typeof(bool) && tipo != typeof(bool?))
            {
                throw new InvalidOperationException($"A propriedade '{nomeDaPropriedade}' não é do tipo booleano.");
            }

            var chamadaContem = Expression.Equal(propriedade, Expression.Constant(valorEsperado, tipo));
            return Expression.Lambda<Func<T, bool>>(chamadaContem, parametro);
        }

        public static Expression<Func<T, bool>> EhMenorQue<T>(string nomeDaPropriedade, string valor, bool inclusive, Type tipo) =>
             EhMaiorOuMenorQue<T>(nomeDaPropriedade, valor, inclusive, tipo, false);

        public static Expression<Func<T, bool>> EhMaiorQue<T>(string nomeDaPropriedade, string valor, bool inclusive, Type tipo) =>
            EhMaiorOuMenorQue<T>(nomeDaPropriedade, valor, inclusive, tipo, true);

        private static Expression<Func<T, bool>> EhMaiorOuMenorQue<T>(
            string nomeDaPropriedade,
            string valor,
            bool inclusive,
            Type tipo,
            bool valorEsperadoEhMaior)
        {
            var parametro = Expression.Parameter(typeof(T), "x");
            var propriedade = Expression.Property(parametro, nomeDaPropriedade);
            object valorObjeto;
            BinaryExpression chamadaContem;

            if (tipo == typeof(DateTime) || tipo == typeof(DateTime?))
            {
                if (DateTime.TryParseExact(valor, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var valorEmData))
                {
                    valorObjeto = valorEmData;
                }
                else
                {
                    throw new DesenvolvimentoIncorretoException($"O valor '{valor}' para o filtro '{nameof(EhMaiorOuMenorQue)}' não é uma data válida.");
                }
            }
            else if (tipo.IsNumeric())
            {
                try
                {
                    valorObjeto = Convert.ChangeType(valor, tipo, CultureInfo.InvariantCulture);
                }
                catch (Exception)
                {
                    throw new DesenvolvimentoIncorretoException($"O valor '{valor}' para o filtro '{nameof(EhMaiorOuMenorQue)}' não é um número válido.");
                }
            }
            else
            {
                throw new DesenvolvimentoIncorretoException($"O tipo '{tipo.Name}' da propriedade" +
                                                            $" '{nomeDaPropriedade}' não é suportado pelos filtros '{nameof(EhMaiorOuMenorQue)}'.");
            }

            if (valorEsperadoEhMaior)
            {
                chamadaContem = inclusive ?
                    Expression.GreaterThanOrEqual(propriedade, Expression.Constant(valorObjeto, tipo)) :
                    Expression.GreaterThan(propriedade, Expression.Constant(valorObjeto, tipo));
            }
            else
            {
                chamadaContem = inclusive ?
                    Expression.LessThanOrEqual(propriedade, Expression.Constant(valorObjeto, tipo)) :
                    Expression.LessThan(propriedade, Expression.Constant(valorObjeto, tipo));
            }

            return Expression.Lambda<Func<T, bool>>(chamadaContem, parametro);
        }

        public static Expression<Func<T, bool>> EhNulo<T>(string nomeDaPropriedade)
        {
            var parametro = Expression.Parameter(typeof(T), "x");
            var propriedade = Expression.Property(parametro, nomeDaPropriedade);
            var checagemDeNulo = Expression.Equal(
                propriedade, 
                Expression.Constant(null, typeof(object)));

            return Expression.Lambda<Func<T, bool>>(checagemDeNulo, parametro);
        }

        public static Expression<Func<T, bool>> NaoEhNulo<T>(string nomeDaPropriedade)
        {
            var parametro = Expression.Parameter(typeof(T), "x");
            var propriedade = Expression.Property(parametro, nomeDaPropriedade);
            var checagemDeNulo = Expression.NotEqual(
                propriedade, 
                Expression.Constant(null, typeof(object)));

            return Expression.Lambda<Func<T, bool>>(checagemDeNulo, parametro);
        }
    }
}
