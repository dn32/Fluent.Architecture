//// -----------------------------------------------------------------------
//// <copyright company="DnControlador System">
////     Copyright © DnControlador System. All rights reserved.
////     TODOS OS DIREITOS RESERVADOS.
//// </copyright>
//// -----------------------------------------------------------------------

//using dn32.infra.EntityFramework;
//using dn32.infra.EntityFramework.SqLite;
//using Microsoft.AspNetCore.Hosting;
//using Microsoft.AspNetCore.Mvc.Testing;
//using Microsoft.Extensions.DependencyInjection;
//using Newtonsoft.Json;
//using Newtonsoft.Json.Serialization;
//using System;
//using System.Net.Http;
//using static dn32.infra.Test.IndexPageTests;

//namespace dn32.infra.Test
//{
//    public class IndexPageTests : IClassFixture<CustomWebApplicationFactory<RazorPagesProject.Startup>>
//    {
//        private readonly HttpClient _client;
//        private readonly CustomWebApplicationFactory<RazorPagesProject.Startup> _factory;

//        public IndexPageTests(
//            CustomWebApplicationFactory<RazorPagesProject.Startup> factory)
//        {
//            _factory = factory;
//            _client = factory.CreateClient(new WebApplicationFactoryClientOptions
//            {
//                AllowAutoRedirect = false
//            });
//        }

//        public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
//        {
//            private static long Ticks { get; set; } = DateTime.Now.Ticks;

//            public string ConnectionString { get; set; }

//            public void Initialize(string connectionString)
//            {
//                ConnectionString = connectionString;
//            }

//            protected override void ConfigureWebHost(IWebHostBuilder builder)
//            {
//                builder.ConfigureServices(services =>
//                {

//                    var jsonSerializerSettings = new ConfiguracoesDeSerializacao { ContractResolver = new CamelCasePropertyNamesContractResolver() };
//                    /* 1. Startup Arquitetura */
//                    services
//                        .AddMvc()
//                        .AddNewtonsoftJson(options => options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver())
//                        .AddDnArquitetura(jsonSerializerSettings)
//                        .UseEntityFramework()
//                        .AddConnectionString(string.IsNullOrWhiteSpace(ConnectionString) ? $"Data Source=unit-tests-{Ticks}.db;" : ConnectionString, createDatabaseIfNotExists: true, typeof(EfContextSqLite))
//                        .Build();

//                    //// Create a new service provider.
//                    //var serviceProvider = new ServiceCollection()
//                    //    .AddEntityFrameworkInMemoryDatabase()
//                    //    .BuildServiceProvider();

//                    //// Adicionar a database context (ApplicationDbContext) using an in-memory 
//                    //// database for testing.
//                    //services.AddDbContext<ApplicationDbContext>((options, context) =>
//                    //{
//                    //    context.UseInMemoryDatabase("InMemoryDbForTesting")
//                    //        .UseInternalServiceProvider(serviceProvider);
//                    //});

//                    //// Build the service provider.
//                    //var sp = services.BuildServiceProvider();

//                    //// Create a scope to obtain a Referencia to the database
//                    //// context (ApplicationDbContext).
//                    //using (var scope = sp.CreateScope())
//                    //{
//                    //    var scopedServices = scope.ServiceProvider;
//                    //    var db = scopedServices.GetRequiredService<ApplicationDbContext>();
//                    //    var logger = scopedServices
//                    //        .GetRequiredService<ILogger<CustomWebApplicationFactory<TStartup>>>();

//                    //    // Ensure the database is created.
//                    //    db.Database.EnsureCreated();

//                    //    try
//                    //    {
//                    //        // Seed the database with test data.
//                    //        Utilities.InitializeDbForTests(db);
//                    //    }
//                    //    catch (Exception ex)
//                    //    {
//                    //        logger.LogError(ex, "An error occurred seeding the " +
//                    //            "database with test messages. Error: {Mensagem}", ex.Mensagem);
//                    //    }
//                    //}
//                });
//            }
//        }
//    }
//}