using Fluente.Arquitetura.Base.Models;
using Fluente.Arquitetura.Base.Atributos;
using Fluente.Arquitetura.Base.Enumeradores;
using Fluente.Arquitetura.Base.Extensoes;
using Fluente.Arquitetura.Interfaces;
using Fluente.Arquitetura.Exceptions;
using Fluente.Arquitetura.Extensoes;
using Fluente.Arquitetura.Interfaces;
using Fluente.Arquitetura.Nucleo.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Fluente.Arquitetura.Nucleo.Atributos;
using System.Collections;
using System.Linq.Expressions;
using System.Reflection;

namespace Fluente.Arquitetura.EntityFramework
{
    public partial class FluenteEFRepository<TE>
    {
        internal protected async Task<int> CountSqlAsync(string sql, bool includeExcludedLogically = false)
        {
            if (includeExcludedLogically)
            {
                lock (SessionRequest)
                {
                    Session.EnableLogicalDeletion = false;
                }
            }

            var ret = await FromSql(sql).CountAsync();

            lock (SessionRequest)
            {
                Session.EnableLogicalDeletion = true;
            }

            return ret;
        }

        internal protected async Task<TO> FindSingleOrDefaultSqlAsync<TO>(string sql) where TO : EntidadeBase
        {
            try
            {
                return await FromSqlSelect<TO>(sql).SingleOrDefaultAsync();
            }
            catch (InvalidOperationException)
            {
                throw new InvalidOperationException($"More than one record was found with the given keys. This is an indication of data with duplicate keys in the database. The table is {typeof(TE).GetTableName()}");
            }
        }

        public virtual async Task<bool> ExistsAsync(ISpec spec)
        {
            return await GetSpec(spec).ToIQueryable(Query).AnyAsync();
        }

        public virtual async Task<bool> ExistsSelectAsync<TO>(ISpec spec)
        {
            return await GetSpecSelect<TO>(spec).ToIQueryable(Query).AnyAsync();
        }

        public virtual async Task<List<TE>> ListAsync(IFluenteSpecification ispec, FluentePaginacao pagination = null)
        {
            var spec = GetSpec(ispec);
            var query = spec.ToIQueryable(Query);
            var taskList = await FluentePaginateAsync(query, pagination);
            return await taskList.ToListAsync();
        }

        public virtual async Task<int> CountAsync(TE entity, bool includeExcludedLogically = false)
        {
            var sql = RepositoryUtil.GetKeyAndFluenteUniqueKeyFilterSql(entity);
            return await CountSqlAsync(sql, includeExcludedLogically);
        }

        public virtual async Task<int> CountSelectAsync<TO>(IFluenteSpecification<TO> spec)
        {
            if (spec.FluenteEntityType != typeof(TE))
            {
                var serviceName = $"{spec.FluenteEntityType.Name}Service";
                throw new IncorrectDevelopmentException($"The type of input reported in the {spec} specification is not the same as that requested in the repository request.\r\nSpecification type: {spec.FluenteEntityType}.\r\nRequisition Type: {typeof(TE)}\r\nThis usually occurs when you make use of the wrong service. Make sure that when invoking the method that is causing this error you are making use of the service: {serviceName}");
            }

            return await GetSpecSelect<TO>(spec).ToIQueryable(Query).CountAsync();
        }

        public virtual async Task<int> CountAsync(IFluenteSpecification spec)
        {
            return await GetSpec(spec).ToIQueryable(Query).CountAsync();
        }

        public virtual async Task<int> CountAsync()
        {
            return await Query.CountAsync();
        }

        public virtual async Task<bool> ExistsOnlyOneAsync(TE entity, bool includeExcludedLogically = false)
        {
            var sql = RepositoryUtil.GetKeyAndFluenteUniqueKeyFilterSql(entity);
            return await CountSqlAsync(sql, includeExcludedLogically) == 1;
        }


