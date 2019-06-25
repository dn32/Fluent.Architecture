// -----------------------------------------------------------------------
// <copyright company="Fluent System">
//     Copyright © Fluent System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------

using Fluent.Architecture.Entities;
using System.Runtime.InteropServices;

namespace Fluent.Architecture.Test
{
    public class UserSessionRequestCustom : UserSessionRequest
    {
        public int Teste { get; set; }
    }


    [ComVisible(true)]
    public static class Setup
    {
        public static void Initialize(string connectionString)
        {

            Fluent.Architecture.Setup
                               .Init()
                               .SetUserSessionRequestType(typeof(UserSessionRequestCustom))
                               .Build()
                               .Run();

            // Fluent.Architecture.Setup.Initialize(connectionString, true);
            // Architecture.Setup.SetCustomTypes(transactionObjectsType: typeof(TransactionObjectsTest));
        }
    }
}
