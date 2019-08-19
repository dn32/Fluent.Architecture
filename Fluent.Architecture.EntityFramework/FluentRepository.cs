// -----------------------------------------------------------------------
// <copyright company="Fluent System">
//     Copyright © Fluent System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------

// ReSharper disable CommentTypo

using Fluent.Architecture.Core.Interfaces;
using Fluent.Architecture.Entities;
using Fluent.Architecture.Exceptions;
using Fluent.Architecture.Extensions;
using Fluent.Architecture.Interfaces;
using Fluent.Architecture.Repository;
using Fluent.Architecture.Services;
using Fluent.Architecture.Specifications;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo(@"Fluent.Architecture.EntityFramework.SqlServer, PublicKey=00240000048000009400000006020000002400005253413100040000010001002d98533364f3b3fbd11e7a3f14cd73d169e1daabd62ba2d1e5bc6a48a9bc709a503960db0e76c190e7a8dcefaed037e539682d6a891b242ddb91a3ab20fbfa0c04fb6304c8903857e1ed75399850fca4037dd2c810749e75770e5d455e950ccb9d06cf6fea5f30b00557a29408ce4c45021c412eca32616f47809bfe2cf404cc")]
[assembly: InternalsVisibleTo(@"Fluent.Architecture.EntityFramework.PostgreSQL, PublicKey=0024000004800000940000000602000000240000525341310004000001000100192d4ee01ba583399ab1d381c4301592f8520d29c628f3220e1550b2068e540e26886fa8d8b52618553f89fed1dccb18d5d3c07c548fca3c916a10823f411c23ef0e85bf0526ed94aa3cfbdf79a9595861348cfc369670f8ed9f7c4afd08de5f3cd87a0c7c6b1d8a0b94622c163a764813ba95d39dc44ea1baf7b663800a49bc")]
[assembly: InternalsVisibleTo(@"Fluent.Architecture.EntityFramework.MySQL, PublicKey=0024000004800000940000000602000000240000525341310004000001000100617593ae2b67e94c33ea38be9727f7a4a0e18fe316ea3cddceaaadd51d47546be3f27dc1d1c6c84d0a0cb43db45a7c476479c7ebd881d76b5dad404cafd086743036bd3c929dbf14c759ff504d798ca1097eb96b02dde75ee1bc120adc0e94553298c8749271502eb50cb427db851b1a26044bcb8e8fae1acf106069d2a349c0")]
namespace Fluent.Architecture.EntityFramework
{
    /// <inheritdoc />
    /// <summary>
    /// Repositório base com entidade do sistema baseado em Entity Framework.
    /// </summary>
    /// <typeparam name="TE">
    /// O tipo de entidade do repositório.
    /// </typeparam>
    public class FluentEFRepository<TE> : TransactionlRepository, IFluentRepository<TE> where TE : BaseEntity
    {
        public FluentEFRepository()
        {
        }

        #region PROPERTIES

        public ITransactionObjects TransactionObjects { get; set; }

        public virtual Type TransactionObjectsType => typeof(TransactionObjects);

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
        public virtual TE FirstOrDefault(IFluentSpecification spec)
        {
            var val = GetSpec(spec).ToIQueryable(Query);
            return val.FirstOrDefault();
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
            return this.Input.FromSql(sql).Any();
        }

        internal TE FindSingleOrDefaultSql(string sql)
        {
            return this.Input.FromSql(sql).SingleOrDefault();
        }

        /// <summary>
        /// Exemplo:
        ///   public int ProximoId()
        ///   {
        ///       int Leitor(DbDataReader reader)
        ///       {
        ///           return (int)reader[0];
        ///       }
        ///
        ///       return RawSqlQuery("SELECT TOP 10 Name, COUNT(*) FROM Users", Leitor).FirstOrDefault();
        ///   }
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="map"></param>
        /// <returns></returns>
        public List<T> RawSqlQuery<T>(string query, Func<DbDataReader, T> map)
        {
            using (var command = Session.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = query;
                command.CommandType = CommandType.Text;

                Session.Database.OpenConnection();

                using (var result = command.ExecuteReader())
                {
                    var entities = new List<T>();

                    while (result.Read())
                    {
                        entities.Add(map(result));
                    }

                    return entities;
                }
            }
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
            var sql = CreateSqlFromKeyOrFluentUniqueKeys(entity);
            return FindSingleOrDefaultSql(sql);
        }

        public virtual bool Exists(TE entity)
        {
            var sql = CreateSqlFromKeyOrFluentUniqueKeys(entity);
            return this.ExistsSql(sql);
        }

        public virtual void AddRange(params TE[] entities)
        {
            RunTheContextValidation();

            entities.ToList().ForEach(x => Input.Add(x));
        }


        public virtual TE Add(TE entity)
        {
            RunTheContextValidation();
            return Input.Add(entity).Entity;
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

        public virtual void UpdateRange(TE[] entities)
        {
            RunTheContextValidation();
            foreach (var entity in entities)
            {
                var currentEntity = Service.Find(entity);
                TransactionObjects.Session.Entry(currentEntity).CurrentValues.SetValues(entity);
            }
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
            return this.Input.Remove(Service.Find(entity, false)).Entity;
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
            Service.SessionRequest.LocalHttpContext.Request.Headers.TryGetValue(key, out StringValues value);
            if (!string.IsNullOrEmpty(value))
            {
                return value;
            }

            if (Service.SessionRequest.LocalHttpContext.Request.Method == "GET" || Service.SessionRequest.LocalHttpContext.Request.HasFormContentType == false)
            {
                return "";
            }

            return Service.SessionRequest.LocalHttpContext.Request?.Form[key];
        }

        private static string CreateSqlFromKeyOrFluentUniqueKeys(TE entity)
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

        protected IQueryable<TX> FluentPaginate<TX>(IOrderedQueryable<TX> query, FluentPagination pagination = null)
        {
            if (pagination == null)
            {
                pagination = GetPagination() ?? new FluentPagination(0, true, 20);
            }

            pagination.TotalQuantityOfItems = query.Count();
            SessionRequest.Pagination = pagination;

            return query.Skip(pagination.Skip).Take(pagination.ItemsPerPage);
        }

        #endregion
    }
}

