// <copyright file="DuplicateKeyException.cs" company="Cognisant">
// Copyright (c) Cognisant. All rights reserved.
// </copyright>

namespace CR.ViewModels.Core.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    /// <inheritdoc />
    /// <summary>
    /// Represents an error that occurs when an <see cref="T:CR.ViewModels.Core.IViewModelWriter" /> attempts to write a View Model
    /// with a key that matches an existing key in the specified view model storage implementation.
    /// </summary>
    [Serializable]
    public class DuplicateKeyException : Exception
    {
        // ReSharper disable once UnusedMember.Global
#pragma warning disable SA1648 // inheritdoc should be used with inheriting class - disabled due to missing implementation for construtors.
        /// <inheritdoc />
        public DuplicateKeyException()
        {
        }

        /// <inheritdoc />
        public DuplicateKeyException(string message)
            : base(message)
        {
        }

        /// <inheritdoc />
        public DuplicateKeyException(string message, Exception inner)
            : base(message, inner)
        {
        }

        /// <inheritdoc />
        protected DuplicateKeyException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
#pragma warning restore SA1648 // inheritdoc should be used with inheriting class
    }
}
