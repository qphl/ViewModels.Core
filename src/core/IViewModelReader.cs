// <copyright file="IViewModelReader.cs" company="Cognisant">
// Copyright (c) Cognisant. All rights reserved.
// </copyright>

namespace CR.ViewModels.Core
{
    using System;
    using System.Linq;

    /// <summary>
    /// An interface that will be extended by any class that allows for reading view models.
    /// </summary>
    public interface IViewModelReader
    {
        /// <summary>
        /// Gets a view model of type TEntity using the key provided.
        /// </summary>
        /// <typeparam name="TEntity">The type of the view model to retrieve.</typeparam>
        /// <param name="key">The key of the view model being retrieved.</param>
        /// <returns>The view model of type TEntity that corresponds to the given key.</returns>
        /// <exception cref="ArgumentNullException">Exception will be thrown whenever null is passed in for parameter key.</exception>
        /// <exception cref="ArgumentException">Exception will be thrown whenever an empty string is passed in for parameter key.</exception>
        TEntity GetByKey<TEntity>(string key)
            where TEntity : class;

        /// <summary>
        /// Allows for querying over all view models of type TEntity in the view model reader.
        /// </summary>
        /// <typeparam name="TEntity">The type of the view model to query over.</typeparam>
        /// <returns>A queryable that can be used to query the view model reader for type TEntity</returns>
        IQueryable<TEntity> Query<TEntity>()
            where TEntity : class;
    }
}
