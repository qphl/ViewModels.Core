using System.IO;
using CR.ViewModels.Core;
using CR.ViewModels.Persistance.RavenDB;
using NUnit.Framework;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Embedded;

namespace CR.ViewModels.Tests
{
    public class RavenDBViewModelRepositoryTests : ViewModelRepositoryTestFixture
    {
        private static int DataDirectories = 0;
        private IDocumentStore _docStore;

        [SetUp]
        public void SetUp()
        {
            //_docStore = new EmbeddableDocumentStore() {DataDirectory = "data" + ++DataDirectories};
            _docStore = new EmbeddableDocumentStore() {RunInMemory = true};
            _docStore.Initialize();
            Reader = new RavenDBViewModelReader(_docStore);
            Writer = new RavenDBViewModelWriter(_docStore);
        }

        /*[TearDown]
        public void TearDown()
        {
            _docStore.Dispose();
            var dir = new DirectoryInfo("data");
            dir.Delete(true);
        }*/

    }
}
