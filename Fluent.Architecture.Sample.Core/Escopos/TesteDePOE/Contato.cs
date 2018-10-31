using Fluent.Architecture.Model;

namespace Fluent.Architecture.Sample.Core.Escopos
{
    public class UsuarioDeTesteContato : FluentEntity
    {
        [Hidden]
        public int Id { get; set; }

        public string Telefone { get; set; }

        public string Email { get; set; }
    }
}
