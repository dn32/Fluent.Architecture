// ReSharper disable CommentTypo

namespace Fluent.Architecture.Model
{
    /// <summary>
    /// Classe de solicitação de paginação padrão.
    /// </summary>
    public class FluentPagination
    {
        private const int ItemsPerPageDefault = 10;
        private int _currentPage;
        private readonly int _itemsPerPage;
        private readonly bool _startAtZero;

        /// <summary>
        /// Essa propriedade é preenchida durante a requisição e retornada com o valor da quantidade total de itens referentes à solicitação.
        /// </summary>
        public virtual int TotalQuantityOfItems { get; set; }

        //Todo - doc
        public virtual bool StartAtZero => _startAtZero;

        //Todo - doc
        public virtual int Skip => ItemsPerPage * (StartAtZero ? CurrentPage : CurrentPage - 1);

        /// <summary>
        /// A quantidade de itens por página.
        /// </summary>
        public virtual int ItemsPerPage => _itemsPerPage == 0 ? ItemsPerPageDefault : this._itemsPerPage;

        /// <summary>
        /// A página atual.
        /// </summary>
        public virtual int CurrentPage
        {
            get => !_startAtZero && _currentPage == 0 ? 1 : _currentPage;
            set => _currentPage = value;
        }

        /// <summary>
        /// Quantidade de páginas.
        /// Todo - Testar
        /// </summary>
        public virtual int NumberOfPages
        {
            get
            {
                var number = TotalQuantityOfItems / ItemsPerPage;
                if (TotalQuantityOfItems % ItemsPerPage > 0)
                {
                    number++;
                }

                return number;
            }
        }

        /// <summary>
        /// Inicializa uma nova paginação.
        /// </summary>
        /// <param name="currentPage">
        /// A página atual, começando em 1.
        /// </param>
        /// <param name="startAtZero">
        /// Se a paginação deve iniciar em 0.
        /// </param>
        /// <param name="itemsPerPage">
        /// A quantidade de itens por página. Quando não informado, é preencido com o valor padrão do sistema <see cref="ItemsPerPageDefault"/>.
        /// </param>
        public FluentPagination(int currentPage, bool startAtZero = true, int itemsPerPage = ItemsPerPageDefault)
        {
            _currentPage = !startAtZero && currentPage == 0 ? 1 : currentPage;
            _itemsPerPage = itemsPerPage;
            _startAtZero = startAtZero;
        }
    }
}
