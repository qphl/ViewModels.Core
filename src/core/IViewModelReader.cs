using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace CR.ViewModels.Core
{
    public interface IViewModelReader
    {
        TEntity GetByKey<TEntity>(string key) where TEntity : class;
        IEnumerable<TEntity> Query<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class;
    }
}