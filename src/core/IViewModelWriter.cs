using System;
using System.Linq.Expressions;

namespace CR.ViewModels.Core
{
    public interface IViewModelWriter
    {
        void Add<TEntity>(string key, TEntity entity) where TEntity : class;

        void Update<TEntity>(string key, Action<TEntity> update) where TEntity : class;

        void UpdateWhere<TEntity>(Expression<Func<TEntity,bool>> predicate, Action<TEntity> update) where TEntity : class;

        void Delete<TEntity>(string key) where TEntity : class;

        void DeleteWhere<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class;
    }
}
