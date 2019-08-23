//using Fluent.Architecture;
//using Fluent.Architecture.Controllers;
//using Fluent.Architecture.Core.Extensions;
//using Fluent.Architecture.Core.Filters;
//using Fluent.Architecture.EntityFramework;
//using Fluent.Architecture.EntityFramework.Oracle;
//using Fluent.Architecture.EntityFramework.SqLite;
//using Fluent.Architecture.Test;
//using Fluent.Architecture.Test.Mock;
//using Microsoft.AspNetCore.Http;
//using Microsoft.Extensions.Primitives;
//using Newtonsoft.Json.Linq;
//using NUnit.Framework;
//using System;
//using System.Collections.Generic;
//using System.Linq;

//internal class DepartamentoControllerTest : FluentTest<State>
//{
//    public DepartamentoControllerTest()
//    {
//        var Ticks = DateTime.Now.Ticks;

//        Fluent.Architecture.Setup
//               .Init()
//               .UseEntityFramework()
//               .AddConnectionString($"Data Source=unit-tests-{Ticks}.db;", createDatabaseIfNotExists: true, typeof(EfContextSqLite))
//               .AddConnectionString($"User ID=TESTEMANUAL2; Password=k23B67#jiY09; Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=10.62.38.63)(PORT=1721))(CONNECT_DATA=(SID = XE)))", createDatabaseIfNotExists: false, typeof(EfContextOracle))
//               .SetUserSessionRequestType(typeof(UserSessionRequestCustom))
//               .SetServiceProvider(null)
//               .Build()
//               .Run();
//    }

//    public virtual DefaultPaginationResult FindByProximity(string property, string term, int limit)
//    {
//        var newController = GetNewController();
//        return TestUtil.Execute(newController, (FluentAPIController<State> controller) => ((DepartamentoController)controller).FindByProximity(property, term, limit)) as DefaultPaginationResult;
//    }

//    public override FluentAPIController<State> GetNewController()
//    {
//        return MockUtil.GetMockController<DepartamentoController>();
//    }

//    [Test]
//    public void ListByProximitySucess()
//    {
//        //Preparation
//        var list = new State[]
//               {
//                    new State { Name = "Alabama", Code = "AL"},
//                    new State { Name = "Alaska", Code = "AK"},
//                    new State { Name = "Arizona", Code = "AZ"},
//                    new State { Name = "Arkansas", Code = "AR"},
//                    new State { Name = "California", Code = "CA"},
//                    new State { Name = "Colorado", Code = "CO"},
//                    new State { Name = "Connecticut", Code = "CT"},
//                    new State { Name = "Delaware", Code = "DE"},
//                    new State { Name = "Florida", Code = "FL"},
//                    new State { Name = "Georgia", Code = "GA"},
//                    new State { Name = "Hawaii", Code = "HI"},
//                    new State { Name = "Idaho", Code = "ID"},
//                    new State { Name = "Illinois", Code = "IL"},
//                    new State { Name = "Indiana", Code = "IN"},
//                    new State { Name = "Iowa", Code = "IA"},
//                    new State { Name = "Kansas", Code = "KS"},
//                    new State { Name = "Kentucky", Code = "KY"},
//                    new State { Name = "Louisiana", Code = "LA"},
//                    new State { Name = "Maine", Code = "ME"},
//                    new State { Name = "Maryland", Code = "MD"},
//                    new State { Name = "Massachusetts", Code = "MA"},
//                    new State { Name = "Michigan", Code = "MI"},
//                    new State { Name = "Minnesota", Code = "MN"},
//                    new State { Name = "Mississippi", Code = "MS"},
//                    new State { Name = "Missouri", Code = "MO"},
//                    new State { Name = "Montana", Code = "MT"},
//                    new State { Name = "Nebraska", Code = "NE"},
//                    new State { Name = "Nevada", Code = "NV"},
//                    new State { Name = "New Hampshire", Code = "NH"},
//                    new State { Name = "New Jersey", Code = "NJ"},
//                    new State { Name = "New Mexico", Code = "NM"},
//                    new State { Name = "New York", Code = "NY"},
//                    new State { Name = "North Carolina", Code = "NC"},
//                    new State { Name = "North Dakota", Code = "ND"},
//                    new State { Name = "Ohio", Code = "OH"},
//                    new State { Name = "Oklahoma", Code = "OK"},
//                    new State { Name = "Oregon", Code = "OR"},
//                    new State { Name = "Pennsylvania", Code = "PA"},
//                    new State { Name = "Rhode Island", Code = "RI"},
//                    new State { Name = "South Carolina", Code = "SC"},
//                    new State { Name = "South Dakota", Code = "SD"},
//                    new State { Name = "Tennessee", Code = "TN"},
//                    new State { Name = "Texas", Code = "TX"},
//                    new State { Name = "Utah", Code = "UT"},
//                    new State { Name = "Vermont", Code = "VT"},
//                    new State { Name = "Virginia", Code = "VA"},
//                    new State { Name = "Washington", Code = "WA"},
//                    new State { Name = "West Virginia", Code = "WV"},
//                    new State { Name = "Wisconsin", Code = "WI"},
//                    new State { Name = "Wyoming", Code = "WY"}
//               };

//        //Truncate("yes");

//       // base.AddRange(list);

//        //Operation
//        var result = FindByProximity(nameof(State.Name), "new iork", 80);
//        var lista = result.Data.JsonObjectToObject<List<State>>();

//        //Clear
//       // base.RemoveRange(list);
//    }
//}
