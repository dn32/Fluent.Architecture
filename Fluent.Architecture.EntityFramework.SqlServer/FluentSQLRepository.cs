// -----------------------------------------------------------------------
// <copyright company="Fluent System">
//     Copyright © Fluent System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------

// ReSharper disable CommentTypo
#if NET461
using System.Data.Entity;

#else
using Microsoft.EntityFrameworkCore;

#endif

using System.Collections.Generic;
using System.Linq;
using Fluent.Architecture.Attributes;
using Fluent.Architecture.Exceptions;
using Fluent.Architecture.Extensions;
using Fluent.Architecture.Interfaces;
using Fluent.Architecture.Model;
using Fluent.Architecture.Services;
using Fluent.Architecture.Specifications;
using Fluent.Architecture.Core.Interfaces;
using System;
using Fluent.Architecture.Core.Attributes;
using Fluent.Architecture.Enumerator;

namespace Fluent.Architecture.Repository
{
    /// <inheritdoc />
    /// <summary>
    /// Repositório base com entidade do sistema baseado em Entity Framework.
    /// </summary>
    /// <typeparam name="TE">
    /// O tipo de entidade do repositório.
    /// </typeparam>
    [DbType(FluentDbType.SQL_SERVER)]
    public class FluentSQLRepository<TE> : TransactionlRepository, IFluentRepository<TE> where TE : BaseEntity
    {
        public FluentSQLRepository()
        {
        }

        #region PROPERTIES

        private string ConnectionStringID { get; set; }

        public ITransactionObjects TransactionObjects { get; set; }

        public Type TransactionObjectsType => typeof(TransactionObjects);

        public UserSessionRequest SessionRequest => Service.SessionRequest;

        /// <summary>
        /// A referência da sessão do EF.
        /// </summary>
        protected internal EfContext Session => TransactionObjects.Session as EfContext;

        /// <summary>
        /// A query contem a referência de todas as tabelas/documentos do banco de dados.
        /// </summary>
        protected internal IQueryable<TE> Query => this.TransactionObjects.GetObjectQueryInternal<TE>();

        /// <summary>
        /// A referência de input de dados para o banco de dados.
        /// </summary>
        internal DbSet<TE> Input => this.TransactionObjects.GetObjectInputDataInternal<TE>() as DbSet<TE>;

        /// <summary>
        /// A referência de um Input de tradução.
        /// </summary>
        internal DbSet<Translation> TranslactionInput => this.TransactionObjects.GetObjectInputDataInternal<Translation>() as DbSet<Translation>;

        /// <summary>
        /// O serviço qual esse repositório representa.
        /// </summary>
        public FluentService<TE> Service { get; set; }

       // FluentService<TE> IFluentRepository<TE>.Service { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

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
        /// <param name="ispec">
        /// A especificação de requisição.
        /// </param>
        /// <param name="pagination">
        /// A paginação desejada.
        /// </param>
        /// <returns>
        /// A lista paginada de resultados.
        /// </returns>

        public virtual List<TO> ListSelect<TO>(IFluentSpecification<TO> ispec, FluentPagination pagination = null)
        {
            var spec = GetSpecSelect<TO>(ispec);
            var query = spec.ToIQueryable(Query);
            var fluentPagination = FluentPaginate(query, pagination);
            return fluentPagination.ToList();
        }

        /// <summary>
        /// Executa uma solicitação baseada em uma especificação e retorna um resultado ou nulo quando a consulta não é satisfeita.
        /// </summary>
        /// <typeparam name="TO">
        /// O tipo de saida desejada. Deve ser o mesmo definido na saida da especificação.
        /// </typeparam>
        /// <param name="ispec">
        /// A especificação de requisição.
        /// </param>
        /// <returns>
        /// O item referente à consulta ou nulo.
        /// </returns>

