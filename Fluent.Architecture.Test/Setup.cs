using System;
using System.Collections.Generic;
using System.Linq;

namespace Fluent.Architecture.Test
{
    public class Setup
    {
        internal static Dictionary<Type, IQueryable> LocalContext { get; set; }

        public static void Initialize( string connectionString)
        {
            LocalContext = new Dictionary<Type, IQueryable>();
            Architecture.Setup.Initialize(connectionString);
            Architecture.Setup.SetCustomTypes(transactionObjectsType: typeof(TransactionObjectsTest));
        }
    }
}
