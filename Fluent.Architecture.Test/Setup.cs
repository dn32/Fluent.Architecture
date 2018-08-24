using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace Fluent.Architecture.Test
{
    public class Setup
    {
        internal static Dictionary<Type, IQueryable> LocalContext { get; set; }

        public static void Initialize()
        {
#if NET461
            var connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
#else
            var connectionString = "";
            throw new NotImplementedException();
#endif
            Initialize(connectionString);
        }

        public static void Initialize(string connectionString)
        {
            LocalContext = new Dictionary<Type, IQueryable>();
            Architecture.Setup.Initialize(connectionString);
            Architecture.Setup.SetCustomTypes(transactionObjectsType: typeof(TransactionObjectsTest));
        }
    }
}
