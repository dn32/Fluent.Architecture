using Fluent.Architecture.Core.Attributes;

namespace Max.Infraestrutura.config.Logs
{
    [FluentUseEnumValueToDB]
    public enum EnumOrigemLogDeOperacao
    {
        API = 0,
        FormularioDinamico = 1,
    }
}
