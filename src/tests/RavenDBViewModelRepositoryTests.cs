using CR.ViewModels.Persistence.RavenDB;
using NUnit.Framework;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Embedded;

namespace CR.ViewModels.Tests
{
    [TestFixture]
    public class RavenDBViewModelRepositoryTests : ViewModelRepositoryTestFixture
    {
        private IDocumentStore _docStore;

        [SetUp]
        public void SetUp()
        {
            _docStore = new EmbeddableDocumentStore() {RunInMemory = true, Conventions = new DocumentConvention() {DefaultQueryingConsistency = ConsistencyOptions.QueryYourWrites}};
            _docStore.Initialize();
            Reader = new RavenDBViewModelReader(_docStore);
            Writer = new RavenDBViewModelWriter(_docStore);
        }

        [TearDown]
        public void TearDown()
        {
            _docStore.Dispose();
        }
    }
}
