using Fluent.Architecture.Model;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Fluent.Architecture.Sample.Core
{
    public class Course : FluentIdEntity
    {
        public string Name { get; set; }
    }

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            var connectionString = "Data Source=localhost\\SQLEXPRESS; Database=test_db_core; Integrated Security=True";

            Fluent.Architecture.Setup.Initialize(connectionString, createDatabaseIfNotExists: true);

            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc(routes =>
            {
            });
        }
    }
}
