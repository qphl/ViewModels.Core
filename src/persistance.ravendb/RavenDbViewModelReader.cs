using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using CR.ViewModels.Core;
using CR.ViewModels.Core.Exceptions;
using Raven.Client;

namespace CR.ViewModels.Persistance.RavenDB
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

        /*public IEnumerable<TEntity> Query<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class
        {
            using (var session = _docStore.OpenSession())
            {
                List<TEntity> resultChunk;
                int resultsToSkip = 0;
                const int pageSize = 512;
                do
                {
                    resultChunk = session.Query<TEntity>().Customize(x => x.WaitForNonStaleResultsAsOfNow()).Where(predicate).Skip(resultsToSkip).Take(pageSize).ToList();
                    resultsToSkip += resultChunk.Count;
                    foreach(var result in resultChunk)
                        yield return result;
                } while (resultChunk.Count > 0);
            }
        }*/

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

