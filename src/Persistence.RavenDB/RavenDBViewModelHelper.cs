// <copyright file="RavenDBViewModelHelper.cs" company="Cognisant">
// Copyright (c) Cognisant. All rights reserved.
// </copyright>

namespace CR.ViewModels.Persistence.RavenDB
{
    /// <summary>
    /// Helper class used for code shared between <see cref="RavenDBViewModelReader"/> and <see cref="RavenDBViewModelWriter"/>.
    /// </summary>
    // ReSharper disable once InconsistentNaming
    internal static class RavenDBViewModelHelper
    {
        /// <summary>
        /// Helper method used to generate the ID of the RavenDB document that a view model of type TEntity with specified key should be stored.
        /// </summary>
        /// <typeparam name="TEntity">The type of the View Model.</typeparam>
        /// <param name="key">The key of the view model.</param>
        /// <returns>The ID of the RavenDB document.</returns>
        // ReSharper disable once InconsistentNaming
        internal static string MakeID<TEntity>(string key) => $"{typeof(TEntity).FullName}/{key}";
    }
}
