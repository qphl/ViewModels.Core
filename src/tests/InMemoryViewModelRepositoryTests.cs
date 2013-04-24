using CR.ViewModels.Persistance.Memory;
using NUnit.Framework;

namespace CR.ViewModels.Tests
{
    [TestFixture]
    public class InMemoryViewModelRepositoryTests : ViewModelRepositoryTestFixture
    {
        private InMemoryViewModelRepository _repo;

        [SetUp]
        public void SetUp()
        {
            _repo = new InMemoryViewModelRepository();
            Reader = _repo;
            Writer = _repo;
        }

    }
}
