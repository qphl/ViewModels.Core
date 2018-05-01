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

    /// <summary>
    /// A RavenDB implementation of the <see cref="IViewModelWriter"/> interface.
    /// This implementation allows for writing View Models to a RavenDB document store.
    /// </summary>
    public class RavenDBViewModelWriter : IViewModelWriter
    {
        private const int RavenPageSize = 512;
        private readonly IDocumentStore _docStore;

        /// <summary>
        /// Initializes a new instance of the <see cref="RavenDBViewModelWriter"/> class.
        /// </summary>
        /// <param name="store">The document store that should be used for the <see cref="RavenDBViewModelWriter"/>.</param>
        public RavenDBViewModelWriter(IDocumentStore store)
        {
            _docStore = store;
        }

        /// <inheritdoc />
        public void Add<TEntity>(string key, TEntity entity)
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

            using (var session = _docStore.OpenSession())
            {
                session.Advanced.UseOptimisticConcurrency = true;
                session.Store(entity, RavenDBViewModelHelper.MakeID<TEntity>(key));
                try
                {
                    session.SaveChanges();
                }
                catch (ConcurrencyException ex)
                {
                    throw new DuplicateKeyException($"Attempt to insert key \'{key}\' failed", ex);
                }
            }
        }

        /// <inheritdoc />
        public void Update<TEntity>(string key, Action<TEntity> update)
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

            if (update == null)
            {
                throw new ArgumentNullException(nameof(update));
            }

            using (var session = _docStore.OpenSession())
            {
                var entity = session.Load<TEntity>(RavenDBViewModelHelper.MakeID<TEntity>(key));

                if (entity == null)
                {
                    throw new EntityNotFoundException();
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
                throw new ArgumentNullException(nameof(predicate));
            }

            if (update == null)
            {
                throw new ArgumentNullException(nameof(update));
            }

            using (var session = _docStore.OpenSession())
            {
                List<TEntity> resultChunk;
                int resultsToSkip = 0;

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
                throw new ArgumentNullException(nameof(key));
            }

            if (key == string.Empty)
            {
                throw new ArgumentException("key must not be an empty string", nameof(key));
            }

            using (var session = _docStore.OpenSession())
            {
                var toDelete = session.Load<TEntity>(RavenDBViewModelHelper.MakeID<TEntity>(key));

                if (toDelete == null)
                {
                    throw new EntityNotFoundException();
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
                throw new ArgumentNullException(nameof(predicate));
            }

            using (var session = _docStore.OpenSession())
            {
                List<TEntity> resultChunk;
                int resultsToSkip = 0;

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
