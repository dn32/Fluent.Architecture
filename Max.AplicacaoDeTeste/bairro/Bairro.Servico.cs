using Max.Infraestrutura.ClassesBase;

namespace Max.AplicacaoDeTeste.bairro
{
    public class BairroService : MaxServico<Bairro>
    {
        public override Bairro Add(Bairro entity)
        {
            return base.Add(entity);
        }
    }
}
