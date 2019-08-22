using Fluent.Architecture;
using Fluent.Architecture.Controllers;
using Fluent.Architecture.Core.Extensions;
using Fluent.Architecture.Core.Filters;
using Fluent.Architecture.EntityFramework;
using Fluent.Architecture.EntityFramework.Oracle;
using Fluent.Architecture.EntityFramework.SqLite;
using Fluent.Architecture.Test;
using Fluent.Architecture.Test.Mock;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

internal class DepartamentoControllerTest : FluentTest<Departamento>
{
    public DepartamentoControllerTest()
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

    public virtual DefaultPaginationResult ListByProximity(string term)
    {
        var newController = GetNewController();
        return TestUtil.Execute(newController, (FluentAPIController<Departamento> controller) => ((DepartamentoController)controller).ListByProximity(term)) as DefaultPaginationResult;
    }

    public override FluentAPIController<Departamento> GetNewController()
    {
        return MockUtil.GetMockController<DepartamentoController>();
    }

    [Test]
    public void ListByProximitySucess()
    {
        //Preparation
        var departamento1 = base.GetNew();
        var departamento2 = base.GetNew();
        var departamento3 = base.GetNew();
        var departamento4 = base.GetNew();

        departamento1.Descricao = "São Paulo";
        departamento2.Descricao = "Rio de Janeiro";
        departamento3.Descricao = "São Paulo";
        departamento4.Descricao = "Brasil";

        var departamentos = new[] { departamento1, departamento2, departamento3, departamento4 };

        base.AddRange(departamentos);


        //Operation
        var result = ListByProximity("Brasil");
        var lista = result.Data.JsonObjectToObject<List<Departamento>>();


        //Clear
        base.RemoveRange(departamentos);
    }
}
