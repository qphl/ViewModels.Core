using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using CR.ViewModels.Core;
using CR.ViewModels.Core.Exceptions;

namespace CR.ViewModels.Persitance.ApplicationState
{
    public class ApplicationStateViewModelWriter : IViewModelWriter
    {
        private HttpApplicationStateBase AppState { get; set; }

        public ApplicationStateViewModelWriter(HttpApplicationStateBase appState)
        {
                AppState = appState;
        }

        public void Add<TEntity>(string key, TEntity entity) where TEntity : class
        {
            if (AppState[typeof(TEntity).FullName] == null)
                AppState[typeof(TEntity).FullName] = new Dictionary<string, TEntity>();

            var entities = GetEntities<TEntity>();

            if (entities.ContainsKey(key))
                throw new DuplicateKeyException("An entity with this key has alreaady been added");
            entities.Add(key, entity);
        }

        public void Update<TEntity>(string key, Action<TEntity> update) where TEntity : class
        {
            if (key == null)
                throw new ArgumentNullException("key");

            if (key == "")
                throw new ArgumentException("key must not be an empty string", "key");

            if (update == null)
                throw new ArgumentNullException("update");

            var entities = GetEntities<TEntity>();
            TEntity entity;

            if (entities.TryGetValue(key, out entity))
                update(entity);
            else
                throw new EntityNotFoundException();
        }

        public void UpdateWhere<TEntity>(Func<TEntity, bool> predicate, Action<TEntity> update) where TEntity : class
        {
            if (predicate == null)
                throw new ArgumentNullException("predicate");

            if (update == null)
                throw new ArgumentNullException("update");

            var entities = GetEntities<TEntity>();
            var targets = entities.Values.Where(predicate).ToList();
            Parallel.ForEach(targets, update);
        }

        public void Delete<TEntity>(string key) where TEntity : class
        {
            if (key == null)
                throw new ArgumentNullException("key");

            if (key == "")
                throw new ArgumentException("key must not be an empty string", "key");

            var entities = GetEntities<TEntity>();

            if (!entities.ContainsKey(key))
                throw new EntityNotFoundException();

            entities.Remove(key);
        }

        public void DeleteWhere<TEntity>(Func<TEntity, bool> predicate) where TEntity : class
        {
            if (predicate == null)
                throw new ArgumentNullException("predicate");

            var entities = GetEntities<TEntity>();

            var toDelete = entities.Where(e => predicate(e.Value)).ToList();

            foreach (var entity in toDelete)
                entities.Remove(entity.Key);
        }

        private Dictionary<String, TEntity> GetEntities<TEntity>()
        {
            var appStateKey = typeof(TEntity).FullName;
            return AppState[appStateKey] != null
                       ? (Dictionary<string, TEntity>) AppState[appStateKey]
                       : new Dictionary<string, TEntity>();
        }
    }
}