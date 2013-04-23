using System;
using System.Collections.Generic;

namespace CR.ViewModels.Core
{
    public interface IViewModelReader
    {
        TEntity GetByKey<TEntity>(string key) where TEntity : class;
        IEnumerable<TEntity> Query<TEntity>(Func<TEntity, bool> predicate) where TEntity : class;
    }
}