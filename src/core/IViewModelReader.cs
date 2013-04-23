using System;
using System.Collections.Generic;

namespace cr.viewmodels.core
{
    public interface IViewModelReader
    {
        TEntity GetByKey<TEntity>(string key, string category = "") where TEntity : class;
        IEnumerable<TEntity> Query<TEntity>(Func<TEntity, bool> predicate, string category = "") where TEntity : class;
    }
}