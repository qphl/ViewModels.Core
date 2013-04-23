using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cr.viewmodels.core;

namespace persistance.memory
{
    /// <summary>
    /// In Memory ViewModel Repository
    /// Stores ViewModels to an internal dictionary
    /// </summary>
    public class InMemoryViewModelRepository : IViewModelReader, IViewModelWriter
    {
        
        private Dictionary<string, object> Items { get; set; }

        #region Constructor

        public InMemoryViewModelRepository()
        {
            Items = new Dictionary<string, object>();
        }

        #endregion

        #region IViewModelReader Implementation

        public TEntity GetByKey<TEntity>(string key, string category = "") where TEntity : class
        {
            throw new ArgumentException();
        }

        public IEnumerable<TEntity> Query<TEntity>(Func<TEntity, bool> predicate, string category = "") where TEntity : class
        {
            throw new NotImplementedException();
        }


        #endregion

        #region IViewModelWriter Implementation

        public void Add<TEntity>(string key, TEntity entity, string category = "") where TEntity : class
        {
            throw new NotImplementedException();
        }

        public void Update<TEntity>(string key, Action<TEntity> update, string category = "") where TEntity : class
        {
            throw new NotImplementedException();
        }

        public void UpdateWhere<TEntity>(Func<TEntity, bool> predicate, Action<TEntity> update, string category = "") where TEntity : class
        {
            throw new NotImplementedException();
        }

        public void Delete<TEntity>(string key, string category = "") where TEntity : class
        {
            throw new NotImplementedException();
        }

        public void DeleteWhere<TEntity>(Func<TEntity, bool> predicate, string category = "") where TEntity : class
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
