// -----------------------------------------------------------------------
// <copyright company="Fluent System">
//     Copyright © Fluent System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------

using Fluent.Architecture.Controllers;

public class DepartamentoController : FluentAPIController<Departamento>
{
    public virtual DefaultPaginationResult ListByProximity(string term)
    {
        var spec = CreateSpec<DepartamentoNameProximity>().AddParameter(term);
        var list = Service.List(spec);
        return Result(list, LastRequestPagination);
    }
}



