using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Reflection.Emit;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using dn32.infra.dados;
using dn32.infra.Factory.Proxy;
using dn32.infra.Nucleo.Models;
using dn32.infra.Nucleo.Util;
using dn32.infra.servicos;
using Microsoft.AspNetCore.Http;

namespace dn32.infra.nucleo.extensoes
{
    //Todo - refatorar
    public static class EspecificacaoDinamicaExtensao
    {
        private static string ObterParametro(this HttpRequest requisicao, string nomeDaPropriedade)
        {
            var valor = requisicao.Headers[nomeDaPropriedade].FirstOrDefault()?.Trim();
            valor = string.IsNullOrWhiteSpace(valor) ? requisicao.Query[nomeDaPropriedade].ToString()?.Trim() : valor;
            return string.IsNullOrWhiteSpace(valor) ? requisicao.Cookies[nomeDaPropriedade]?.Trim() : valor;
        }

        private static string[] ObterParametros(this HttpRequest requisicao, string nomeDaPropriedade)
        {
            return requisicao
                .ObterParametro(nomeDaPropriedade)?
                .Split(",")?
                .Select(x => x?.Trim())
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .ToArray();
        }

        public static string[] ObterPropriedadesAConsiderar(this HttpRequest requisicao) =>
         requisicao.ObterParametros("propriedadeAConsiderar");


        public static string[] ObterPropriedadesAOrdenar(this HttpRequest requisicao) =>
             requisicao.ObterParametros("propriedadeAOrdenar");

        public static IQueryable<T> ProjetarDeFormaDinamica<T>(
            this IQueryable<T> consulta,
            DnServicoTransacionalBase servico,
            string[] campos = null) where T : EntidadeBase
        {
            var requisicao = servico.HttpContextLocal.Request;
            if (campos == null || campos.Length == 0)
            {
                campos = requisicao.ObterPropriedadesAConsiderar();
            }

            if (campos == null || campos.Length == 0)
            {
                return consulta;
            }

            return consulta.ProjetarDeFormaDinamica(campos);
        }

        private static IQueryable<T> ProjetarDeFormaDinamica<T>(this IQueryable<T> consulta, string[] campos)
        {
            var ret = ProjetarDeFormaDinamicaSelecionada(consulta, campos, out var configuracoesDeMapeamento);
            return ret.ProjectTo<T>(configuracoesDeMapeamento);
        }

        public static IOrderedQueryable<T> ProjetarDeFormaDinamicaOrdenada<T>(this IQueryable<T> consulta, DnServicoTransacionalBase servico, string[] campos = null) where T : EntidadeBase
        {
            var requisicao = servico.HttpContextLocal.Request;
            if (campos == null || campos.Length == 0)
            {
                campos = requisicao.ObterPropriedadesAConsiderar();
            }

            var ordem = requisicao.ObterPropriedadesAOrdenar();
            var ordemEmTexto = ordem?.Length > 0 ? string.Join(",", ordem) : campos?.FirstOrDefault();
            return string.IsNullOrEmpty(ordemEmTexto) ? consulta.OrderBy(x => x) : consulta.OrderBy(ordemEmTexto);
        }

        private static IQueryable<object> ProjetarDeFormaDinamicaSelecionada<T>(
            this IQueryable<T> consulta,
            string[] campos,
            out MapperConfiguration configuracoesDeMapeamento)
        {
            var descricoes = new DnClassDescription(typeof(T), campos);
            var id = RandomUtil.NextRandomString(6);
            var nomeDaDll = $"DnDll_{id}";
            var tipo = descricoes.CriarOTipo(nomeDaDll);
            var tipos = new List<Tuple<Type, Type>>();

            descricoes.ObterTodosOsTiposPorDescricao(tipos);

            configuracoesDeMapeamento = new MapperConfiguration(
                cfg =>
            {
                cfg.CreateMap(typeof(T), typeof(T));
                cfg.CreateMap(typeof(T), tipo).ReverseMap();
                tipos.ForEach(x => cfg.CreateMap(x.Item1, x.Item2).ReverseMap());
            });

            var metodo = typeof(Extensions).GetMethods()
                .Where(x => x.Name == nameof(Extensions.ProjectTo))
                .ToList()[2].MakeGenericMethod(tipo);//Todo - Isso deve ser revisto, há uma chance de problemas futuros aqui

            var ret = metodo.Invoke(
                null,
                new object[]
                {
                    consulta, configuracoesDeMapeamento, null, campos.Select(x => x).ToArray()
                });
            return ret as IQueryable<object>;
        }

        internal static void ObterTodosOsTiposPorDescricao(this DnClassDescription descricao, List<Tuple<Type, Type>> tipos)
        {
            var descricoes = descricao.Properties.Where(propriedade => propriedade.DnClassDescription != null);
            foreach (var propriedade in descricoes)
            {
                tipos.Add(new Tuple<Type, Type>(propriedade.Type, propriedade.DynamicProperty));
                ObterTodosOsTiposPorDescricao(propriedade.DnClassDescription, tipos);
            }
        }

        internal static Type CriarOTipo(this DnClassDescription descricao, string nomeDaDll)
        {
            var construtorPrincipal = CriarTipo(nomeDaDll);

            foreach (var propriedade in descricao.Properties)
            {
                if (propriedade.DnClassDescription != null)
                {
                    propriedade.DynamicProperty = CriarOTipo(propriedade.DnClassDescription, nomeDaDll);
                    construtorPrincipal.CreateProperty(propriedade.Name, propriedade.DynamicProperty);
                }
                else
                {
                    construtorPrincipal.CreateProperty(propriedade.Name, propriedade.Type);
                }
            }

            return construtorPrincipal.CreateType();
        }

        internal static TypeBuilder CriarTipo(string nomeDaDll)
        {
            var nomeDoModulo = "DnEntidadeCustomizadaMap";
            var construtorDeTipo = BuilderClassUtil.CreateClass(typeof(object), nomeDaDll, nomeDoModulo);
            construtorDeTipo.CreateConstructor();
            return construtorDeTipo;
        }

        //Todo - Não remove. Dei muito trabalho pra desenvolver
        public static IOrderedQueryable<object> ProjetarDeFormaDinamicaSelecionadaEOrdenada(this IQueryable<object> consulta, DnServicoTransacionalBase servico)
        {
            var requisicao = servico.HttpContextLocal.Request;
            var considerar = requisicao.ObterPropriedadesAConsiderar();
            var ordenar = requisicao.ObterPropriedadesAOrdenar();
            var ordenarTexto = ordenar?.Length > 0 ? string.Join(",", ordenar) : considerar.FirstOrDefault();

            if (string.IsNullOrEmpty(ordenarTexto))
            {
                return consulta.OrderBy(x => x);
            }

            return consulta.OrderBy(ordenarTexto);
        }

        //Todo - Não remove. Dei muito trabalho pra desenvolver
        public static IQueryable<object> ProjetarDeFormaDinamicaSelecionada<T>(this IQueryable<T> consulta, DnServicoTransacionalBase servico) where T : EntidadeBase
        {
            var requisicao = servico.HttpContextLocal.Request;
            var campos = requisicao.ObterPropriedadesAConsiderar();
            if (campos == null || campos.Length == 0)
            {
                return consulta;
            }

            return consulta.ProjetarDeFormaDinamicaSelecionada(campos, out _);
        }
    }
}
