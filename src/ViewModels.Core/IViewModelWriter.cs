// <copyright file="IViewModelWriter.cs" company="Corsham Science">
// Copyright (c) Corsham Science. All rights reserved.
// </copyright>

namespace CorshamScience.ViewModels.Core
{
    using System;
    using System.Linq.Expressions;
    using CorshamScience.ViewModels.Core.Exceptions;

    /// <summary>
    /// An interface that will be extended by any class that allows for writing view models.
    /// </summary>
    public interface IViewModelWriter
    {
        /// <summary>
        /// Attempts to add the specified view model of type TEntity to the view model storage.
        /// </summary>
        /// <typeparam name="TEntity">The type of the view model to store.</typeparam>
        /// <param name="key">The key of the view model being stored.</param>
        /// <param name="entity">The view model of type TEntity that is being stored.</param>
        /// <exception cref="ArgumentNullException">Exception will be thrown whenever null is passed in for either of the parameters.</exception>
        /// <exception cref="ArgumentException">Exception will be thrown whenever an empty string is passed in for parameter key.</exception>
        /// <exception cref="DuplicateKeyException">Exception will be thrown when the given key for a view model of type TEntity is already stored in the view model storage for this <see cref="IViewModelWriter"/>.</exception>
        void Add<TEntity>(string key, TEntity entity)
            where TEntity : class;

        /// <summary>
        /// Attempts to update the specified view model of type TEntity in the view model storage using the provided delegate.
        /// </summary>
        /// <typeparam name="TEntity">The type of the view model to update.</typeparam>
        /// <param name="key">The key of the view model being updated.</param>
        /// <param name="update">The update delegate that should be performed on the view model.</param>
        /// <exception cref="ArgumentNullException">Exception will be thrown whenever null is passed in for either of the parameters.</exception>
        /// <exception cref="ArgumentException">Exception will be thrown whenever an empty string is passed in for parameter key.</exception>
        /// <exception cref="EntityNotFoundException">Exception will be thrown when attempting to update a key that is not present in the view model storage.</exception>
        void Update<TEntity>(string key, Action<TEntity> update)
            where TEntity : class;

        /// <summary>
        /// Attempts to update all view models of type TEntity in the view model storage matching the provided expression using the provided delegate.
        /// </summary>
        /// <typeparam name="TEntity">The type of the view models to update.</typeparam>
        /// <param name="predicate">A predicate used to determine which view models to update.</param>
        /// <param name="update">The update delegate that should be performed on the selected view model.</param>
        /// <exception cref="ArgumentNullException">Exception will be thrown whenever null is passed in for either of the parameters.</exception>
        void UpdateWhere<TEntity>(Expression<Func<TEntity, bool>> predicate, Action<TEntity> update)
            where TEntity : class;

        /// <summary>
        /// Attempts to delete the specified view model of type TEntity from the view model storage using the provided delegate.
        /// </summary>
        /// <typeparam name="TEntity">The type of the view model to delete.</typeparam>
        /// <param name="key">The key of the view model being deleted.</param>
        /// <exception cref="ArgumentNullException">Exception will be thrown whenever null is passed in for parameter key.</exception>
        /// <exception cref="ArgumentException">Exception will be thrown whenever an empty string is passed in for parameter key.</exception>
        /// <exception cref="EntityNotFoundException">Exception will be thrown when attempting to delete a key that is not present in the view model storage.</exception>
        void Delete<TEntity>(string key)
            where TEntity : class;

        /// <summary>
        /// Attempts to delete all view models of type TEntity in the view model storage matching the provided expression.
        /// </summary>
        /// <typeparam name="TEntity">The type of the view models to delete.</typeparam>
        /// <param name="predicate">A predicate used to determine which view models to delete.</param>
        /// <exception cref="ArgumentNullException">Exception will be thrown whenever null is passed in for the parameter predicate.</exception>
        void DeleteWhere<TEntity>(Expression<Func<TEntity, bool>> predicate)
            where TEntity : class;
    }
}
