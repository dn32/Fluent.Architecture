// -----------------------------------------------------------------------
// <copyright company="Fluent System">
//     Copyright © Fluent System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------

using System.Runtime.InteropServices;

namespace Fluent.Architecture.Test
{
    [ComVisible(true)]
    public static class Setup
    {
        public static void Initialize(string connectionString)
        {
            Fluent.Architecture.Setup.Init();
            // Fluent.Architecture.Setup.Initialize(connectionString, true);
            // Architecture.Setup.SetCustomTypes(transactionObjectsType: typeof(TransactionObjectsTest));
        }
    }
}