        public virtual TO FirstOrDefaultSelect<TO>(IFluentSpecification<TO> ispec)
        {
            var spec = GetSpecSelect<TO>(ispec);
            var iquerie = spec.ToIQueryable(Query);
            return iquerie.FirstOrDefault();
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

        public virtual int CountSelect<TO>(IFluentSpecification<TO> spec)
        {
            if (spec.FluentEntityType != typeof(TE))
            {
                var serviceName = $"{spec.FluentEntityType.Name}Service";
                throw new IncorrectDevelopmentException($"The type of input reported in the {spec} specification is not the same as that requested in the repository request.\r\nSpecification type: {spec.FluentEntityType}.\r\nRequisition Type: {typeof(TE)}\r\nThis usually occurs when you make use of the wrong service. Make sure that when invoking the method that is causing this error you are making use of the service: {serviceName}");
            }

            return GetSpecSelect<TO>(spec).ToIQueryable(Query).Count();
        }

        #endregion

        #region SQL

        /// <summary>
        /// Todo - Muito cuidado, pois se definir esse método como público, pode permitir vilnerabilidades no sistema por ser string sql.
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>


        internal bool ExistsSql(string sql)
        {
#if NET461
            return this.Input.SqlQuery(sql).Any();
#else
            return this.Input.FromSql(sql).Any();
#endif
        }

        internal TE FindSingleOrDefaultSql(string sql)
        {
#if NET461
            return this.Input.SqlQuery(sql).SingleOrDefault();

#else
            return this.Input.FromSql(sql).SingleOrDefault();
#endif
        }

        #endregion

        #region ENTITY ITEMS

        /// <summary>
        /// Executa uma solicitação baseada em uma especificação e retorna uma lista paginada de resultados.
        /// </summary>
        /// <param name="ispec">
        /// A especificação de requisição.
        /// </param>
        /// <param name="pagination">
        /// A paginação desejada.
        /// </param>
        /// <returns>
        /// A lista paginada de resultados.
        /// </returns>

        public virtual List<TE> List(IFluentSpecification ispec, FluentPagination pagination = null)
        {
            var spec = GetSpec(ispec);
            var query = spec.ToIQueryable(Query);
            return FluentPaginate(query, pagination).ToList();
        }

        //Todo não é ´permitido listar sem spec, pois sem spec não tem como ordenar pra paginar. Sem paginação pode ter sobrecarga.
        //Todo2 doc
        //
        //public virtual List<TE> List(FluentPagination pagination = null)
        //{
        //    return FluentPaginate(Query.ToIQueryable(Query), pagination).ToList();
        //}

        //Todo2 doc

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

        public virtual bool Exists(ISpec spec)
        {
            return GetSpec(spec).ToIQueryable(Query).Any();
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

        public virtual bool ExistsSelect<TO>(ISpec spec)
        {
            return GetSpecSelect<TO>(spec).ToIQueryable(Query).Any();
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

        public virtual int Count(IFluentSpecification spec)
        {
            return GetSpec(spec).ToIQueryable(Query).Count();
        }

        //Todo2 doc

        public virtual int Count()
        {
            return Query.Count();
        }

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

        public virtual void AddRange(params TE[] entities)
        {
            RunTheContextValidation();

            entities.ToList().ForEach(x => Input.Add(x));
        }


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

        public virtual TE Update(TE entity)
        {
            RunTheContextValidation();

            var currentEntity = Service.Find(entity);
            //Input.Attach(currentEntity);
            //TransactionObjects.Session.Entry(currentEntity).State = EntityState.Modified;
            ((DbContext)TransactionObjects.Session).Entry(currentEntity).CurrentValues.SetValues(entity);
            return entity;
        }

        /// <summary>
        /// Remove um item do banco de dados baseado em seu identificador.
        /// </summary>
        /// <param name="entity">
        /// Entidate a ser removida.
        /// </param>

        public virtual TE Remove(TE entity)
        {
            this.RunTheContextValidation();

#if NET461
            return this.Input.Remove(Service.Find(entity));
#else
            return this.Input.Remove(Service.Find(entity)).Entity;
#endif
        }

        public virtual void RemoveRange(IFluentSpecification spec)
        {
            var list = GetSpec(spec).ToIQueryable(Query).ToList();
            this.Input.RemoveRange(list);
        }


        public virtual void RemoveRange(params TE[] entities)
        {
            entities.ToList().ForEach(x => this.Remove(x));
        }

        #endregion

        #region INTERNAL

        private FluentSelectSpecification<TE, TO> GetSpecSelect<TO>(ISpec spec1)
        {
            if (spec1 is IFluentSpecification<TO> spec)
            {
                if (spec.FluentEntityType != typeof(TE))
                {
                    var serviceName = $"{spec.FluentEntityType.Name}Service";
                    throw new IncorrectDevelopmentException($"The type of input reported in the {spec} specification is not the same as that requested in the repository request.\r\nSpecification type: {spec.FluentEntityType}.\r\nRequisition Type: {typeof(TE)}\r\nThis usually occurs when you make use of the wrong service. Make sure that when invoking the method that is causing this error you are making use of the service: {serviceName}");
                }

                if (spec.FluentEntityOutType != typeof(TO))
                {
                    var serviceName = $"{typeof(TE).Name}Service";
                    throw new IncorrectDevelopmentException($"The type of output reported in the {spec} specification is not the same as that requested in the repository request.\r\nSpecification type: {spec.FluentEntityType}.\r\nRequisition Type: {typeof(TO)}\r\nThis usually occurs when you make use of the wrong service. Make sure that when invoking the method that is causing this error you are making use of the service: {serviceName}");
                }

                return spec as FluentSelectSpecification<TE, TO>;
            }

            throw new IncorrectDevelopmentException("The specification is of a different type than expected");
        }

        private FluentSpecification<TE> GetSpec(ISpec spec1)
        {
            if (spec1 is FluentSpecification<TE> spec)
            {
                return spec as FluentSpecification<TE>;
            }

            throw new IncorrectDevelopmentException("The specification is of a different type than expected");
        }

        // private static string CreateSqlFromKeys(TE entity)
        // {
        // var tableName = entity.GetTableName();
        // var keyValues = entity.GetKeyValues().Select(x => $"({x.Key} = {x.Value} and {x.Key} != 0)").ToArray();
        // var sql = $"select * from {tableName} where ";
        // sql += string.Join(" and ", keyValues);
        // return sql;
        // }

        private FluentPagination GetPagination()
        {
            var currentPageInt = int.TryParse(GetParameter("CurrentPage"), out var currentPageInt_) ? currentPageInt_ : 0;
            var itemsPerPageInt = int.TryParse(GetParameter("ItemsPerPage"), out var itemsPerPageInt_) ? itemsPerPageInt_ : 20;
            var startAtZeroBool = !bool.TryParse(GetParameter("StartAtZero"), out var startAtZeroBool_) || startAtZeroBool_;

            return new FluentPagination(currentPageInt, startAtZeroBool, itemsPerPageInt);
        }

        private string GetParameter(string key)
        {
#if NET461
            return Service.SessionRequest.LocalHttpContext.Request.Params.Get(key);
#else
            return Service.SessionRequest.LocalHttpContext.Request.Form[key];
#endif
        }

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

        protected IQueryable<TX> FluentPaginate<TX>(IQueryable<TX> query, FluentPagination pagination = null)
        {
            if (pagination == null)
            {
                pagination = GetPagination() ?? new FluentPagination(1, false, 255);
            }

            pagination.TotalQuantityOfItems = query.Count();
            query = query
                .Skip(pagination.Skip)
                .Take(pagination.ItemsPerPage);

            SessionRequest.Pagination = pagination;

            return query;
        }

        #endregion
    }
}

