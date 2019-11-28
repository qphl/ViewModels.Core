// <copyright file="DuplicateKeyException.cs" company="Cognisant">
// Copyright (c) Cognisant. All rights reserved.
// </copyright>

namespace CR.ViewModels.Core.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    /// <inheritdoc />
    /// <summary>
    /// Represents an error that occurs when an <see cref="IViewModelWriter" /> attempts to write a View Model
    /// with a key that matches an existing key in the specified view model storage implementation.
    /// </summary>
    [Serializable]
    public class DuplicateKeyException : Exception
    {
        // ReSharper disable once UnusedMember.Global
#pragma warning disable SA1648 // inheritdoc should be used with inheriting class - disabled due to missing implementation for construtors.
        /// <inheritdoc />
        public DuplicateKeyException(string key) => Key = key;

        /// <inheritdoc />
        public DuplicateKeyException(string message, string key)
            : base(message) => Key = key;

        /// <inheritdoc />
        // ReSharper disable once UnusedMember.Global
        public DuplicateKeyException(string message, Exception inner, string key)
            : base(message, inner) => Key = key;

        /// <inheritdoc />
        // ReSharper disable once UnusedMember.Global
        protected DuplicateKeyException(SerializationInfo info, StreamingContext context, string key)
            : base(info, context) => Key = key;
#pragma warning restore SA1648 // inheritdoc should be used with inheriting class

        /// <summary>
        /// Gets the key which was already present in the view model storage implementation.
        /// </summary>
        public string Key { get; }
    }
}
