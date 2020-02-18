using dn32.infra.Interfaces;
using System;
using System.Linq;
using System.Runtime.CompilerServices;
using dn32.infra.dados;

[assembly: InternalsVisibleTo(@"dn32.infra.EntityFramework, PublicKey=002400000480000094000000060200000024000052534131000400000100010001e5fbcd7e6f1d70524fc7b787a6ba4d8f332e822c5506e1831f4e59ab41e930c56bbf8cc29fa91f1270f4e873c036335c5aa4ccfc76ab13bfa7372de9d4e17de6c2d188fae9e6842d7d90d51e123836fd9f5d6be5580a32d1a12e59489519c6b93cdcf7ecd782042db1f31190350fbf937bbd6a5ae61d648773b46b9a706ccf")]
[assembly: InternalsVisibleTo(@"dn32.infra.EntityFramework.SqlServer, PublicKey=00240000048000009400000006020000002400005253413100040000010001002d98533364f3b3fbd11e7a3f14cd73d169e1daabd62ba2d1e5bc6a48a9bc709a503960db0e76c190e7a8dcefaed037e539682d6a891b242ddb91a3ab20fbfa0c04fb6304c8903857e1ed75399850fca4037dd2c810749e75770e5d455e950ccb9d06cf6fea5f30b00557a29408ce4c45021c412eca32616f47809bfe2cf404cc")]
[assembly: InternalsVisibleTo(@"dn32.infra.EntityFramework.PostgreSQL, PublicKey=0024000004800000940000000602000000240000525341310004000001000100192d4ee01ba583399ab1d381c4301592f8520d29c628f3220e1550b2068e540e26886fa8d8b52618553f89fed1dccb18d5d3c07c548fca3c916a10823f411c23ef0e85bf0526ed94aa3cfbdf79a9595861348cfc369670f8ed9f7c4afd08de5f3cd87a0c7c6b1d8a0b94622c163a764813ba95d39dc44ea1baf7b663800a49bc")]
[assembly: InternalsVisibleTo(@"dn32.infra.EntityFramework.MySQL, PublicKey=0024000004800000940000000602000000240000525341310004000001000100617593ae2b67e94c33ea38be9727f7a4a0e18fe316ea3cddceaaadd51d47546be3f27dc1d1c6c84d0a0cb43db45a7c476479c7ebd881d76b5dad404cafd086743036bd3c929dbf14c759ff504d798ca1097eb96b02dde75ee1bc120adc0e94553298c8749271502eb50cb427db851b1a26044bcb8e8fae1acf106069d2a349c0")]
namespace dn32.infra.Specifications
{
    /// <summary>
    /// Especificação base para todas as especificações do sistema.
    /// </summary>
    /// <typeparam Nome="TE">Tipo de entidade da especificação.</typeparam>
    public abstract partial class DnSpecification<TE> : BaseSpecification, IDnSpecification where TE : EntidadeBase
    {
        public Type DnEntityType => typeof(TE);

        /// <summary>
        /// A especificação.
        /// </summary>
        /// <param Nome="query">
        /// A referência à tabela/documento vinda do repositório.
        /// </param>
        /// <returns>
        /// A especificação criada.
        /// </returns>
        public abstract IQueryable<TE> Where(IQueryable<TE> query);

        // Todo2 doc
        public abstract IOrderedQueryable<TE> Order(IQueryable<TE> query);

        public IQueryable<TE> ToIQueryable(IQueryable<TE> query)
        {
            var w = Where(query);
            if (IgnoreOrder)
            {
                return w;
            }
            else
            {
                return Order(w);
            }
        }
    }
}