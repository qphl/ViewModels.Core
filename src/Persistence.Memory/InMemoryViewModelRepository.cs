// <copyright file="InMemoryViewModelRepository.cs" company="Cognisant">
// Copyright (c) Cognisant. All rights reserved.
// </copyright>

namespace CR.ViewModels.Persistence.Memory
{
    using System;
    using System.Collections.Concurrent;
    using System.Linq;
    using System.Linq.Expressions;
    using CR.ViewModels.Core;
    using CR.ViewModels.Core.Exceptions;

    /// <summary>
    /// In Memory ViewModel Repository
    /// Stores ViewModels to an internal ConcurrentDictionary
    /// </summary>
    [Serializable]
    public class InMemoryViewModelRepository : IViewModelReader, IViewModelWriter
    {
        public InMemoryViewModelRepository()
        {
            EntityCollections = new ConcurrentDictionary<Type, object>();
        }

        public ConcurrentDictionary<Type, object> EntityCollections { get; set; }

        public TEntity GetByKey<TEntity>(string key)
            where TEntity : class
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }

            if (key == string.Empty)
            {
                throw new ArgumentException("key must not be an empty string", "key");
            }

            var entities = GetEntities<TEntity>();

            TEntity result;
            return entities.TryGetValue(key, out result) ? result : null;
        }

        public IQueryable<TEntity> Query<TEntity>()
            where TEntity : class
        {
            var entities = GetEntities<TEntity>();
            return entities.Values.AsQueryable();
        }

        public void Add<TEntity>(string key, TEntity entity)
            where TEntity : class
        {
            var entities = (ConcurrentDictionary<string, TEntity>)EntityCollections.GetOrAdd(typeof(TEntity), new ConcurrentDictionary<string, TEntity>());

            if (!entities.TryAdd(key, entity))
            {
                throw new DuplicateKeyException("An entity with this key has alreaady been added");
            }
        }

        public void Update<TEntity>(string key, Action<TEntity> update)
            where TEntity : class
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }

            if (key == string.Empty)
            {
                throw new ArgumentException("key must not be an empty string", "key");
            }

            if (update == null)
            {
                throw new ArgumentNullException("update");
            }

            var entities = GetEntities<TEntity>();

            TEntity entity;
            if (entities.TryGetValue(key, out entity))
            {
                update(entity);
            }
            else
            {
                throw new EntityNotFoundException();
            }
        }

        public void UpdateWhere<TEntity>(Expression<Func<TEntity, bool>> predicate, Action<TEntity> update)
            where TEntity : class
        {
            if (predicate == null)
            {
                throw new ArgumentNullException("predicate");
            }

            if (update == null)
            {
                throw new ArgumentNullException("update");
            }

            var entities = GetEntities<TEntity>();
            var toUpdate = entities.Values.Where(predicate.Compile()).ToList();

            foreach (var ent in toUpdate)
            {
                update(ent);
            }
        }

        public void Delete<TEntity>(string key)
            where TEntity : class
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }

            if (key == string.Empty)
            {
                throw new ArgumentException("key must not be an empty string", "key");
            }

            var entities = GetEntities<TEntity>();
            TEntity entity;
            if (!entities.TryRemove(key, out entity))
            {
                throw new EntityNotFoundException();
            }
        }

        public void DeleteWhere<TEntity>(Expression<Func<TEntity, bool>> predicate)
            where TEntity : class
        {
            if (predicate == null)
            {
                throw new ArgumentNullException("predicate");
            }

            var entities = GetEntities<TEntity>();
            var toDelete = entities.Where(e => predicate.Compile()(e.Value)).ToList();

            foreach (var ent in toDelete)
            {
                TEntity entity;
                entities.TryRemove(ent.Key, out entity);
            }
        }

        private ConcurrentDictionary<string, TEntity> GetEntities<TEntity>()
        {
            object typeDict;
            if (EntityCollections.TryGetValue(typeof(TEntity), out typeDict))
            {
                return (ConcurrentDictionary<string, TEntity>)typeDict;
            }

            return new ConcurrentDictionary<string, TEntity>();
        }
    }
}
