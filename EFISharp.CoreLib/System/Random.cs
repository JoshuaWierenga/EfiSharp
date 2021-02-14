// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// Changes made by Joshua Wierenga.

namespace System
{
    /// <summary>
    /// Represents a pseudo-random number generator, which is an algorithm that produces a sequence of numbers
    /// that meet certain statistical requirements for randomness.
    /// </summary>
    public partial class Random
    {
        /// <summary>The underlying generator implementation.</summary>
        /// <remarks>
        /// This is separated out so that different generators can be used based on how this Random instance is constructed.
        /// If it's built from a seed, then we may need to ensure backwards compatibility for folks expecting consistent sequences
        /// based on that seed.  If the instance is actually derived from Random, then we need to ensure the derived type's
        /// overrides are called anywhere they were being called previously.  But if the instance is the base type and is constructed
        /// with the default constructor, we have a lot of flexibility as to how to optimize the performance and quality of the generator.
        /// </remarks>
        private readonly ImplBase _impl;

        /// <summary>Initializes a new instance of the <see cref="Random"/> class using a default seed value.</summary>
        public Random() =>
            // With no seed specified, if this is the base type, we can implement this however we like.
            // If it's a derived type, for compat we respect the previous implementation, so that overrides
            // are called as they were previously.
            //TODO Add XoshiroImpl and LegacyImpl
            //_impl = GetType() == typeof(Random) ? new XoshiroImpl() : new LegacyImpl(this);
            _impl = new EfiImpl();

        /* TODO Either add seed support to EfiS, is that even possible? or add LegacyImpl
        /// <summary>Initializes a new instance of the Random class, using the specified seed value.</summary>
        /// <param name="Seed">
        /// A number used to calculate a starting value for the pseudo-random number sequence. If a negative number
        /// is specified, the absolute value of the number is used.
        /// </param>
        public Random(int Seed) =>
            // With a custom seed, for compat we respect the previous implementation so that the same sequence
            // previously output continues to be output.
            _impl = new LegacyImpl(this, Seed);*/

        /// <summary>Fills the elements of a specified array of bytes with random numbers.</summary>
        /// <param name="buffer">The array to be filled with random numbers.</param>
        /// <!--<exception cref="ArgumentNullException"><paramref name="buffer"/> is null.</exception>-->
        public virtual void NextBytes(byte[] buffer)
        {
            if (buffer == null)
            {
                return;
                //ThrowHelper.ThrowArgumentNullException(ExceptionArgument.buffer);
            }

            _impl.NextBytes(buffer);
        }
    }
}
