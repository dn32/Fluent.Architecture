// ReSharper disable CommentTypo

using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Fluent.Architecture.Attributes;
using Fluent.Architecture.Extensions;
using Fluent.Architecture.Interfaces;
using Fluent.Architecture.Model;
using Fluent.Architecture.Services;
using Fluent.Architecture.Specifications;

namespace Fluent.Architecture.Repository
{
    /// <inheritdoc />
    /// <summary>
    /// Repositório base com entidade do sistema baseado em Entity Framework.
    /// </summary>
    /// <typeparam name="TE">
    /// O tipo de entidade do repositório.
    /// </typeparam>
    public class FluentRepository<TE> : TransactionlRepository where TE : BaseEntity
    {
        #region PROPERTIES

        /// <summary>
        /// Os objetos de transação do repositório.
        /// </summary>
        internal TransactionObjects TransactionObjects { get; set; }

        /// <summary>
        /// A referência da sessão do EF.
        /// </summary>
        protected internal EfContext Session => TransactionObjects.Session;

        /// <summary>
        /// A query contem a referência de todas as tabelas/documentos do banco de dados.
        /// </summary>
        protected internal IQueryable<TE> Query => this.TransactionObjects.GetObjectQueryInternal<TE>();

        /// <summary>
        /// A referência de input de dados para o banco de dados.
        /// </summary>
        internal DbSet<TE> Input => this.TransactionObjects.GetObjectInputDataInternal<TE>();

        /// <summary>
        /// A referência de um Input de tradução.
        /// </summary>
        internal DbSet<Translation> TranslactionInput => this.TransactionObjects.GetObjectInputDataInternal<Translation>();

        /// <summary>
        /// O serviço qual esse repositório representa.
        /// </summary>
        internal FluentService<TE> Service { get; set; }

        protected void RunTheContextValidation() => Service.SessionRequest.ContextFluentValidationException.Validate();

        #endregion

        #region SPEC TE

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
        //public virtual TE FirstOrDefault(IFluentSpecification spec)
        public virtual TE FirstOrDefault(IFluentSpecification spec)
        {
            return GetSpec(spec).ToIQueryable(Query).FirstOrDefault();
        }

        #endregion

        #region SPEC OUT

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
        public virtual List<TO> List<TO>(IFluentSpecification<TO> spec, FluentPagination pagination = null)
        {
            return FluentPaginate(GetSpec(spec).ToIQueryable(Query), pagination).ToList();
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
        public virtual TO FirstOrDefault<TO>(IFluentSpecification<TO> spec)
        {
            return GetSpec(spec).ToIQueryable(Query).FirstOrDefault();
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
        public virtual bool Exists<TO>(IFluentSpecification<TO> spec)
        {
            return GetSpec(spec).ToIQueryable(Query).Any();
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
        public virtual int Count<TO>(IFluentSpecification<TO> spec)
        {
            return GetSpec(spec).ToIQueryable(Query).Count();
        }

        #endregion

        #region SQL

        /// <summary>
        /// Todo - Muito cuidado, pois se definir esse método como público, pode permitir vilnerabilidades no sistema por ser string sql.
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        [Propagate]
        internal bool ExistsSql(string sql)
        {
            return this.Input.SqlQuery(sql).Any();
        }

        [Propagate]
        internal TE FindSingleOrDefaultSql(string sql)
        {
            return this.Input.SqlQuery(sql).SingleOrDefault();
        }

        #endregion

        #region ENTITY ITEMS

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
        public virtual List<TE> List(IFluentSpecification spec, FluentPagination pagination = null)
        {
            return FluentPaginate(GetSpec(spec).ToIQueryable(Query), pagination).ToList();
        }

        //Todo não é ´permitido listar sem spec, pois sem spec não tem como ordenar pra paginar. Sem paginação pode ter sobrecarga.
        //Todo doc
        //[Propagate]
        //public virtual List<TE> List(FluentPagination pagination = null)
        //{
        //    return FluentPaginate(Query.ToIQueryable(Query), pagination).ToList();
        //}

        //Todo doc
        [Propagate]
        public virtual TE FirstOrDefault()
        {
            return Query.FirstOrDefault();
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
        public virtual bool Exists(IFluentSpecification spec)
        {
            return GetSpec(spec).ToIQueryable(Query).Any();
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
        public virtual int Count(IFluentSpecification spec)
        {
            return GetSpec(spec).ToIQueryable(Query).Count();
        }

        //Todo doc
        [Propagate]
        public virtual int Count()
        {
            return Query.Count();
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
        // [PropagateMethod]
        // public virtual TE Find(int id)
        // {
        //     return Input.Find(id);
        // }

        public virtual TE Find(TE entity)
        {
            var sql = CreateSqlFromKeyAndFluentUniqueKeys(entity);
            return FindSingleOrDefaultSql(sql);
        }

        public virtual bool Exists(TE entity)
        {
            var sql = CreateSqlFromKeyAndFluentUniqueKeys(entity);
            return this.ExistsSql(sql);
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

        [Propagate]
        public virtual TE Add(TE entity)
        {
            RunTheContextValidation();

            return Input.Add(entity);
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

            var currentEntity = this.Find(entity);
            TransactionObjects.Session.Entry(currentEntity).CurrentValues.SetValues(entity);
            return currentEntity;
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
            this.RunTheContextValidation();

            return this.Input.Remove(this.Find(entity));
        }

        public virtual void RemoveRange(IFluentSpecification spec)
        {
            var list = GetSpec(spec).ToIQueryable(Query);
            this.Input.RemoveRange(list);
        }

        [Propagate]
        public virtual void RemoveRange(params TE[] entities)
        {
            entities.ToList().ForEach(x => this.Remove(x));
        }

        #endregion

        #region INTERNAL

        private FluentSelectSpecification<TE, TO> GetSpec<TO>(IFluentSpecification<TO> spec)
        {
            return spec as FluentSelectSpecification<TE, TO>;
        }

        private FluentSpecification<TE> GetSpec(IFluentSpecification spec)
        {
            return spec as FluentSpecification<TE>;
        }

        // private static string CreateSqlFromKeys(TE entity)
        // {
        // var tableName = entity.GetTableName();
        // var keyValues = entity.GetKeyValues().Select(x => $"({x.Key} = {x.Value} and {x.Key} != 0)").ToArray();
        // var sql = $"select * from {tableName} where ";
        // sql += string.Join(" and ", keyValues);
        // return sql;
        // }

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

        protected static IQueryable<TX> FluentPaginate<TX>(IQueryable<TX> query, FluentPagination pagination)
        {
            if (pagination == null)
            {
                pagination = new FluentPagination(1, false, 255);
            }

            pagination.TotalQuantityOfItems = query.Count();
            query = query
                .Skip(pagination.Skip)
                .Take(pagination.ItemsPerPage);

            return query;
        }

        #endregion
    }
}

