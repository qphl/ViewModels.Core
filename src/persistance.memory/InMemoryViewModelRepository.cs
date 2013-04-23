using System;
using System.Collections.Generic;
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
        
        private Dictionary<Type, Dictionary<string,object>> Entities { get; set; }

        #region Constructor

        public InMemoryViewModelRepository()
        {
            Entities = new Dictionary<Type, Dictionary<string, object>>();
        }

        #endregion

        #region IViewModelReader Implementation

        public TEntity GetByKey<TEntity>(string key) where TEntity : class
        {
            if(key == null)
                throw new ArgumentNullException("key");

            if(key == "")
                throw new ArgumentException("key must not be an empty string", "key");

            if (!Entities.ContainsKey(typeof (TEntity)))
                return null;
            
            object result;
            
            if (Entities[typeof(TEntity)].TryGetValue(key, out result))
                return (TEntity)result;

            return null;
        }

        public IEnumerable<TEntity> Query<TEntity>(Func<TEntity, bool> predicate) where TEntity : class
        {
            throw new NotImplementedException();
            //return new List<TEntity>();
        }
        
        #endregion

        #region IViewModelWriter Implementation

        public void Add<TEntity>(string key, TEntity entity) where TEntity : class
        {
            if (!Entities.ContainsKey(typeof(TEntity)))
                Entities.Add(typeof(TEntity), new Dictionary<string, object>());

            var typeDict = Entities[typeof (TEntity)];

            if(typeDict.ContainsKey(key))
                throw new DuplicateKeyException("An entity with this key has alreaady been added");

            typeDict.Add(key, entity);
        }

        public void Update<TEntity>(string key, Action<TEntity> update) where TEntity : class
        {
            throw new NotImplementedException();
        }

        public void UpdateWhere<TEntity>(Func<TEntity, bool> predicate, Action<TEntity> update) where TEntity : class
        {
            throw new NotImplementedException();
        }

        public void Delete<TEntity>(string key) where TEntity : class
        {
            throw new NotImplementedException();
        }

        public void DeleteWhere<TEntity>(Func<TEntity, bool> predicate) where TEntity : class
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
