using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using persistance.memory;

namespace cr.viewmodels.tests
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
