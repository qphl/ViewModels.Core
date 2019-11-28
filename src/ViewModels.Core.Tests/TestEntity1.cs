// <copyright file="TestEntity1.cs" company="Corsham Science">
// Copyright (c) Corsham Science. All rights reserved.
// </copyright>

namespace CorshamScience.ViewModels.Core.Tests
{
    using System;

    /// <inheritdoc />
    /// <summary>
    /// An example object used for testing.
    /// </summary>
    internal sealed class TestEntity1 : IEquatable<TestEntity1>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TestEntity1"/> class.
        /// </summary>
        /// <param name="id">The identifier for the test object.</param>
        /// <param name="field1">The test field.</param>
        public TestEntity1(string id, string field1)
        {
            Identifier = id;
            Field1 = field1;
        }

        /// <summary>
        /// Gets or sets the identifier for the test object.
        /// </summary>
        public string Identifier { get; set; }

        /// <summary>
        /// Gets or sets a test field.
        /// </summary>
        public string Field1 { get; set; }

        /// <inheritdoc />
        public bool Equals(TestEntity1 other) => other != null && string.Equals(Identifier, other.Identifier) && string.Equals(Field1, other.Field1);

        /// <inheritdoc />
        public override bool Equals(object obj) => obj is TestEntity1 other && Equals(other);

        /// <inheritdoc />
        // ReSharper disable NonReadonlyMemberInGetHashCode
        public override int GetHashCode()
        {
            unchecked
            {
                return ((Identifier != null ? Identifier.GetHashCode() : 0) * 397) ^ (Field1 != null ? Field1.GetHashCode() : 0);
            }
        } // ReSharper restore NonReadonlyMemberInGetHashCode
    }
}
