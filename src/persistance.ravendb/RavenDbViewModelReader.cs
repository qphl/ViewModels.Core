using System;
using System.Collections.Generic;
using System.Linq;
using CR.ViewModels.Core;
using CR.ViewModels.Core.Exceptions;
using Raven.Client;

namespace CR.ViewModels.Persistance.RavenDB
{
    public class RavenDbViewModelReader : IViewModelReader
    {
        private readonly IDocumentStore _docStore;

        public RavenDbViewModelReader(IDocumentStore store)
        {
            _docStore = store;
        }

        public TEntity GetByKey<TEntity>(string key) where TEntity : class
        {
            using (var session = _docStore.OpenSession())
            {
                var loaded = session.Load<TEntity>(key);

                if (loaded == null)
                    throw new EntityNotFoundException();

                return loaded;
            }
        }

        public IEnumerable<TEntity> Query<TEntity>(Func<TEntity, bool> predicate) where TEntity : class
        {
            //TODO: Fix paging
            using (var session = _docStore.OpenSession())
            {
                return session.Query<TEntity>().Where(predicate);
            }
        }
    }

    public class RavenDBViewModelWriter : IViewModelWriter
    {

        private IDocumentStore _docStore;

        public RavenDBViewModelWriter(IDocumentStore store)
        {
            _docStore = store;
        }

        public void Add<TEntity>(string key, TEntity entity) where TEntity : class
        {
            using (var session = _docStore.OpenSession())
            {
                session.Advanced.UseOptimisticConcurrency = true;
                session.Store(entity, key);
                session.SaveChanges();
            }
        }

        public void Update<TEntity>(string key, Action<TEntity> update) where TEntity : class
        {
            using (var session = _docStore.OpenSession())
            {
                var entity = session.Load<TEntity>(key);

                if (entity == null)
                    throw new EntityNotFoundException();

                update(entity);
                session.SaveChanges();
            }
        }

        public void UpdateWhere<TEntity>(Func<TEntity, bool> predicate, Action<TEntity> update) where TEntity : class
        {
            using (var session = _docStore.OpenSession())
            {
                var results = session.Query<TEntity>().Where(predicate).Take(1000).ToList();

                //TODO: Deal with stupid ravendb paging

                foreach (var entity in results)
                {
                    update(entity);
                }

                session.SaveChanges();
            }
        }

        public void Delete<TEntity>(string key) where TEntity : class
        {
            using (var session = _docStore.OpenSession())
            {
                var toDelete = session.Load<TEntity>(key);

                if (toDelete == null)
                    throw new EntityNotFoundException();

                session.Delete(toDelete);
                session.SaveChanges();
            }
        }

        public void DeleteWhere<TEntity>(Func<TEntity, bool> predicate) where TEntity : class
        {
            using (var session = _docStore.OpenSession())
            {
                var results = session.Query<TEntity>().Where(predicate).Take(1000).ToList();

                //TODO: Deal with stupid ravendb paging

                foreach (var entity in results)
                {
                    session.Delete(entity);
                }

                session.SaveChanges();
            }
        }
    }
}

