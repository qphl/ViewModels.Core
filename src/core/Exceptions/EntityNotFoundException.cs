// <copyright file="EntityNotFoundException.cs" company="Cognisant">
// Copyright (c) Cognisant. All rights reserved.
// </copyright>

namespace CR.ViewModels.Core.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    /// <inheritdoc />
    /// <summary>
    /// Represents an error that occurs when an <see cref="T:CR.ViewModels.Core.IViewModelWriter" /> attempts to update or delete a View Model
    /// with a key that does not match an existing key in the specified view model storage implementation.
    /// </summary>
    [Serializable]
    public class EntityNotFoundException : Exception
    {
#pragma warning disable SA1648 // inheritdoc should be used with inheriting class - disabled due to missing implementation for construtors.
        /// <inheritdoc />
        public EntityNotFoundException(string key) => Key = key;

        /// <inheritdoc />
        // ReSharper disable once UnusedMember.Global
        public EntityNotFoundException(string message, string key)
            : base(message) => Key = key;

        /// <inheritdoc />
        // ReSharper disable once UnusedMember.Global
        public EntityNotFoundException(string message, Exception inner, string key)
            : base(message, inner) => Key = key;

        /// <inheritdoc />
        protected EntityNotFoundException(SerializationInfo info, StreamingContext context, string key)
            : base(info, context) => Key = key;
#pragma warning restore SA1648

        /// <summary>
        /// Gets the key which was not present in the view model storage implementation.
        /// </summary>
        public string Key { get; }
    }
}
