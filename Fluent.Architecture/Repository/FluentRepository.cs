using System;
using System.Collections.Generic;
using System.Linq;
using Fluent.Architecture.Attributes;
using Fluent.Architecture.Extensions;
using Fluent.Architecture.Model;
using Fluent.Architecture.Specifications;
using Fluent.Architecture.Service;
#if NET461
using System.Data.Entity;
#else
using Microsoft.EntityFrameworkCore;
#endif

namespace Fluent.Architecture.Repository
{
    /// <inheritdoc />
    /// <summary>
    /// Repositório base com entidade do sistema baseado em Entity Framework.
    /// </summary>
    /// <typeparam name="TE">
    /// O tipo de entidade do repositório.
    /// </typeparam>
    public class FluentRepository<TE> : BaseRepository where TE : BaseEntity
    {
        /// <summary>
        /// Os objetos de transação do repositório.
        /// </summary>
        internal TransactionObjects TransactionObjects { get; set; }

        /// <summary>
        /// A query contem a referência de todas as tabelas/documentos do banco de dados.
        /// </summary>
        internal IQueryable<TE> Query => TransactionObjects.GetObjectQueryInternal<TE>();

        /// <summary>
        /// A referência de input de dados para o banco de dados.
        /// </summary>
        internal DbSet<TE> Input => TransactionObjects.GetObjectInputDataInternal<TE>();

        /// <summary>
        /// O serviço qual esse repositório representa.
        /// </summary>
        internal FluentService<TE> Service { get; set; }

        private void RunTheContextValidation()
        {
            Service.SessionRequest.ContextFluentValidation.Validate();
        }

#if NET461
        /// <summary>
        /// Todo - Muito cuidado, pois se definir esse método como público, pode permitir vilnerabilidades no sistema.
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        [Propagate]
        internal bool ExistsSql(string sql)
        {
            return Input.SqlQuery(sql).Any();
        }

        //Todo - Codumentar
        [Propagate]
        internal TE FindSingleOrDefaultSql(string sql)
        {
            return Input.SqlQuery(sql).SingleOrDefault();
        }
#else
        [Propagate]
        internal bool ExistsSql(string sql)
        {
            throw new NotImplementedException();
        }

        [Propagate]
        internal TE FindSingleOrDefaultSql(string sql)
        {
            throw new NotImplementedException();
        }
#endif

        /// <summary>
        /// Executa uma solicitação baseada em uma especificação e retorna uma lista paginada de resultados.
        /// </summary>
        /// <typeparam name="TO">
        /// O tipo de saida desejada. Deve ser o mesmo definido na saida da especificação.
        /// </typeparam>
        /// <param name="spec">
        /// A especificação de requisição.
        /// </param>
        /// <param name="pagination">
        /// A paginação desejada.
        /// </param>
        /// <returns>
        /// A lista paginada de resultados.
        /// </returns>
        [Propagate]
        public virtual List<TO> Spec<TO>(FluentSelectSpecification<TE, TO> spec, FluentPagination pagination = null)
        {
            return FluentPaginate(spec.ToIQueryable(Query), pagination).ToList();
        }

       

        /// <summary>
        /// Executa uma solicitação baseada em uma especificação e retorna uma lista paginada de resultados.
        /// </summary>
        /// <param name="spec">
        /// A especificação de requisição.
        /// </param>
        /// <param name="pagination">
        /// A paginação desejada.
        /// </param>
        /// <returns>
        /// A lista paginada de resultados.
        /// </returns>
        [Propagate]
        public virtual List<TE> Spec(FluentSpecification<TE> spec, FluentPagination pagination = null)
        {
            return FluentPaginate(spec.ToIQueryable(Query), pagination).ToList();
        }

        /// <summary>
        /// Executa uma solicitação baseada em uma especificação e retorna um resultado ou nulo quando a consulta não é satisfeita.
        /// </summary>
        /// <typeparam name="TO">
        /// O tipo de saida desejada. Deve ser o mesmo definido na saida da especificação.
        /// </typeparam>
        /// <param name="spec">
        /// A especificação de requisição.
        /// </param>
        /// <returns>
        /// O item referente à consulta ou nulo.
        /// </returns>
        [Propagate]
        public virtual TO SpecOne<TO>(FluentSelectSpecification<TE, TO> spec)
        {
           return spec.Spec(Query).FirstOrDefault();
        }

        /// <summary>
        /// Executa uma solicitação baseada em uma especificação e retorna um resultado ou nulo quando a consulta não é satisfeita.
        /// </summary>
        /// <param name="spec">
        /// A especificação de requisição.
        /// </param>
        /// <returns>
        /// O item referente à consulta ou nulo.
        /// </returns>
        [Propagate]
        public virtual TE SpecOne(FluentSpecification<TE> spec)
        {
            return spec.ToIQueryable(Query).FirstOrDefault();
        }

        /// <summary>
        /// Avalia se um item existe no banco de dados, baseado em uma especificação.
        /// </summary>
        /// <param name="spec">
        /// A especificação de requisição.
        /// </param>
        /// <returns>
        /// Se o item existe ou não.
        /// </returns>
        [Propagate]
        public virtual bool Exists(FluentSpecification<TE> spec)
        {
            return spec.ToIQueryable(Query).Any();
        }

        /// <summary>
        /// Avalia se um item existe no banco de dados, baseado em uma especificação.
        /// </summary>
        /// <typeparam name="TO">
        /// O tipo de saida desejada. Deve ser o mesmo definido na saida da especificação.
        /// </typeparam>
        /// <param name="spec">
        /// A especificação de requisição.
        /// </param>
        /// <returns>
        /// Se o item existe ou não.
        /// </returns>
        [Propagate]
        public virtual bool Exists<TO>(FluentSelectSpecification<TE, TO> spec)
        {
            return spec.ToIQueryable(Query).Any();
        }

