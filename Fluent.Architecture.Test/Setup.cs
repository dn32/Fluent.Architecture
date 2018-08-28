#if NET461
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Fluent.Architecture.Test.TestTools;

namespace Fluent.Architecture.Test
{
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