// -----------------------------------------------------------------------
// <copyright company="Fluent System">
//     Copyright © Fluent System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------

// ReSharper disable CommentTypo

using Fluent.Architecture.Core.Attributes;
using Fluent.Architecture.Core.Controllers.ControllerModel;
using Fluent.Architecture.Core.Interfaces;
using Fluent.Architecture.Core.Util;
using Fluent.Architecture.Entities;
using Fluent.Architecture.Exceptions;
using Fluent.Architecture.Extensions;
using Fluent.Architecture.Interfaces;
using Fluent.Architecture.Services;
using Fluent.Architecture.Specifications;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.Common;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

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
    public partial class FluentEFRepository<TE> : IFluentRepository<TE> where TE : BaseEntity
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

        #region COMPOSITION

        protected void UpdateCompositionList(TE entity)
        {
            var compositionProperties = entity.GetType().GetProperties().Where(x => x.GetCustomAttributeAny<FluentCompositionAttribute>());
            foreach (var compositionProperty in compositionProperties)
            {
                var compositionValue = compositionProperty.GetValue(entity);
                CompleteEmptyKeys(compositionValue);

                var compositionPropertyType = compositionProperty.PropertyType;

                if (compositionPropertyType.IsList())
                {
                    var listType = compositionPropertyType.GenericTypeArguments[0];
                    var allPersistedForThisEntity = ListAllByForeignKey(entity, listType).FluentCast<IList>();
                    var compositionListValue = compositionValue.FluentCast<IList>();

                    var allPersistedForThisEntityForRemove = allPersistedForThisEntity;
                    if (compositionListValue != null)
                    {
                        foreach (var item in compositionListValue)
                        {
                            allPersistedForThisEntityForRemove.Remove(item);
                        }
                    }

                    if (allPersistedForThisEntityForRemove.Count > 0)
                    {
                        foreach (var entityToRemove in allPersistedForThisEntityForRemove)
                        {
                            Session.Remove(entityToRemove);
                        }
                    }

                    if (compositionListValue != null)
                    {
                        foreach (var auth in compositionListValue)
                        {
                            var currentEntity = Find(auth);
                            if (currentEntity == null)
                            { //Add
                                Session.Add(auth);
                            }
                            else
                            { //update
                                TransactionObjects.Session.Entry(currentEntity).CurrentValues.SetValues(auth);
                            }
                        }
                    }
                }
                else
                {
                    var list = ListAllByForeignKey(entity, compositionPropertyType).FluentCast<IList>();
                    var currentEntity = list.Count == 1 ? list[0] : null;

                    if (currentEntity == null)
                    {
                        if (compositionValue == null)
                        { // Não tem no bd e nem no objeto
                            continue;
                        }
                        else
                        { // Não tem no BD e precisa adicionar
                            Session.Add(compositionValue);
                            continue;
                        }
                    }
                    else
                    {
                        if (compositionValue == null)
                        { // Tem no bd, mas precisa ser removido
                            Session.RemoveRange(currentEntity);
                            continue;
                        }
                        else
                        { // Tem no bd e precisa ser atualizado

                            var keyProperties = currentEntity.GetType().GetProperties().Where(x => x.GetCustomAttributeAny<KeyAttribute>()).ToList();
                            foreach (var p in keyProperties)
                            {
                                var value = p.GetValue(currentEntity);
                                if (value != null)
                                {
                                    p.SetValue(compositionValue, value);
                                }
                            }

                            TransactionObjects.Session.Entry(currentEntity).CurrentValues.SetValues(compositionValue);
                            continue;
                        }
                    }
                }
            }
        }

        protected void DefineForeignKeyOfCompositions(TE entity)
        {
            var localProperties = entity.GetType().GetProperties();

            localProperties.ToList().ForEach(LocalProperty =>
            {
                var composition = LocalProperty.GetCustomAttribute<FluentCompositionAttribute>();
                if (composition == null) { return; }
                for (int i = 0; i < composition.ExternalKeys.Length; i++)
                {
                    var externalKey = composition.ExternalKeys[i];
                    var localKey = composition.LocalKeys[i];
                    var localValue = localProperties.Single(x => x.Name == localKey).GetValue(entity);

                    for (int i2 = 0; i < composition.ExternalKeys.Length; i++)
                    {
                        var ext = composition.ExternalKeys[i2];
                        var loca = composition.LocalKeys[i2];

                        if (LocalProperty.PropertyType.IsList())
                        {
                            if (LocalProperty.GetValue(entity) is ICollection List)
                            {
                                foreach (var item in List)
                                {
                                    var property = item.GetType().GetProperty(ext);
                                    property.SetValue(item, localValue);
                                }
                            }
                        }
                        else
                        {
                            var externalProperty = LocalProperty.PropertyType.GetProperty(ext);
                            var propertyValue = LocalProperty.GetValue(entity);
                            if (propertyValue != null)
                            {
                                externalProperty.SetValue(propertyValue, localValue);
                            }
                        }
                    }
                }
            });
        }

        private void CompleteEmptyKeys(object compositionValue)
        {
            if (compositionValue == null) { return; }
            var keyPoroperties = compositionValue.GetType().GetProperties().Where(x => x.GetCustomAttributeAny<FluentRandomKeyValueOnAdd>()).ToList();
            //Todo - Permitir esse atributo somente em tipos primitivos
            foreach (var property in keyPoroperties)
            {
                var type = property.PropertyType;
                var value = property.GetValue(compositionValue);
                if (value == type.GetDefaultValue())
                {
                    List<object> notExists;

                    do
                    {
                        var list = new object[10];
                        for (int i = 0; i < list.Length; i++)
                        {
                            list[i] = RandomUtil.GetRandomValue(property);
                        }
                        //Essa operação deve ser async
                        var exists = ExistOnList(property, list).FluentCast<IEnumerable<object>>(); // Verificar se esse cast vai funcionar
                        notExists = list.Except(exists).ToList();
                    }
                    while (notExists.Count == 0);

                    value = notExists.First();
                    property.SetValue(compositionValue, value);
                }
            }
        }

        #endregion

        #region SQL

        internal ICollection ExistOnList(PropertyInfo property, object[] elements)
        {
            var outType = property.PropertyType;
            var sql = RepositoryUtil.ListToInSql(outType, elements, property);
            var query = FromSqlByType(sql, outType);
            var list = typeof(Enumerable).GetMethod(nameof(Enumerable.ToList)).MakeGenericMethod(outType).Invoke(null, new object[] { query }).FluentCast<ICollection>();
            return list;
        }

        private IQueryable FromSqlByType(string sql, Type outType, params object[] parameters)
        {
            return GetType().GetMethod(nameof(FromSqlSelect), BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).MakeGenericMethod(outType).Invoke(this, new object[] { sql, parameters }).FluentCast<IQueryable>();
        }

        internal protected IQueryable<TE> FromSql(string sql, params object[] parameters)
        {
            return FromSqlSelect<TE>(sql, parameters);
        }

        internal protected IQueryable<TO> FromSqlSelect<TO>(string sql, params object[] parameters) where TO : BaseEntity
        {
            var source = TransactionObjects.GetObjectInputDataInternal<TO>();

#if NETCOREAPP3_0
            return source.FromSqlRaw(sql, parameters);
#else
            return source.FromSql(sql, parameters);
#endif
        }

        internal protected ICollection ListAllNotPaginate(string sql, Type outType)
        {
            var query = FromSqlByType(sql, outType);
            return typeof(Enumerable).GetMethod(nameof(Enumerable.ToList)).MakeGenericMethod(outType).Invoke(null, new object[] { query }).FluentCast<ICollection>();
        }



        #endregion

        #region ENTITY ITEMS

        public virtual object Find(object entity)
        {
            var type = entity.GetType();
            var method = GetType().GetMethod(nameof(FindSelectAsync)).MakeGenericMethod(type);
            return method.Invoke(this, new object[] { entity });
        }


        public virtual ICollection ListAllByForeignKey(TE entity, Type outType)
        {
            var sql = RepositoryUtil.GetForeignKeyFilterSql(entity, outType, out bool nonKeys);
            if (nonKeys == false)
            {
                return ListAllNotPaginate(sql, outType);
            }

            return default;
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

            DefineForeignKeyOfCompositions(entity);

            lock (SessionRequest)
            {
                Session.EnableLogicalDeletion = false;
            }

            var currentEntity = Service.FindAsync(entity);

            TransactionObjects.Session.Entry(currentEntity).CurrentValues.SetValues(entity);

            lock (SessionRequest)
            {
                Session.EnableLogicalDeletion = true;
            }

            UpdateCompositionList(entity);

            return entity;
        }


        //Todo - tratar recuperação de exclusão lógica, como foi feito no Update
        public TE UpdateAlter(UpdateAlter<TE> value)
        {
            RunTheContextValidation();

            DefineForeignKeyOfCompositions(value.Final);
            var currentEntity = Service.FindAsync(value.Original);
            TransactionObjects.Session.Entry(currentEntity).CurrentValues.SetValues(value.Final);
            UpdateCompositionList(value.Final);
            return value.Final;
        }

        //Todo - tratar recuperação de exclusão lógica, como foi feito no Update
        public virtual void UpdateRange(TE[] entities)
        {
            RunTheContextValidation();
            foreach (var entity in entities)
            {
                DefineForeignKeyOfCompositions(entity);
                var currentEntity = Service.FindAsync(entity);
                TransactionObjects.Session.Entry(currentEntity).CurrentValues.SetValues(entity);
                UpdateCompositionList(entity);
            }
        }

        /// <summary>
        /// Remove um item do banco de dados baseado em seu identificador.
        /// </summary>
        /// <param name="entity">
        /// Entidate a ser removida.
        /// </param>

        public virtual async Task<TE> RemoveAsync(TE entity)
        {
            RunTheContextValidation();
            var teEntity = await Service.FindAsync(entity, false);
            return Input.Remove(teEntity).Entity;
        }

        public virtual void RemoveRange(IFluentSpecification spec)
        {
            var list = GetSpec(spec).ToIQueryable(Query).ToList();
            this.Input.RemoveRange(list);
        }

        public virtual async Task TruncateAsync()
        {
            var tableName = typeof(TE).GetTableName();
            var sql = $"TRUNCATE TABLE {tableName}";
            await ExecuteSqlQueryAsync(sql);
        }


        public virtual void RemoveRange(params TE[] entities)
        {//Todo melhorar isso e tornar async
            entities.ToList().ForEach(async x => await RemoveAsync(x));
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

        protected FluentSpecification<TE> GetSpec(ISpec spec1)
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


        #endregion
    }
}

