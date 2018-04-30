// <copyright file="RavenDBViewModelWriter.cs" company="Cognisant">
// Copyright (c) Cognisant. All rights reserved.
// </copyright>

namespace CR.ViewModels.Persistence.RavenDB
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using CR.ViewModels.Core;
    using CR.ViewModels.Core.Exceptions;
    using Raven.Abstractions.Exceptions;
    using Raven.Client;

    public class RavenDBViewModelWriter : IViewModelWriter
    {
        private const int RAVENPAGESIZE = 512;
        private IDocumentStore _docStore;

        public RavenDBViewModelWriter(IDocumentStore store)
        {
            _docStore = store;
        }

        public void Add<TEntity>(string key, TEntity entity)
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

            using (var session = _docStore.OpenSession())
            {
                session.Advanced.UseOptimisticConcurrency = true;
                session.Store(entity, MakeId<TEntity>(key));
                try
                {
                    session.SaveChanges();
                }
                catch (ConcurrencyException ex)
                {
                    throw new DuplicateKeyException("Attempt to insert key '" + key + "' failed", ex);
                }
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

            using (var session = _docStore.OpenSession())
            {
                var entity = session.Load<TEntity>(MakeId<TEntity>(key));

                if (entity == null)
                {
                    throw new EntityNotFoundException();
                }

                update(entity);
                session.SaveChanges();
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

            using (var session = _docStore.OpenSession())
            {
                List<TEntity> resultChunk;
                int resultsToSkip = 0;

                do
                {
                    resultChunk = session.Query<TEntity>().Customize(x => x.WaitForNonStaleResultsAsOfNow()).Where(predicate).Skip(resultsToSkip).Take(RAVENPAGESIZE).ToList();
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

            using (var session = _docStore.OpenSession())
            {
                var toDelete = session.Load<TEntity>(MakeId<TEntity>(key));

                if (toDelete == null)
                {
                    throw new EntityNotFoundException();
                }

                session.Delete(toDelete);
                session.SaveChanges();
            }
        }

        public void DeleteWhere<TEntity>(Expression<Func<TEntity, bool>> predicate)
            where TEntity : class
        {
            if (predicate == null)
            {
                throw new ArgumentNullException("predicate");
            }

            using (var session = _docStore.OpenSession())
            {
                List<TEntity> resultChunk;
                int resultsToSkip = 0;

                do
                {
                    resultChunk = session.Query<TEntity>().Customize(x => x.WaitForNonStaleResultsAsOfNow()).Where(predicate).Skip(resultsToSkip).Take(RAVENPAGESIZE).ToList();
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

        private string MakeId<TEntity>(string key)
        {
            return typeof(TEntity).FullName + "/" + key;
        }
    }
}
