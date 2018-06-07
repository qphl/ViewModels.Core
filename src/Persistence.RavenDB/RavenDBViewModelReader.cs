// <copyright file="RavenDBViewModelReader.cs" company="Cognisant">
// Copyright (c) Cognisant. All rights reserved.
// </copyright>

namespace CR.ViewModels.Persistence.RavenDB
{
    using System;
    using System.Linq;
    using Core;
    using Raven.Client;

    /// <inheritdoc />
    /// <summary>
    /// A RavenDB implementation of the <see cref="T:CR.ViewModels.Core.IViewModelReader" /> interface.
    /// This implementation allows for retrieving View Models that have been stored in RavenDB.
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public class RavenDBViewModelReader : IViewModelReader
    {
        private readonly IDocumentStore _docStore;

        /// <summary>
        /// Initializes a new instance of the <see cref="RavenDBViewModelReader"/> class.
        /// </summary>
        /// <param name="store">The document store that should be used for the <see cref="RavenDBViewModelReader"/>.</param>
        public RavenDBViewModelReader(IDocumentStore store)
        {
            _docStore = store;
        }

        /// <inheritdoc />
        public TEntity GetByKey<TEntity>(string key)
            where TEntity : class
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (key == string.Empty)
            {
                throw new ArgumentException("key must not be an empty string", nameof(key));
            }

            using (var session = _docStore.OpenSession())
            {
                var loaded = session.Load<TEntity>(RavenDbViewModelHelper.MakeId<TEntity>(key));
                return loaded;
            }
        }

        /// <summary>
        /// <inheritdoc />
        /// NOTE: RavenDB will only return 128 items by default, to force you to use skip/take paging
        /// </summary>
        /// <inheritdoc />
        public IQueryable<TEntity> Query<TEntity>()
            where TEntity : class
        {
            using (var session = _docStore.OpenSession())
            {
                return session.Query<TEntity>();
            }
        }
    }
}
