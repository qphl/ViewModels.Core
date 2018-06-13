// <copyright file="HttpApplicationStateBaseExtensions.cs" company="Cognisant">
// Copyright (c) Cognisant. All rights reserved.
// </copyright>

namespace CR.ViewModels.ApplicationState
{
    using System.Collections.Generic;
    using System.Web;

    /// <summary>
    /// Helper class used for extensions to HttpApplicationStateBase.
    /// Used by <see cref="ApplicationStateViewModelReader"/> and <see cref="ApplicationStateViewModelWriter"/>.
    /// </summary>
    internal static class HttpApplicationStateBaseExtensions
    {
        /// <summary>
        /// Extension method to get all view models stored in HttpApplicationStateBase for the specified type.
        /// </summary>
        /// <typeparam name="TEntity">The type of entities that should be retrieved.</typeparam>
        /// <param name="appState">The application state to get the view models from.</param>
        /// <returns>The view models stored in the ApplicationState as a dictionary of key => view model.</returns>
        internal static Dictionary<string, TEntity> GetEntities<TEntity>(this HttpApplicationStateBase appState)
        {
            var appStateKey = typeof(TEntity).FullName;
            return appState[appStateKey] != null
                ? (Dictionary<string, TEntity>)appState[appStateKey]
                : new Dictionary<string, TEntity>();
        }
    }
}
