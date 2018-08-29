// ReSharper disable CommentTypo

namespace Fluent.Architecture.Model
{
    /// <summary>
    /// Classe de solicitação de paginação padrão.
    /// </summary>
    public class FluentPagination
    {
        private const int ItemsPerPageDefault = 10;
        private int currentPage;
        private readonly int itemsPerPage;

        /// <summary>
        /// Essa propriedade é preenchida durante a requisição e retornada com o valor da quantidade total de itens referentes à solicitação.
        /// </summary>
        public int TotalQuantityOfItems { get; set; }

        /// <summary>
        /// A quantidade de itens por página.
        /// </summary>
        public int ItemsPerPage => this.itemsPerPage == 0 ? ItemsPerPageDefault : this.itemsPerPage;

        /// <summary>
        /// A página atual.
        /// </summary>
        public int CurrentPage
        {
            get => this.currentPage == 0 ? 1 : this.currentPage;
            set => this.currentPage = value;
        }

        /// <summary>
        /// Inicializa uma nova paginação.
        /// </summary>
        /// <param name="currentPage">
        /// A página atual, começando em 1.
        /// </param>
        /// <param name="itemsPerPage">
        /// A quantidade de itens por página. Quando não informado, é preencido com o valor padrão do sistema <see cref="ItemsPerPageDefault"/>.
        /// </param>
        public FluentPagination(int currentPage, int? itemsPerPage)
        {
            this.CurrentPage = currentPage;
            this.itemsPerPage = itemsPerPage ?? ItemsPerPageDefault;
        }
    }
}
