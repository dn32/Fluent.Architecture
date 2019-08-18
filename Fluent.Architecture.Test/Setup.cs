// -----------------------------------------------------------------------
// <copyright company="Fluent System">
//     Copyright © Fluent System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------

using Fluent.Architecture.Entities;
using Fluent.Architecture.EntityFramework;
using Fluent.Architecture.EntityFramework.SqLite;
using System;
using System.Runtime.InteropServices;

namespace Fluent.Architecture.Test
{
    public class UserSessionRequestCustom : UserSessionRequest
    {
    }

    [ComVisible(true)]
    public static class Setup
    {
        private static long Ticks { get; set; } = DateTime.Now.Ticks;

        public static void Initialize(string connectionString)
        {
            Architecture.Setup
              .Init()
              .UseEntityFramework()
              .AddConnectionString(string.IsNullOrWhiteSpace(connectionString) ? $"Data Source=unit-tests-{Ticks}.db;" : connectionString, createDatabaseIfNotExists: true, typeof(EfContextSqLite))
              .SetUserSessionRequestType(typeof(UserSessionRequestCustom))
              .SetServiceProvider(null)
              .Build()
              .Run();
        }
    }
}