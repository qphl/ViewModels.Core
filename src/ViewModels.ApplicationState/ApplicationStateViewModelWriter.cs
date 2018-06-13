// <copyright file="ApplicationStateViewModelWriter.cs" company="Cognisant">
// Copyright (c) Cognisant. All rights reserved.
// </copyright>

namespace CR.ViewModels.ApplicationState
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using System.Web;
    using Core;
    using Core.Exceptions;

    /// <inheritdoc />
    /// <summary>
    /// A implementation of the <see cref="IViewModelReader" /> interface that uses a <see cref="HttpApplicationState"/> from System.Web to store view models.
    /// </summary>
    public class ApplicationStateViewModelWriter : IViewModelWriter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationStateViewModelWriter"/> class, which uses the provided <see cref="HttpApplicationState"/> to store view models.
        /// </summary>
        /// <param name="appState">The application state where the view models should be stored.</param>
        public ApplicationStateViewModelWriter(HttpApplicationStateBase appState) => AppState = appState;

        private HttpApplicationStateBase AppState { get; }

        /// <inheritdoc />
        public void Add<TEntity>(string key, TEntity entity)
            where TEntity : class
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key), "Key must not be null.");
            }

            if (key == string.Empty)
            {
                throw new ArgumentException("Key must not be an empty string.", nameof(key));
            }

            if (AppState[typeof(TEntity).FullName] == null)
            {
                AppState[typeof(TEntity).FullName] = new Dictionary<string, TEntity>();
            }

            var entities = AppState.GetEntities<TEntity>();
            if (entities.ContainsKey(key))
            {
                throw new DuplicateKeyException("An entity with this key has already been added", key);
            }

            entities.Add(key, entity);
        }

        /// <inheritdoc />
        public void Update<TEntity>(string key, Action<TEntity> update)
            where TEntity : class
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key), "Key must not be null.");
            }

            if (key == string.Empty)
            {
                throw new ArgumentException("Key must not be an empty string.", nameof(key));
            }

            if (update == null)
            {
                throw new ArgumentNullException(nameof(update), "The update action must not be null.");
            }

            var entities = AppState.GetEntities<TEntity>();
            if (entities.TryGetValue(key, out var entity))
            {
                update(entity);
            }
            else
            {
                throw new EntityNotFoundException("The key provided for updating a view model was not found in the view model storage.", key);
            }
        }

        /// <inheritdoc />
        public void UpdateWhere<TEntity>(Expression<Func<TEntity, bool>> predicate, Action<TEntity> update)
            where TEntity : class
        {
            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate), "The Update Predicate cannot be null.");
            }

            if (update == null)
            {
                throw new ArgumentNullException(nameof(update), "The Update Action cannot be null.");
            }

            var entities = AppState.GetEntities<TEntity>();
            var targets = entities.Values.Where(predicate.Compile()).ToList();
            Parallel.ForEach(targets, update);
        }

        /// <inheritdoc />
        public void Delete<TEntity>(string key)
            where TEntity : class
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key), "Key must not be null.");
            }

            if (key == string.Empty)
            {
                throw new ArgumentException("Key must not be an empty string.", nameof(key));
            }

            var entities = AppState.GetEntities<TEntity>();
            if (!entities.ContainsKey(key))
            {
                throw new EntityNotFoundException("The key provided to delete a view model was not found in the view model storage.", key);
            }

            entities.Remove(key);
        }

        /// <inheritdoc />
        public void DeleteWhere<TEntity>(Expression<Func<TEntity, bool>> predicate)
            where TEntity : class
        {
            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate), "The deletion predicate cannot be null.");
            }

            var entities = AppState.GetEntities<TEntity>();
            var toDelete = entities.Where(e => predicate.Compile()(e.Value)).ToList();
            foreach (var entity in toDelete)
            {
                entities.Remove(entity.Key);
            }
        }
    }
}