        /// <summary>
        /// Retorna a quantidade de itens existentes que satisfaçam a uma especificação
        /// </summary>
        /// <typeparam name="TO">
        /// O tipo de saida desejada. Deve ser o mesmo definido na saida da especificação.
        /// </typeparam>
        /// <param name="spec">
        /// A especificação de requisição.
        /// </param>
        /// <returns>
        /// A quantidade de itens.
        /// </returns>
        [Propagate]
        public virtual int Count<TO>(FluentSelectSpecification<TE, TO> spec)
        {
            return spec.ToIQueryable(Query).Count();
        }

        /// <summary>
        /// Retorna a quantidade de itens existentes que satisfaçam a uma especificação
        /// </summary>
        /// <param name="spec">
        /// A especificação de requisição.
        /// </param>
        /// <returns>
        /// A quantidade de itens.
        /// </returns>
        [Propagate]
        public virtual int Count(FluentSpecification<TE> spec)
        {
            return spec.ToIQueryable(Query).Count();
        }

        ///// <summary>
        ///// Consulta um item no banco de dados com base em seu id.
        ///// </summary>
        ///// <param name="id">
        ///// Id da consulta.
        ///// </param>
        ///// <returns>
        ///// Item encontrado ou nulo.
        ///// </returns>
        //[Propagate]
        //public virtual TE Find(int id)
        //{
        //    return Input.Find(id);
        //}

        public TE Find(TE entity)
        {
            var sql = CreateSqlFromKeyAndFluentUniqueKeys(entity);
            return FindSingleOrDefaultSql(sql);
        }

        public bool Exists(TE entity)
        {
            var sql = CreateSqlFromKeyAndFluentUniqueKeys(entity);
            return ExistsSql(sql);
        }

        /// <summary>
        /// Adiciona vários itens de um mesmo tipo ao banco de dados.
        /// </summary>
        /// <param name="entities">
        /// Itens a serem adicionados.
        /// </param>
        [Propagate]
        public virtual void AddRange(params TE[] entities)
        {
            RunTheContextValidation();
            entities.ToList().ForEach(x => Input.Add(x));
        }

        /// <summary>
        /// Adiciona um item ao banco de dados.
        /// </summary>
        /// <param name="entity">
        /// Item a ser adicionado.
        /// </param>
        [Propagate]
        public virtual TE Add(TE entity)
        {
            RunTheContextValidation();

#if NET461
            return Input.Add(entity);
#else
            return Input.Add(entity).Entity;
#endif
        }

        /// <summary>
        /// Atualiza um item do banco de dados baseado em seu identificador.
        /// </summary>
        /// <param name="entity">
        /// Entidade a ser atualizada com o identificador preenchido.
        /// </param>
        [Propagate]
        public virtual TE Update(TE entity)
        {
            RunTheContextValidation();

#if NET461
            var currentEntity = Find(entity);
            TransactionObjects.Session.Entry(currentEntity).CurrentValues.SetValues(entity);
            return entity;
#else
            throw new NotImplementedException();
            // Input.Update(entity);
#endif
        }

        /// <summary>
        /// Remove um item do banco de dados baseado em seu identificador.
        /// </summary>
        /// <param name="entity">
        /// Entidate a ser removida.
        /// </param>
        [Propagate]
        public virtual TE Remove(TE entity)
        {
            RunTheContextValidation();

#if NET461
          return Input.Remove(Find(entity));
#else
            throw new NotImplementedException();
#endif
        }

        public virtual void RemoveRange(FluentSpecification<TE> spec)
        {
            var list = spec.ToIQueryable(Query);
            Input.RemoveRange(list);
        }

        [Propagate]
        public virtual void RemoveRange(params TE[] entities)
        {
            entities.ToList().ForEach(x => Remove(x));
        }

        #region INTERNAL

        //private static string CreateSqlFromKeys(TE entity)
        //{
        //    var tableName = entity.GetTableName();
        //    var keyValues = entity.GetKeyValues().Select(x => $"({x.Key} = {x.Value} and {x.Key} != 0)").ToArray();
        //    var sql = $"select * from {tableName} where ";
        //    sql += string.Join(" and ", keyValues);
        //    return sql;
        //}

        private static string CreateSqlFromKeyAndFluentUniqueKeys(TE entity)
        {
            var tableName = entity.GetTableName();
            var keyValues = entity.GetKeyValues().Select(x => $"{x.ColumnName} = {x.Value}").ToArray();
            var fluentUniqueKeyValues = entity.GetFluentUniqueKeyValues().Select(x => $"{x.ColumnName} = {x.Value}").ToArray();


            var sql = $"select * from {tableName} where ({string.Join(" and ", keyValues)})";

            if (fluentUniqueKeyValues.Length > 0)
            {
                sql += $" or ({string.Join(" or ", fluentUniqueKeyValues)})";
            }

            return sql;
        }

        private static IQueryable<TX> FluentPaginate<TX>(IQueryable<TX> query, FluentPagination pagination)
        {
            if (pagination == null)
            {
                return query;
            }

            pagination.TotalQuantityOfItems = query.Count();
            query = query
                .Skip(pagination.ItemsPerPage * (pagination.CurrentPage - 1))
                .Take(pagination.ItemsPerPage);

            return query;
        }

        #endregion
    }
}