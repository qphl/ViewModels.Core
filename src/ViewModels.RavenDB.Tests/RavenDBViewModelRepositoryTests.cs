// <copyright file="RavenDBViewModelRepositoryTests.cs" company="Cognisant">
// Copyright (c) Cognisant. All rights reserved.
// </copyright>

namespace CR.ViewModels.Core.Tests
{
    using NUnit.Framework;
    using Raven.Client;
    using Raven.Client.Document;
    using Raven.Client.Embedded;
    using RavenDB;

    /// <inheritdoc />
    [TestFixture]
    internal sealed class RavenDbViewModelRepositoryTests : ViewModelRepositoryTestFixture
    {
        private IDocumentStore _docStore;

        /// <summary>
        /// Setup for the test fixture.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
#pragma warning disable CS0618 // Type or member is obsolete
            _docStore = new EmbeddableDocumentStore { RunInMemory = true, Conventions = new DocumentConvention { DefaultQueryingConsistency = ConsistencyOptions.AlwaysWaitForNonStaleResultsAsOfLastWrite } };
#pragma warning restore CS0618 // Type or member is obsolete
            _docStore.Initialize();

            Reader = new RavenDBViewModelReader(_docStore);
            Writer = new RavenDBViewModelWriter(_docStore);
        }

        /// <summary>
        /// Teardown for the test fixture.
        /// </summary>
        [TearDown]
        public void TearDown() => _docStore.Dispose();
    }
}
