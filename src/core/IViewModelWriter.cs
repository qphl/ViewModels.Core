using System;
using System.Collections.Generic;

namespace cr.viewmodels.core
{
    public interface IViewModelWriter
    {
        void Add<TEntity>(string key, TEntity entity, string category = "") where TEntity : class;

        void Update<TEntity>(string key, Action<TEntity> update, string category = "") where TEntity : class;

        void UpdateWhere<TEntity>(Func<TEntity, bool> predicate, Action<TEntity> update, string category = "") where TEntity : class;

        void Delete<TEntity>(string key, string category = "") where TEntity : class;

        void DeleteWhere<TEntity>(Func<TEntity, bool> predicate, string category = "") where TEntity : class;
    }
}
