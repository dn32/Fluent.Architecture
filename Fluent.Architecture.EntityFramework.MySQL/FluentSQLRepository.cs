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

using System.Linq;
using Fluent.Architecture.Model;
using Fluent.Architecture.Core.Interfaces;
using Fluent.Architecture.Core.Attributes;
using Fluent.Architecture.Enumerator;

namespace Fluent.Architecture.EntityFramework.MySQL
{
    /// <inheritdoc />
    /// <summary>
    /// Repositório base com entidade do sistema baseado em Entity Framework.
    /// </summary>
    /// <typeparam name="TE">
    /// O tipo de entidade do repositório.
    /// </typeparam>
    [DbType(FluentDbType.MYSQL)]
    public class FluentSQLRepository<TE> : FluentEFRepository<TE>, IFluentRepository<TE> where TE : BaseEntity
    {
        #region SQL

        /// <summary>
        /// Todo - Muito cuidado, pois se definir esse método como público, pode permitir vilnerabilidades no sistema por ser string sql.
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        internal override bool ExistsSql(string sql)
        {
#if NET461
            return this.Input.SqlQuery(sql).Any();
#else
            return this.Input.FromSql(sql).Any();
#endif
        }

        internal override TE FindSingleOrDefaultSql(string sql)
        {
#if NET461
            return this.Input.SqlQuery(sql).SingleOrDefault();
#else
            return this.Input.FromSql(sql).SingleOrDefault();
#endif
        }

        #endregion
    }
}

