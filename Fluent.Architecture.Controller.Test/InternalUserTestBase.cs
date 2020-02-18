using Fluent.Architecture.Test;
using Fluente.Arquitetura.Test;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using NUnit.Framework;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xunit;

internal class InternalUserTestBase : FluenteTest<User>
{
    public class APIWebApplicationFactory : WebApplicationFactory<SimpleHelloWorld.Startup>
    {
        public APIWebApplicationFactory()
        {
          
        }
    }

    [TestFixture]
    public class SampleControllerTests
    {
        private APIWebApplicationFactory _factory;
        private HttpClient _client;

        [OneTimeSetUp]
        public void GivenARequestToTheController()
        {
            _factory = new APIWebApplicationFactory();
              _client = _factory
           .WithWebHostBuilder(builder => builder.UseSolutionRelativeContentRoot("Public/01-SimpleHelloWorld"))
           .CreateClient();
        }

        [Test]
        public async Task WhenSomeTextIsPosted_ThenTheResultIsOk()
        {
            var textContent = new ByteArrayContent(Encoding.UTF8.GetBytes("Backpack for his applesauce"));
            textContent.Headers.ContentType = new MediaTypeHeaderValue("text/plain");

            var result = await _client.PostAsync("/sample", textContent);
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        public async Task WhenNoTextIsPosted_ThenTheResultIsBadRequest()
        {
            var result = await _client.PostAsync("/sample", new StringContent(string.Empty));
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _client.Dispose();
            _factory.Dispose();
        }
    }

    //public class BasicTests//    : ClassInitialize<WebApplicationFactory<SimpleHelloWorld.Startup>>
    //{
    //    private readonly WebApplicationFactory<SimpleHelloWorld.Startup> _factory;

    //    public BasicTests(WebApplicationFactory<SimpleHelloWorld.Startup> factory)
    //    {
    //        _factory = factory;
    //    }

    //    [NUnit.Framework.Theory]
    //    [InlineData("/")]
    //    [InlineData("/Index")]
    //    [InlineData("/About")]
    //    [InlineData("/Privacy")]
    //    [InlineData("/Contact")]
    //    public async Task Get_EndpointsReturnSuccessAndCorrectContentType(string url)
    //    {
    //        // Arrange
    //        var client = _factory.CreateClient();

    //        // Act
    //        var response = await client.GetAsync(url);

    //        // Assert
    //        response.EnsureSuccessStatusCode(); // Status Code 200-299
    //        Assert.Equals("text/html; charset=utf-8",
    //            response.Content.Headers.ContentType.ToString());
    //    }
    //}
    //    public InternalUserTestBase()
    //{
    //    var Ticks = DateTime.Now.Ticks;

    //      Fluent.Architecture.Setup
    //             .Init()
    //             .UseEntityFramework()
    //             .AddConnectionString($"Data Source=unit-tests-{Ticks}.db;", createDatabaseIfNotExists: true, typeof(EfContextSqLite))
    //             .AddConnectionString($"User ID=TESTEMANUAL2; Password=k23B67#jiY09; Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=10.62.38.63)(PORT=1721))(CONNECT_DATA=(SID = XE)))", createDatabaseIfNotExists: false, typeof(EfContextOracle))
    //             .SetUserSessionRequestType(typeof(UserSessionRequestCustom))
    //             .Build();
    //}

    //protected void SetCategory(int category)
    //{
    //    Category = category;
    //}

    //private int Category { get; set; }

    //public override User GetNew()
    //{
    //    var rand = RandomUtil.NextRandom();
    //    if (Category == 0) { Category = rand; }

    //    return new User
    //    {
    //        PersonType = EnumPersonType.User,
    //        Id = rand,
    //        UserName = $"maria {rand}",
    //        Name = $"maria {rand}",
    //        Email = $"test{rand}@mail.com",
    //        Password = $"test{rand}@mail.com",
    //        Tel = $"test{rand}@mail.com",
    //        ZipCode = rand,
    //        Category = Category
    //    };
    //}
}
