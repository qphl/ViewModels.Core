// <copyright file="InMemoryViewModelRepositoryTests.cs" company="Corsham Science">
// Copyright (c) Corsham Science. All rights reserved.
// </copyright>

namespace CorshamScience.ViewModels.Core.Tests
{
    using CorshamScience.ViewModels.Core;
    using NUnit.Framework;

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
