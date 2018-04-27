using System;
using System.Web;
using CR.ViewModels.Persistence.ApplicationState;
using NUnit.Framework;

namespace CR.ViewModels.Tests
{
    [TestFixture]
    public class ApplicationStateViewModelRepositoryTests : ViewModelRepositoryTestFixture
    {
        [SetUp]
        public void SetUp()
        {
            var state = new HttpApplicationStateWrapper((HttpApplicationState)Activator.CreateInstance(typeof(HttpApplicationState), true));
            Writer = new ApplicationStateViewModelWriter(state);
            Reader = new ApplicationStateViewModelReader(state);
        }
    }
}