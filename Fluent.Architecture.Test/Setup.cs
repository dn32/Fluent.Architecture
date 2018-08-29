#if NET461
using System.Configuration;
using System.Runtime.InteropServices;

namespace Fluent.Architecture.Test
{

    [ComVisible(true)]
    public class Setup
    {
        public static void Initialize()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            Architecture.Setup.Initialize(connectionString, true);
            //Architecture.Setup.SetCustomTypes(transactionObjectsType: typeof(TransactionObjectsTest));
        }
    }
}
#endif