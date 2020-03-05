using dn32.infra.Interfaces;
using dn32.infra.Nucleo.Interfaces;
using dn32.infra.Nucleo.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Runtime.CompilerServices;
using dn32.infra.dados;
using dn32.infra.extensoes;

[assembly: InternalsVisibleTo(@"dn32.infra.EntityFramework.SqlServer, PublicKey=00240000048000009400000006020000002400005253413100040000010001002d98533364f3b3fbd11e7a3f14cd73d169e1daabd62ba2d1e5bc6a48a9bc709a503960db0e76c190e7a8dcefaed037e539682d6a891b242ddb91a3ab20fbfa0c04fb6304c8903857e1ed75399850fca4037dd2c810749e75770e5d455e950ccb9d06cf6fea5f30b00557a29408ce4c45021c412eca32616f47809bfe2cf404cc")]
[assembly: InternalsVisibleTo(@"dn32.infra.EntityFramework.PostgreSQL, PublicKey=0024000004800000940000000602000000240000525341310004000001000100192d4ee01ba583399ab1d381c4301592f8520d29c628f3220e1550b2068e540e26886fa8d8b52618553f89fed1dccb18d5d3c07c548fca3c916a10823f411c23ef0e85bf0526ed94aa3cfbdf79a9595861348cfc369670f8ed9f7c4afd08de5f3cd87a0c7c6b1d8a0b94622c163a764813ba95d39dc44ea1baf7b663800a49bc")]
[assembly: InternalsVisibleTo(@"dn32.infra.EntityFramework.MySQL, PublicKey=0024000004800000940000000602000000240000525341310004000001000100617593ae2b67e94c33ea38be9727f7a4a0e18fe316ea3cddceaaadd51d47546be3f27dc1d1c6c84d0a0cb43db45a7c476479c7ebd881d76b5dad404cafd086743036bd3c929dbf14c759ff504d798ca1097eb96b02dde75ee1bc120adc0e94553298c8749271502eb50cb427db851b1a26044bcb8e8fae1acf106069d2a349c0")]
namespace dn32.infra.EntityFramework
{
    /// <summary>
    /// Obtetos de transação.
    /// </summary>
    public class TransactionObjects : ITransactionObjects
    {
        /// <summary>
        /// Sessão do EF.
        /// </summary>
        public DbContext Session { get; set; }

        public SessaoDeRequisicaoDoUsuario UserSessionRequest { get; set; }

        public void Dispose()
        {
            this.Session.Dispose();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionObjects"/> class. 
        /// Inicializa objeto de transação.
        /// </summary>
        /// <param Nome="dataBaseConnectionString">
        /// String de conexão com o banco de dados.
        /// </param>
        public TransactionObjects(Connection connection, SessaoDeRequisicaoDoUsuario userSessionRequest)
        {
            this.UserSessionRequest = userSessionRequest;
            this.Session = ContextFactory.Create(connection, UserSessionRequest);
        }

        public DbSet<TX> GetObjectInputDataInternal<TX>() where TX : EntidadeBase
        {
            return this.Session.Set<TX>();
        }

        public IQueryable GetObjectInputDataInternal(Type type)
        {
            return typeof(DbContext).GetMethod(nameof(DbContext.Set))?.MakeGenericMethod(type).Invoke(Session, null).DnCast<IQueryable>();
        }

        /// <summary>
        /// Obtem o referência de uma tabela do banco de dados.
        /// </summary>
        /// <typeparam Nome="TX">
        /// Tipo de entidade desejada.
        /// </typeparam>
        /// <returns>
        /// A referência da tabela do banco de dados.
        /// </returns>
        public virtual IQueryable<TX> GetObjectQueryInternal<TX>() where TX : EntidadeBase
        {
            return this.Session.Set<TX>();
        }
    }
}