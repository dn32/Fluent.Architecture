// -----------------------------------------------------------------------
// <copyright company="Fluent System">
//     Copyright © Fluent System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------

// ReSharper disable CommentTypo

using System.ComponentModel;

namespace Fluent.Architecture.Core.Models
{
    /// <summary>
    /// Classe de solicitação de paginação padrão.
    /// </summary>
    [Attributes.FluentDoc]
    public class FluentPagination
    {
        private const int ItemsPerPageDefault = 10;
        private int _currentPage;
        private readonly int _itemsPerPage;
        private readonly bool _startAtZero;

        /// <summary>
        /// Essa propriedade é preenchida durante a requisição e retornada com o valor da quantidade total de itens referentes à solicitação.
        /// </summary>
        [Description("This property is filled during the request and returns the value of the total quantity of items")]
        public virtual int TotalQuantityOfItems { get; set; }

        //Todo2 - doc
        [Description("If the first page is 0")]
        public virtual bool StartAtZero => _startAtZero;

        //Todo2 - doc
        [Description("How many items are being \"skipped\" to get to the current page")]
        public virtual int Skip => ItemsPerPage * (StartAtZero ? CurrentPage : CurrentPage - 1);

        /// <summary>
        /// A quantidade de itens por página.
        /// </summary>
        [Description("The number of items per page")]
        public virtual int ItemsPerPage => _itemsPerPage == 0 ? ItemsPerPageDefault : this._itemsPerPage;

        /// <summary>
        /// A página atual.
        /// </summary>
        [Description("The current page")]
        public virtual int CurrentPage
        {
            get => !_startAtZero && _currentPage == 0 ? 1 : _currentPage;
            set => _currentPage = value;
        }

        /// <summary>
        /// Quantidade de páginas.
        /// Todo - Testar
        /// </summary>
        [Description("Total number of pages")]
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
