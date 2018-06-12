// <copyright file="InMemoryViewModelRepositoryTests.cs" company="Cognisant">
// Copyright (c) Cognisant. All rights reserved.
// </copyright>

namespace CR.ViewModels.Tests
{
    using NUnit.Framework;
    using Persistence.Memory;

    /// <inheritdoc />
    [TestFixture]
    internal sealed class InMemoryViewModelRepositoryTests : ViewModelRepositoryTestFixture
    {
        private InMemoryViewModelRepository _repo;

        /// <summary>
        /// Setup for the test fixture.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            _repo = new InMemoryViewModelRepository();
            Reader = _repo;
            Writer = _repo;
        }
    }
}
