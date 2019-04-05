using Fluent.Architecture.Attributes;
using Fluent.Architecture.Model;
using System.ComponentModel.DataAnnotations;

namespace Fluent.Architecture.Sample.Core.Escopos
{
    public class UsuarioDeTeste : FluentEntity
    {
        public int Id { get; set; }

        [MaxLength(100), Required]
        public string Nome { get; set; }

        [FluentUniqueKey, MaxLength(100)]
        public string Email { get; set; }

        [Required, Label("Grupo de usuário")]
        public GrupoDeUsuario GrupoDeUsuario { get; set; }

        [Required, Filtro(typeof(EmpresasPorVisibilidadeSpec)), Hidden]
        public Empresa Empresa { get; set; }

        [Composite, Advanced]
        public UsuarioDeTesteContato Contato { get; set; }

        [Advanced]
        public string Matricula { get; set; }
    }

    public class GrupoDeUsuario : FluentEntity2
    {
        public int Id { get; set; }

        public string Nome { get; set; }

        public override string IdentificadorAmigavel()
        {
            return $"{Id} - {Nome}";
        }
    }

    public class Empresa : FluentEntity2
    {
        public int Id { get; set; }

        public string Nome { get; set; }

        public override string IdentificadorAmigavel()
        {
            return $"{Id} - {Nome}";
        }
    }

}
