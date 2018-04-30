// <copyright file="IViewModelReader.cs" company="Cognisant">
// Copyright (c) Cognisant. All rights reserved.
// </copyright>

namespace CR.ViewModels.Core
{
    using System.Linq;

    public interface IViewModelReader
    {
        TEntity GetByKey<TEntity>(string key)
            where TEntity : class;

        IQueryable<TEntity> Query<TEntity>()
            where TEntity : class;
    }
}
