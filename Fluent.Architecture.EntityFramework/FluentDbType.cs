namespace Fluent.Architecture.EntityFramework
{
#if NET461
    public enum FluentDbType
    {
        SQL_SERVER,
    }
#else
    public enum FluentDbType
    {
        SQL_SERVER,
        ORACLE,
        RAVENDB,
        MYSQL,
        POSTGREE_SQL
    }
#endif
}