        #region SPEC TE

        public virtual async Task<TE> FirstOrDefaultAsync(IFluenteSpecification spec)
        {
            var val = GetSpec(spec).ToIQueryable(Query);
            return await val.FirstOrDefaultAsync();
        }

        public virtual async Task<TO> SingleOrDefaultSelectAsync<TO>(IFluenteSpecification<TO> spec)
        {
            var query = GetSpecSelect<TO>(spec).ToIQueryable(Query);
            return await query.FirstOrDefaultAsync();
        }

        public virtual async Task<TE> SingleOrDefaultAsync(IFluenteSpecification spec)
        {
            var val = GetSpec(spec).ToIQueryable(Query);

            try
            {
                return await val.SingleOrDefaultAsync();
            }
            catch (InvalidOperationException)
            {
                throw new InvalidOperationException($"More than one record was found with the given keys. This is an indication of data with duplicate keys in the database. The table is {typeof(TE).GetTableName()}");
            }
        }

        #endregion

        public virtual async Task<List<TO>> ListSelectAsync<TO>(IFluenteSpecification<TO> ispec, FluentePaginacao pagination = null)
        {
            var spec = GetSpecSelect<TO>(ispec);
            var query = spec.ToIQueryable(Query);
            var fluentPagination = await FluentePaginateAsync(query, pagination);
            return await fluentPagination.ToListAsync();
        }

        public virtual async Task<TO> FirstOrDefaultSelectAsync<TO>(IFluenteSpecification<TO> ispec)
        {
            var spec = GetSpecSelect<TO>(ispec);
            var query = spec.ToIQueryable(Query);
            return await query.FirstOrDefaultAsync();
        }

        internal protected async Task<bool> ExistsSqlAsync(string sql, bool includeExcludedLogically = false)
        {
            if (includeExcludedLogically)
            {
                lock (SessionRequest)
                {
                    Session.EnableLogicalDeletion = false;
                }
            }

            var ret = await FromSql(sql).AnyAsync();

            lock (SessionRequest)
            {
                Session.EnableLogicalDeletion = true;
            }

            return ret;
        }

        internal protected async Task<TE> FindSingleOrDefaultSqlAsync(string sql)
        {
            try
            {
                return await FromSql(sql).SingleOrDefaultAsync();
            }
            catch (InvalidOperationException)
            {
                throw new InvalidOperationException($"More than one record was found with the given keys. This is an indication of data with duplicate keys in the database. The table is {typeof(TE).GetTableName()}");
            }
        }

        #region ENTITY ITEMS

        public virtual async Task<TE> FindAsync(TE entity) => await FindSelectAsync(entity);

        public async Task<TO> FindSelectAsync<TO>(TO entity) where TO : EntidadeBase
        {
            {
                var sql = RepositoryUtil.GetKeyFilterSql(entity, out bool nonKeys);
                if (nonKeys == false)
                {
                    var valueFound = await FindSingleOrDefaultSqlAsync<TO>(sql);
                    if (valueFound != null)
                    {
                        return valueFound;
                    }
                }
            }

            {
                var sql = RepositoryUtil.GetFluenteUniqueKeyFilterSql(entity, out bool nonKeys);
                if (nonKeys == false)
                {
                    var valueFound = await FindSingleOrDefaultSqlAsync<TO>(sql);
                    if (valueFound != null)
                    {
                        return valueFound;
                    }
                }
            }

            return null;
        }

        public virtual async Task<bool> ExistsAsync(TE entity, bool includeExcludedLogically = false)
        {
            var sql = RepositoryUtil.GetKeyAndFluenteUniqueKeyFilterSql(entity);
            return await ExistsSqlAsync(sql, includeExcludedLogically);
        }

        public virtual async Task AddRangeAsync(params TE[] entities)
        {
            RunTheContextValidation();
            foreach (var entity in entities)
            {
                DefineForeignKeyOfCompositionsOrAggregations(entity);
                await UpdateCompositionListAsync(entity, false);
            }

            await Input.AddRangeAsync(entities);
        }

