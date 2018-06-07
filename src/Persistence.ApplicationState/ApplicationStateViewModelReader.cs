// <copyright file="ApplicationStateViewModelReader.cs" company="Cognisant">
// Copyright (c) Cognisant. All rights reserved.
// </copyright>

namespace CR.ViewModels.Persistence.ApplicationState
{
    using System;
    using System.Linq;
    using System.Web;
    using Core;

    /// <inheritdoc />
    /// <summary>
    /// A implementation of the <see cref="T:CR.ViewModels.Core.IViewModelReader" /> interface that uses
    /// HttpApplicationState from System.Web to retrieve view models.
    /// </summary>
    public class ApplicationStateViewModelReader : IViewModelReader
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationStateViewModelReader"/> class.
        /// </summary>
        /// <param name="appState">The application state where the view models should be retrieved from.</param>
        public ApplicationStateViewModelReader(HttpApplicationStateBase appState)
        {
            AppState = appState;
        }

        private HttpApplicationStateBase AppState { get; }

        /// <inheritdoc />
        public TEntity GetByKey<TEntity>(string key)
            where TEntity : class
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (key == string.Empty)
            {
                throw new ArgumentException("key must not be an empty string", nameof(key));
            }

            var entities = AppState.GetEntities<TEntity>();

            return entities.TryGetValue(key, out var result) ? result : null;
        }

        /// <inheritdoc />
        public IQueryable<TEntity> Query<TEntity>()
            where TEntity : class
        {
            var entities = AppState.GetEntities<TEntity>();
            return entities.Values.AsQueryable();
        }
    }
}
