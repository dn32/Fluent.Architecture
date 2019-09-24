// -----------------------------------------------------------------------
// <copyright company="Fluent System">
//     Copyright © Fluent System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------

// ReSharper disable CommentTypo
using Fluent.Architecture.Core.Factory;
using Fluent.Architecture.Exceptions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo(@"Fluent.Architecture.EntityFramework, PublicKey=002400000480000094000000060200000024000052534131000400000100010001e5fbcd7e6f1d70524fc7b787a6ba4d8f332e822c5506e1831f4e59ab41e930c56bbf8cc29fa91f1270f4e873c036335c5aa4ccfc76ab13bfa7372de9d4e17de6c2d188fae9e6842d7d90d51e123836fd9f5d6be5580a32d1a12e59489519c6b93cdcf7ecd782042db1f31190350fbf937bbd6a5ae61d648773b46b9a706ccf")]
namespace Fluent.Architecture
{
    public static class FluentArchitectureSetup
    {
        //public static Config UseFluentArchitecture(this IApplicationBuilder app)
        //{
        //    if (Setup.ConfigInstance is Config)
        //    {
        //        return Setup.ConfigInstance.SetServiceProvider(app.ApplicationServices);
        //    }
        //    else
        //    {
        //        throw new IncorrectDevelopmentException("In Startup.cs, in the ConfigureServices method, add a call to services.AddFluentArchitecture().");
        //    }
        //}

        public static Config AddFluentArchitecture(this IServiceCollection services)
        {
            Setup.ClientServices = services;

            Setup.InternalInitialize();

            services
                .AddMvc()
                .ConfigureApplicationPartManager(apm => apm.FeatureProviders.Add(new ControllerFactory()));

            return Setup.Init();
        }
    }
}