        protected async Task<IQueryable<TX>> FluentePaginateAsync<TX>(IQueryable<TX> query, FluentePaginacao pagination = null)
        {
            if (pagination == null)
            {
                pagination = GetPagination() ?? FluentePaginacao.Criar(0, true, 20);
            }

            pagination.QuantidadeTotalDeItens = await query.CountAsync();
            SessionRequest.Pagination = pagination;

            return query.Skip(pagination.Salto).Take(pagination.ItensPorPagina);
        }

        public virtual async Task<TE> AddAsync(TE entity)
        {
            RunTheContextValidation();
            DefineForeignKeyOfCompositionsOrAggregations(entity);
            await UpdateCompositionListAsync(entity, false);
            await CompleteEmptyKeysAsync(entity);
            var ret = await Input.AddAsync(entity);
            return ret.Entity;
        }

        #endregion

        protected async Task<int> ExecuteSqlQueryAsync(string query)
        {
            using var command = Session.Database.GetDbConnection().CreateCommand();
            command.CommandText = query;
            command.CommandType = CommandType.Text;
            await Session.Database.OpenConnectionAsync();
            return await command.ExecuteNonQueryAsync();
        }

        public virtual async Task<TE> RemoveAsync(TE entity)
        {
            RunTheContextValidation();
            var teEntity = await Service.FindAsync(entity, false);
            var ret = Input.Remove(teEntity).Entity;

            RemoveFluenteCompositionsAndFluenteAggregations(entity);
            return ret;
        }

        private void RemoveFluenteCompositionsAndFluenteAggregations(TE entity)
        {
            var type = entity.GetType();
            if (type.GetCustomAttribute<FluenteJsonFormAttribute>()?.IsIntermediateTable == true) { return; }

            var compositionProperties = type.GetProperties().Where(x => x.GetCustomAttributeAny<FluenteCompositionAttribute>() || x.GetCustomAttributeAny<FluenteManyToManyAggregationAttribute>());
            foreach (var compositionProperty in compositionProperties)
            {
                var compositionPropertyType = compositionProperty.PropertyType;
                var compositionListElements = ListAllByForeignKey(entity, compositionPropertyType.GetListTypeNonNull());
                var dbSet = TransactionObjects.GetObjectInputDataInternal(compositionPropertyType.GetListTypeNonNull());
                if (compositionListElements.Count > 0)
                {
                    var method = dbSet.GetType().GetMethods().Last(x => x.Name == "RemoveRange");
                    method.Invoke(dbSet, new[] { compositionListElements });
                }
            }
        }

        public virtual async Task TruncateAsync()
        {
            var tableName = typeof(TE).GetTableName();
            var sql = $"TRUNCATE TABLE {tableName}";
            await ExecuteSqlQueryAsync(sql);
        }

        /// <summary>
        /// Exemplo:
        ///   public async Task<int> ProximoId()
        ///   {
        ///       int Leitor(DbDataReader reader)
        ///       {
        ///           return (int)reader[0];
        ///       }
        ///
        ///       return await RawSqlQueryAsync("SELECT TOP 10 Name, COUNT(*) FROM Users", Leitor).FirstOrDefault();
        ///   }
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="map"></param>
        /// <returns></returns>
        protected async Task<List<T>> RawSqlQueryAsync<T>(string query, Func<DbDataReader, T> map)
        {
            using var command = Session.Database.GetDbConnection().CreateCommand();
            command.CommandText = query;
            command.CommandType = CommandType.Text;

            await Session.Database.OpenConnectionAsync();

            using var result = await command.ExecuteReaderAsync();
            var entities = new List<T>();

            while (await result.ReadAsync())
            {
                entities.Add(map(result));
            }

            return entities;
        }
    }
}
