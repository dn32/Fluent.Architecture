
using System.Runtime.InteropServices;

namespace Fluent.Architecture.Test
{

    [ComVisible(true)]
    public static class Setup
    {
        public static void Initialize(string connectionString)
        {
            Architecture.Setup.Initialize(connectionString, true);
            // Architecture.Setup.SetCustomTypes(transactionObjectsType: typeof(TransactionObjectsTest));
        }
    }
}

