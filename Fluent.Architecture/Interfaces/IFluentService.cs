//using Fluent.Architecture.Interfaces;
//using Fluent.Architecture.Model;
//using System.Collections.Generic;

//namespace FluentInterfaces
//{
//    public interface IFluentService
//    {
//        List<TO> ListSelect<TO>(IFluentSpecification<TO> spec, FluentPagination pagination = null);

//        /// <summary>
//        /// Executa uma solicitação baseada em uma especificação e retorna uma lista paginada de resultados.
//        /// </summary>
//        /// <param name="spec">
//        /// A especificação de requisição.
//        /// </param>
//        /// <param name="pagination">
//        /// A paginação desejada.
//        /// </param>
//        /// <returns>
//        /// A lista paginada de resultados.
//        /// </returns>
//        //List<T> List(IFluentSpecification spec, FluentPagination pagination = null);

//        /// <summary>
//        /// Executa uma solicitação baseada em uma especificação e retorna um resultado ou nulo quando a consulta não é satisfeita.
//        /// </summary>
//        /// <typeparam name="TO">
//        /// O tipo de saida desejada. Deve ser o mesmo definido na saida da especificação.
//        /// </typeparam>
//        /// <param name="spec">
//        /// A especificação de requisição.
//        /// </param>
//        /// <returns>
//        /// O item referente à consulta ou nulo.
//        /// </returns>
//        TO FirstOrDefaultSelect<TO>(IFluentSpecification<TO> spec);

//        /// <summary>
//        /// Executa uma solicitação baseada em uma especificação e retorna um resultado ou nulo quando a consulta não é satisfeita.
//        /// </summary>
//        /// <param name="spec">
//        /// A especificação de requisição.
//        /// </param>
//        /// <returns>
//        /// O item referente à consulta ou nulo.
//        /// </returns>
//        //T FirstOrDefault(IFluentSpecification spec);

//        //Todo Doc
//        //T FirstOrDefault();

//        /// <summary>
//        /// Retorna a quantidade de itens existentes que satisfaçam a uma especificação
//        /// </summary>
//        /// <typeparam name="TO">
//        /// O tipo de saida desejada. Deve ser o mesmo definido na saida da especificação.
//        /// </typeparam>
//        /// <param name="spec">
//        /// A especificação de requisição.
//        /// </param>
//        /// <returns>
//        /// A quantidade de itens.
//        /// </returns>
//        int CountSelect<TO>(IFluentSpecification<TO> spec);

//        /// <summary>
//        /// Retorna a quantidade de itens existentes que satisfaçam a uma especificação
//        /// </summary>
//        /// <param name="spec">
//        /// A especificação de requisição.
//        /// </param>
//        /// <returns>
//        /// A quantidade de itens.
//        /// </returns>
//        int Count(IFluentSpecification spec);

//        // Todo documentar
//        int Count();

//        // Todo documentar
//        void RemoveRange(IFluentSpecification spec);

//        /// <summary>
//        /// Avalia se um item existe no banco de dados, baseado em uma especificação.
//        /// </summary>
//        /// <param name="spec">
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
//        //// <param name = "entities" >
//        //// Itens a serem adicionados.
//        //// </param>
//        //void AddRange(params T[] entities);

//        //// <summary>
//        //// Adiciona um item ao banco de dados.
//        //// </summary>
//        //// <param name = "entity" >
//        //// Item a ser adicionado.
//        //// </param>
//        //T Add(T entity);

//        ////Todo documentar
//        //T Find(T entity);

//        //// <summary>
//        //// Atualiza um item do banco de dados baseado em seu identificador.
//        //// </summary>
//        //// <param name = "entity" >
//        //// Entidade a ser atualizada com o identificador preenchido.
//        //// </param>
//        //T Update(T entity);

//        //// <summary>
//        //// Remove um item do banco de dados baseado em seu identificador.
//        //// </summary>
//        //// <param name = "entity" >
//        //// Entidade a ser removida.
//        //// </param>
//        //T Remove(T entity);

//        ////Todo documentar
//        //void RemoveRange(params T[] entities);
//    }
//}
