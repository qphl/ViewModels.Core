// <copyright file="ApplicationStateViewModelRepositoryTests.cs" company="Cognisant">
// Copyright (c) Cognisant. All rights reserved.
// </copyright>

namespace CR.ViewModels.Core.Tests
{
    using System;
    using System.Web;
    using ApplicationState;
    using NUnit.Framework;

    /// <inheritdoc />
    [TestFixture]
    internal sealed class ApplicationStateViewModelRepositoryTests : ViewModelRepositoryTestFixture
    {
        /// <summary>
        /// Setup for the test fixture.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            var state = new HttpApplicationStateWrapper((HttpApplicationState)Activator.CreateInstance(typeof(HttpApplicationState), true));
            Writer = new ApplicationStateViewModelWriter(state);
            Reader = new ApplicationStateViewModelReader(state);
        }
    }
}
