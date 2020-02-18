//using dn32.infra.Interfaces;
//using dn32.infra.Model;
//using System.Collections.Generic;

//namespace DnInterfaces
//{
//    public interface IDnService
//    {
//        Listar<TO> ListSelect<TO>(IDnSpecification<TO> spec, DnPagination pagination = null);

//        /// <summary>
//        /// Executa uma solicitação baseada em uma especificação e retorna uma lista paginada de resultados.
//        /// </summary>
//        /// <param Nome="spec">
//        /// A especificação de requisição.
//        /// </param>
//        /// <param Nome="pagination">
//        /// A paginação desejada.
//        /// </param>
//        /// <returns>
//        /// A lista paginada de resultados.
//        /// </returns>
//        //Listar<T> Listar(IDnSpecification spec, DnPagination pagination = null);

//        /// <summary>
//        /// Executa uma solicitação baseada em uma especificação e retorna um resultado ou nulo quando a consulta não é satisfeita.
//        /// </summary>
//        /// <typeparam Nome="TO">
//        /// O tipo de saida desejada. Deve ser o mesmo definido na saida da especificação.
//        /// </typeparam>
//        /// <param Nome="spec">
//        /// A especificação de requisição.
//        /// </param>
//        /// <returns>
//        /// O item referente à consulta ou nulo.
//        /// </returns>
//        TO FirstOrDefaultSelect<TO>(IDnSpecification<TO> spec);

//        /// <summary>
//        /// Executa uma solicitação baseada em uma especificação e retorna um resultado ou nulo quando a consulta não é satisfeita.
//        /// </summary>
//        /// <param Nome="spec">
//        /// A especificação de requisição.
//        /// </param>
//        /// <returns>
//        /// O item referente à consulta ou nulo.
//        /// </returns>
//        //T FirstOrDefault(IDnSpecification spec);

//        //Todo2 Doc
//        //T FirstOrDefault();

//        /// <summary>
//        /// Retorna a quantidade de itens existentes que satisfaçam a uma especificação
//        /// </summary>
//        /// <typeparam Nome="TO">
//        /// O tipo de saida desejada. Deve ser o mesmo definido na saida da especificação.
//        /// </typeparam>
//        /// <param Nome="spec">
//        /// A especificação de requisição.
//        /// </param>
//        /// <returns>
//        /// A quantidade de itens.
//        /// </returns>
//        int CountSelect<TO>(IDnSpecification<TO> spec);

//        /// <summary>
//        /// Retorna a quantidade de itens existentes que satisfaçam a uma especificação
//        /// </summary>
//        /// <param Nome="spec">
//        /// A especificação de requisição.
//        /// </param>
//        /// <returns>
//        /// A quantidade de itens.
//        /// </returns>
//        int Count(IDnSpecification spec);

//        // Todo2 documentar
//        int Count();

//        // Todo2 documentar
//        void RemoverLista(IDnSpecification spec);

//        /// <summary>
//        /// Avalia se um item existe no banco de dados, baseado em uma especificação.
//        /// </summary>
//        /// <param Nome="spec">
//        /// A especificação de requisição.
//        /// </param>
//        /// <returns>
//        /// Se o item existe ou não.
//        /// </returns>
//        bool Exists(ISpec spec);

//        bool ExistsSelect<TO>(ISpec spec);

//        //// <summary>
//        //// Adiciona vários itens de um mesmo tipo ao banco de dados.
//        //// </summary>
//        //// <param Nome = "entities" >
//        //// Itens a serem adicionados.
//        //// </param>
//        //void AdicionarLista(params T[] entities);

//        //// <summary>
//        //// Adiciona um item ao banco de dados.
//        //// </summary>
//        //// <param Nome = "entity" >
//        //// Item a ser adicionado.
//        //// </param>
//        //T Adicionar(T entity);

//        ////Todo2 documentar
//        //T Find(T entity);

//        //// <summary>
//        //// Atualiza um item do banco de dados baseado em seu identificador.
//        //// </summary>
//        //// <param Nome = "entity" >
//        //// Entidade a ser atualizada com o identificador preenchido.
//        //// </param>
//        //T Atualizar(T entity);

//        //// <summary>
//        //// Remover um item do banco de dados baseado em seu identificador.
//        //// </summary>
//        //// <param Nome = "entity" >
//        //// Entidade a ser removida.
//        //// </param>
//        //T Remover(T entity);

//        ////Todo2 documentar
//        //void RemoverLista(params T[] entities);
//    }
//}
