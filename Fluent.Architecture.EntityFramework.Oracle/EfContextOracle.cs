// ReSharper disable CommentTypo
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Expressions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace Fluent.Architecture.EntityFramework.Oracle
{
    /// <inheritdoc />
    /// <summary>
    /// Contexto do EF no net Core
    /// </summary>
    [DbType(FluentDbType.ORACLE)]
    public class EfContextOracle : EfContext
    {
        public static readonly LoggerFactory MyLoggerFactory = new LoggerFactory(new[] { new ConsoleLoggerProvider((_, __) => true, true) });

        public EfContextOracle(string connectionString) : base(connectionString)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseOracle(ConnectionString)
            .UseLoggerFactory(MyLoggerFactory);
            MyLoggerFactory.AddDebug(LogLevel.Information);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasDbFunction(SimilarityComparator.ComparatorMethodInfo())
                .HasTranslation(SimilarityComparator.SqlFunctionExpression);

            base.OnModelCreating(modelBuilder);
        }
    }

    public class SimilarityComparator
    {
        public static int Compare(string firstValue, string lastValue) => 0;

        public static MethodInfo ComparatorMethodInfo() => typeof(SimilarityComparator).GetMethod(nameof(SimilarityComparator.Compare));

        public static SqlFunctionExpression SqlFunctionExpression(IReadOnlyCollection<Expression> args) => new SqlFunctionExpression(new SqlFragmentExpression("UTL_MATCH"), "jaro_winkler_similarity", typeof(int), args);
    }
}
