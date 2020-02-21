using System;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using dn32.infra.dados;
using dn32.infra.enumeradores;
using dn32.infra.extensoes;

[assembly: InternalsVisibleTo(@"dn32.infra.EntityFramework.Oracle, PublicKey=0024000004800000940000000602000000240000525341310004000001000100711a027753449168af0f6154fc04941b1def87bbd780db14e03da60eaf68a976e9a1a149bca2bf0dcba92bab8f90b5889f455143f23423f3231ce3067b7d73462a690618b888df4df5f24dfcc09bb492e486333d025f91938ce6b4cb5e2530376b0cb3cf8ce1808af7458a7ef8768496c38c91656521cbf5b141c6c901aec5ba")]
namespace dn32.infra.nucleo.extensoes
{
    internal static class FiltrosExtensao
    {
        internal static Expression<Func<T, bool>> ConverterFiltrosParaExpressao<T>(this Filtro[] filtros)
        {
            var propriedades = typeof(T).GetProperties().ToList();
            Expression<Func<T, bool>> expressaoCompleta = null;
            var tipoDaUltimaJuncao = EnumTipoDeJuncao.Ou;

            foreach (var filtro in filtros)
            {
                //Todo - Como validar se a propriedade existe antes de chegar aqui?
                var propriedade = propriedades.FirstOrDefault(
                    x =>
                    x.Name.Equals(filtro.NomeDaPropriedade, StringComparison.InvariantCultureIgnoreCase));
                if (propriedade == null)
                {
                    throw new InvalidOperationException($"A entidade '{typeof(T).GetFriendlyName()}' não possui uma propriedade com nome '{filtro.NomeDaPropriedade}' como mencionado no filtro.");
                }

                Expression<Func<T, bool>> expressao = x => true;

                switch (filtro.TipoDeFiltro)
                {
                    case EnumTipoDeFiltro.Contem:
                        expressao = DnExpressoesExtensao.Contem<T>(propriedade.Name, filtro.Valor?.ToUpper(CultureInfo.InvariantCulture) ?? "", propriedade.PropertyType);
                        expressao = DnExpressoesExtensao.NaoEhNulo<T>(propriedade.Name).And(expressao);
                        break;
                    case EnumTipoDeFiltro.MaiorQue:
                        expressao = DnExpressoesExtensao.EhMaiorQue<T>(propriedade.Name, filtro.Valor?.ToUpper(CultureInfo.InvariantCulture) ?? "", filtro?.Inclusive ?? false, propriedade.PropertyType);
                        expressao = DnExpressoesExtensao.NaoEhNulo<T>(propriedade.Name).And(expressao);
                        break;
                    case EnumTipoDeFiltro.MenorQue:
                        expressao = DnExpressoesExtensao.EhMenorQue<T>(propriedade.Name, filtro.Valor?.ToUpper(CultureInfo.InvariantCulture) ?? "", filtro?.Inclusive ?? false, propriedade.PropertyType);
                        expressao = DnExpressoesExtensao.NaoEhNulo<T>(propriedade.Name).And(expressao);
                        break;
                    case EnumTipoDeFiltro.IniciaCom:
                        expressao = DnExpressoesExtensao.IniciaCom<T>(propriedade.Name, filtro.Valor?.ToUpper(CultureInfo.InvariantCulture) ?? "", propriedade.PropertyType);
                        expressao = DnExpressoesExtensao.NaoEhNulo<T>(propriedade.Name).And(expressao);
                        break;
                    case EnumTipoDeFiltro.TerminaCom:
                        expressao = DnExpressoesExtensao.TerminaCom<T>(propriedade.Name, filtro.Valor?.ToUpper(CultureInfo.InvariantCulture) ?? "", propriedade.PropertyType);
                        expressao = DnExpressoesExtensao.NaoEhNulo<T>(propriedade.Name).And(expressao);
                        break;
                    case EnumTipoDeFiltro.Igual:
                        expressao = DnExpressoesExtensao.Igual<T>(propriedade.Name, filtro.Valor?.ToUpper(CultureInfo.InvariantCulture) ?? "", propriedade.PropertyType);
                        break;
                    case EnumTipoDeFiltro.Verdadeiro:
                        expressao = DnExpressoesExtensao.EhVerdadeiro<T>(propriedade.Name, propriedade.PropertyType);
                        expressao = DnExpressoesExtensao.NaoEhNulo<T>(propriedade.Name).And(expressao);
                        break;
                    case EnumTipoDeFiltro.Falso:
                        expressao = DnExpressoesExtensao.EhValso<T>(propriedade.Name, propriedade.PropertyType);
                        expressao = DnExpressoesExtensao.NaoEhNulo<T>(propriedade.Name).And(expressao);
                        break;
                    case EnumTipoDeFiltro.Nulo:
                        expressao = DnExpressoesExtensao.EhNulo<T>(propriedade.Name);
                        break;
                    default:
                        break;
                }

                if (filtro?.Inverter == true)
                {
                    expressao = expressao.Not();
                }

                if (expressaoCompleta == null)
                {
                    expressaoCompleta = expressao;
                }
                else
                {
                    expressaoCompleta = tipoDaUltimaJuncao == EnumTipoDeJuncao.Ou ?
                        expressaoCompleta.Or(expressao) :
                        expressaoCompleta.And(expressao);
                }

                tipoDaUltimaJuncao = filtro?.TipoDeJuncao ?? EnumTipoDeJuncao.E;
            }

            return expressaoCompleta;
        }
    }
}
