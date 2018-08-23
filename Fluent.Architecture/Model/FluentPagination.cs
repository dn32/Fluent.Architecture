
namespace Fluent.Architecture.Model
{
    /// <summary>
    /// Classe de solicitação de paginação padrão.
    /// </summary>
    public class FluentPagination
    {
        private const int ItensPerPageDefault = 10;
        private int _currentPage;
        private int _itemsPerPage;

        /// <summary>
        /// Essa propriedade é preenchida durante a requisição e retornada com o valor da quantidade total de itens referentes à solicitação.
        /// </summary>
        public int TotalQuantityOfItems { get; set; }

        /// <summary>
        /// A paginação padrão do sistema.
        /// </summary>
        public static FluentPagination DefaultPagination => new FluentPagination(ItensPerPageDefault, 0);

        /// <summary>
        /// A quantidade de itens por página.
        /// </summary>
        public int ItemsPerPage
        {
            get => _itemsPerPage == 0 ? ItensPerPageDefault : _itemsPerPage;
            set => _itemsPerPage = value;
        }

        /// <summary>
        /// A página atual.
        /// </summary>
        public int CurrentPage
        {
            get => _currentPage == 0 ? 1 : _currentPage;
            set => _currentPage = value;
        }

        /// <summary>
        /// Inicializa uma nova paginação.
        /// </summary>
        /// <param name="currentPage">
        /// A página atual, começando em 1.
        /// </param>
        /// <param name="itemsPerPage">
        /// A quantidade de itens por página. Quando não informado, é preencido com o valor padrão do sistema <see cref="ItensPerPageDefault"/>.
        /// </param>
        public FluentPagination(int currentPage, int? itemsPerPage)
        {
            CurrentPage = currentPage;
            _itemsPerPage = itemsPerPage ?? ItensPerPageDefault;
        }
    }
}
