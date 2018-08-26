using Fluent.Architecture.Repository;

namespace Fluent.Architecture.Test.SupportElements.Mock
{
    /// <inheritdoc />
    /// <summary>
    /// Fábrica de Mock de repositório para testes com simulação de repositório.
    /// </summary>
    /// <typeparam name="TR">
    /// O tipo de repositório a ser "mockado".
    /// </typeparam>
    public class RepositoryMockFactory<TR> : MockFactory<TR> where TR : BaseRepository
    {
        /// <inheritdoc />
        public override void Return(object @return)
        {
           // Architecture.Setup.SetRepositoryMock();
            base.Return(@return);
        }
    }
}