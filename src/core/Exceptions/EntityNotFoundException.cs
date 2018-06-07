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
        public EntityNotFoundException()
        {
        }

        /// <inheritdoc />
        // ReSharper disable once UnusedMember.Global
        public EntityNotFoundException(string message)
            : base(message)
        {
        }

        /// <inheritdoc />
        // ReSharper disable once UnusedMember.Global
        public EntityNotFoundException(string message, Exception inner)
            : base(message, inner)
        {
        }

        /// <inheritdoc />
        protected EntityNotFoundException(
            SerializationInfo info,
            StreamingContext context)
            : base(info, context)
        {
        }
#pragma warning restore SA1648
    }
}
