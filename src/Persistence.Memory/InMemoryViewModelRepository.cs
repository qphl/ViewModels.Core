// <copyright file="InMemoryViewModelRepository.cs" company="Cognisant">
// Copyright (c) Cognisant. All rights reserved.
// </copyright>

namespace CR.ViewModels.Persistence.Memory
{
    using System;
    using System.Collections.Concurrent;
    using System.Linq;
    using System.Linq.Expressions;
    using Core;
    using Core.Exceptions;

    /// <inheritdoc cref="IViewModelReader"/>
    /// <inheritdoc cref="IViewModelWriter"/>
    /// <summary>
    /// A view model repository that uses an internal ConcurrentDictionary to both store and retrieve view models.
    /// Implements both <see cref="T:CR.ViewModels.Core.IViewModelReader" /> and <see cref="T:CR.ViewModels.Core.IViewModelWriter" />.
    /// </summary>
    [Serializable]
    public class InMemoryViewModelRepository : IViewModelReader, IViewModelWriter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InMemoryViewModelRepository"/> class which reads view models from, and writes them to, a .Net ConcurrentDictionary stored in memory.
        /// </summary>
        public InMemoryViewModelRepository() => EntityCollections = new ConcurrentDictionary<Type, object>();

        private ConcurrentDictionary<Type, object> EntityCollections { get; }

        /// <inheritdoc />
        public TEntity GetByKey<TEntity>(string key)
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

            var entities = GetEntities<TEntity>();
            return entities.TryGetValue(key, out var result) ? result : null;
        }

        /// <inheritdoc />
        public IQueryable<TEntity> Query<TEntity>()
            where TEntity : class
        {
            var entities = GetEntities<TEntity>();
            return entities.Values.AsQueryable();
        }

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

            var entities = (ConcurrentDictionary<string, TEntity>)EntityCollections.GetOrAdd(typeof(TEntity), new ConcurrentDictionary<string, TEntity>());
            if (!entities.TryAdd(key, entity))
            {
                throw new DuplicateKeyException("An entity with this key has already been added.", key);
            }
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
                throw new ArgumentNullException(nameof(update), "The Update Action cannot be null.");
            }

            var entities = GetEntities<TEntity>();
            if (entities.TryGetValue(key, out var entity))
            {
                update(entity);
            }
            else
            {
                throw new EntityNotFoundException("The provided key for the View Model to update was not found in the View Model storage.", key);
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

            var entities = GetEntities<TEntity>();
            var toUpdate = entities.Values.Where(predicate.Compile()).ToList();
            foreach (var ent in toUpdate)
            {
                update(ent);
            }
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

            var entities = GetEntities<TEntity>();
            if (!entities.TryRemove(key, out _))
            {
                throw new EntityNotFoundException(key);
            }
        }

        /// <inheritdoc />
        public void DeleteWhere<TEntity>(Expression<Func<TEntity, bool>> predicate)
            where TEntity : class
        {
            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate), "The Deletion Predicate cannot be null.");
            }

            var entities = GetEntities<TEntity>();
            var toDelete = entities.Where(e => predicate.Compile()(e.Value)).ToList();
            foreach (var ent in toDelete)
            {
                entities.TryRemove(ent.Key, out _);
            }
        }

        private ConcurrentDictionary<string, TEntity> GetEntities<TEntity>()
        {
            if (EntityCollections.TryGetValue(typeof(TEntity), out var typeDict))
            {
                return (ConcurrentDictionary<string, TEntity>)typeDict;
            }

            return new ConcurrentDictionary<string, TEntity>();
        }
    }
}
