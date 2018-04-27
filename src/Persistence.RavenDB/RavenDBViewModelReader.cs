using System;
using System.Linq;
using CR.ViewModels.Core;
using Raven.Client;

namespace CR.ViewModels.Persistence.RavenDB
{
    public class RavenDBViewModelReader : IViewModelReader
    {
        private readonly IDocumentStore _docStore;

        public RavenDBViewModelReader(IDocumentStore store)
        {
            _docStore = store;
        }

        public TEntity GetByKey<TEntity>(string key) where TEntity : class
        {
            if (key == null)
                throw new ArgumentNullException("key");

            if (key == "")
                throw new ArgumentException("key must not be an empty string", "key");

            using (var session = _docStore.OpenSession())
            {
                var loaded = session.Load<TEntity>(MakeId<TEntity>(key));
                return loaded;
            }
        }

        /// <summary>
        /// NOTE: RavenDB will only return 128 items by default, to force you to use skip/take paging
        /// </summary>
        /// <typeparam name="TEntity">The type to query</typeparam>
        /// <returns></returns>
        public IQueryable<TEntity> Query<TEntity>() where TEntity : class
        {
            using (var session = _docStore.OpenSession())
            {
                return session.Query<TEntity>();
            }
        }

        private string MakeId<TEntity>(string key)
        {
            return typeof(TEntity).FullName + "/" + key;
        }
    }
}

