using Fluent.Architecture.Core.Attributes;

namespace Max.Infraestrutura.config.Logs
{
    [FluentUseEnumValueToDB]
    public enum EnumLogDeOperacaoTipoDeOperacao
    {
        Deleted = 2,
        Modified = 3,
        Added = 4
    }
}
