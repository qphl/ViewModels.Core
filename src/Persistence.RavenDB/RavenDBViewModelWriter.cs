// <copyright file="RavenDBViewModelWriter.cs" company="Cognisant">
// Copyright (c) Cognisant. All rights reserved.
// </copyright>

namespace CR.ViewModels.Persistence.RavenDB
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using Core;
    using Core.Exceptions;
    using Raven.Abstractions.Exceptions;
    using Raven.Client;

    /// <inheritdoc />
    /// <summary>
    /// A RavenDB implementation of the <see cref="T:CR.ViewModels.Core.IViewModelWriter" /> interface, which allows for writing View Models to a RavenDB document store.
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public class RavenDBViewModelWriter : IViewModelWriter
    {
        private const int RavenPageSize = 512;

        /// <summary>
        /// Initializes a new instance of the <see cref="RavenDBViewModelWriter"/> class which writes View Models to the provided <see cref="RavenDBViewModelWriter"/>.
        /// </summary>
        /// <param name="store">The document store that should be used for the <see cref="RavenDBViewModelWriter"/>.</param>
        public RavenDBViewModelWriter(IDocumentStore store) => DocStore = store;

        private IDocumentStore DocStore { get; }

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

            using (var session = DocStore.OpenSession())
            {
                session.Advanced.UseOptimisticConcurrency = true;
                session.Store(entity, RavenDbViewModelHelper.MakeId<TEntity>(key));
                try
                {
                    session.SaveChanges();
                }
                catch (ConcurrencyException ex)
                {
                    throw new DuplicateKeyException($"Attempt to insert key '{key}' failed.", ex, key);
                }
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

            using (var session = DocStore.OpenSession())
            {
                var entity = session.Load<TEntity>(RavenDbViewModelHelper.MakeId<TEntity>(key));
                if (entity == null)
                {
                    throw new EntityNotFoundException("The provided key to update was not found in the view model storage.", key);
                }

                update(entity);
                session.SaveChanges();
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

            using (var session = DocStore.OpenSession())
            {
                List<TEntity> resultChunk;
                var resultsToSkip = 0;

                do
                {
                    resultChunk = session.Query<TEntity>().Customize(x => x.WaitForNonStaleResultsAsOfNow())
                        .Where(predicate).Skip(resultsToSkip).Take(RavenPageSize).ToList();
                    resultsToSkip += resultChunk.Count;
                    foreach (var result in resultChunk)
                    {
                        update(result);
                    }
                }
                while (resultChunk.Count > 0);
                session.SaveChanges();
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

            using (var session = DocStore.OpenSession())
            {
                var toDelete = session.Load<TEntity>(RavenDbViewModelHelper.MakeId<TEntity>(key));
                if (toDelete == null)
                {
                    throw new EntityNotFoundException(key);
                }

                session.Delete(toDelete);
                session.SaveChanges();
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

            using (var session = DocStore.OpenSession())
            {
                List<TEntity> resultChunk;
                var resultsToSkip = 0;

                do
                {
                    resultChunk = session.Query<TEntity>().Customize(x => x.WaitForNonStaleResultsAsOfNow())
                        .Where(predicate).Skip(resultsToSkip).Take(RavenPageSize).ToList();
                    resultsToSkip += resultChunk.Count;
                    foreach (var result in resultChunk)
                    {
                        session.Delete(result);
                    }
                }
                while (resultChunk.Count > 0);
                session.SaveChanges();
            }
        }
    }
}
