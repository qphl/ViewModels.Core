using System;
using System.Collections.Generic;
using System.Linq;
using CR.ViewModels.Core;
using CR.ViewModels.Core.Exceptions;

namespace CR.ViewModels.Persistance.Memory
{
    /// <summary>
    /// In Memory ViewModel Repository
    /// Stores ViewModels to an internal dictionary
    /// </summary>
    public class InMemoryViewModelRepository : IViewModelReader, IViewModelWriter
    {
        
        private Dictionary<Type, object> EntityCollections { get; set; }

        #region Constructor

        public InMemoryViewModelRepository()
        {
            EntityCollections = new Dictionary<Type, object>();
        }

        #endregion

        #region IViewModelReader Implementation

        public TEntity GetByKey<TEntity>(string key) where TEntity : class
        {
            if(key == null)
                throw new ArgumentNullException("key");

            if(key == "")
                throw new ArgumentException("key must not be an empty string", "key");

            var entities = GetEntities<TEntity>();
            
            TEntity result;
            return entities.TryGetValue(key, out result) ? result : null;
        }

        public IEnumerable<TEntity> Query<TEntity>(Func<TEntity, bool> predicate) where TEntity : class
        {
            var entities = GetEntities<TEntity>();
            return entities.Values.Where(predicate);
        }
        
        #endregion

        #region IViewModelWriter Implementation

        public void Add<TEntity>(string key, TEntity entity) where TEntity : class
        {
            if (!EntityCollections.ContainsKey(typeof(TEntity)))
                EntityCollections.Add(typeof(TEntity), new Dictionary<string, TEntity>());

            var entities = GetEntities<TEntity>();

            if(entities.ContainsKey(key))
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
            {
                update(entity);
            }
            else
            {
                throw new EntityNotFoundException();
            }
        }

        public void UpdateWhere<TEntity>(Func<TEntity, bool> predicate, Action<TEntity> update) where TEntity : class
        {
            if (predicate == null)
                throw new ArgumentNullException("predicate");

            if (update == null)
                throw new ArgumentNullException("update");
            

            var entities = GetEntities<TEntity>();
            var toUpdate = entities.Values.Where(predicate).ToList();

            foreach (var ent in toUpdate)
                update(ent);
        }

        public void Delete<TEntity>(string key) where TEntity : class
        {
            if(key == null)
                throw new ArgumentNullException("key");

            if (key == "")
                throw new ArgumentException("key must not be an empty string", "key");

            var entities = GetEntities<TEntity>();

            if(!entities.ContainsKey(key))
                throw new EntityNotFoundException();

            entities.Remove(key);
        }

        public void DeleteWhere<TEntity>(Func<TEntity, bool> predicate) where TEntity : class
        {
            if (predicate == null)
                throw new ArgumentNullException("predicate");

            var entities = GetEntities<TEntity>();
            var toDelete = entities.Where(e => predicate(e.Value)).ToList();

            foreach (var ent in toDelete)
                entities.Remove(ent.Key);
        }

        #endregion

        private Dictionary<string, TEntity> GetEntities<TEntity>()
        {
            object typeDict;
            if (EntityCollections.TryGetValue(typeof (TEntity), out typeDict))
            {
                return (Dictionary<string, TEntity>) typeDict;
            }
            
            return new Dictionary<string, TEntity>();
        }
    }
}
