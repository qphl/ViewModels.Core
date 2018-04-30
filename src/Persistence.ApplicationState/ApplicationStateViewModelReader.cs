// <copyright file="ApplicationStateViewModelReader.cs" company="Cognisant">
// Copyright (c) Cognisant. All rights reserved.
// </copyright>

namespace CR.ViewModels.Persistence.ApplicationState
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using Core;

    public class ApplicationStateViewModelReader : IViewModelReader
    {
        public ApplicationStateViewModelReader(HttpApplicationStateBase appState)
        {
            AppState = appState;
        }

        private HttpApplicationStateBase AppState { get; set; }

        public TEntity GetByKey<TEntity>(string key)
            where TEntity : class
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }

            if (key == string.Empty)
            {
                throw new ArgumentException("key must not be an empty string", "key");
            }

            var entities = GetEntities<TEntity>();

            TEntity result;
            return entities.TryGetValue(key, out result) ? result : null;
        }

        public IQueryable<TEntity> Query<TEntity>()
            where TEntity : class
        {
            var entities = GetEntities<TEntity>();
            return entities.Values.AsQueryable();
        }

        private Dictionary<string, TEntity> GetEntities<TEntity>()
        {
            var appStateKey = typeof(TEntity).FullName;
            var entities = AppState[appStateKey];
            return entities == null ? new Dictionary<string, TEntity>() : (Dictionary<string, TEntity>)entities;
        }
    }
}
