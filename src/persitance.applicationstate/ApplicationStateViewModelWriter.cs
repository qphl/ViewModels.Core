using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using CR.ViewModels.Core;

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
            var entities = GetEntities<TEntity>();
            entities.Add(key, entity);
        }

        public void Update<TEntity>(string key, Action<TEntity> update) where TEntity : class
        {
            var entities = GetEntities<TEntity>();
            update(entities[key]);
        }

        public void UpdateWhere<TEntity>(Func<TEntity, bool> predicate, Action<TEntity> update) where TEntity : class
        {
            var entities = GetEntities<TEntity>();
            var targets = entities.Values.Where(predicate).ToList();
            Parallel.ForEach(targets, update);
        }

        public void Delete<TEntity>(string key) where TEntity : class
        {
            var entities = GetEntities<TEntity>();
            entities.Remove(key);
        }

        public void DeleteWhere<TEntity>(Func<TEntity, bool> predicate) where TEntity : class
        {
            var entities = GetEntities<TEntity>();
            foreach (var entity in entities.Where(e => predicate(e.Value)))
            {
                entities.Remove(entity.Key);
            }
        }

        private Dictionary<String, TEntity> GetEntities<TEntity>()
        {
            var appStateKey = typeof(TEntity).FullName;
            var entities = AppState[appStateKey] ?? (AppState[appStateKey] = new Dictionary<String, TEntity>());
            return (Dictionary<String, TEntity>)entities;
        }
    }
}