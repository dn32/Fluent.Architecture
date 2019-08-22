using Fluent.Architecture;
using Fluent.Architecture.EntityFramework;
using Fluent.Architecture.EntityFramework.Oracle;
using Fluent.Architecture.EntityFramework.SqLite;
using Fluent.Architecture.Test;
using System;


internal class InternalUserTestBase : FluentTest<User>
{
    public InternalUserTestBase()
    {
        var Ticks = DateTime.Now.Ticks;

          Fluent.Architecture.Setup
                 .Init()
                 .UseEntityFramework()
                 .AddConnectionString($"Data Source=unit-tests-{Ticks}.db;", createDatabaseIfNotExists: true, typeof(EfContextSqLite))
                 .AddConnectionString($"User ID=TESTEMANUAL2; Password=k23B67#jiY09; Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=10.62.38.63)(PORT=1721))(CONNECT_DATA=(SID = XE)))", createDatabaseIfNotExists: false, typeof(EfContextOracle))
                 .SetUserSessionRequestType(typeof(UserSessionRequestCustom))
                 .SetServiceProvider(null)
                 .Build()
                 .Run();
    }

    protected void SetCategory(int category)
    {
        Category = category;
    }

    private int Category { get; set; }

    public override User GetNew()
    {
        var rand = TestUtil.NextRandom();
        if (Category == 0) { Category = rand; }

        return new User
        {
            PersonType = EnumPersonType.User,
            Id = rand,
            UserName = $"maria {rand}",
            Name = $"maria {rand}",
            Email = $"test{rand}@mail.com",
            Password = $"test{rand}@mail.com",
            Tel = $"test{rand}@mail.com",
            ZipCode = rand,
            Category = Category
        };
    }
}
