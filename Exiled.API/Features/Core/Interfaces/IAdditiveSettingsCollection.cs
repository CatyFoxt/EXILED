// -----------------------------------------------------------------------
// <copyright file="IAdditiveSettingsCollection.cs" company="Exiled Team">
// Copyright (c) Exiled Team. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace Exiled.API.Features.Core.Interfaces
{
    using System.Collections.Generic;

    using Exiled.API.Features.Core.Behaviours;

    /// <summary>
    /// Defines a collection of additive settings set up through user-defined properties.
    /// </summary>
    /// <typeparam name="T">The <see cref="EPlayerBehaviour"/> type.</typeparam>
    public interface IAdditiveSettingsCollection<T> : IAdditivePipe
        where T : IAdditiveProperty
    {
        /// <summary>
        /// Gets or sets the <typeparamref name="T"/> settings.
        /// </summary>
        public List<T> Settings { get; set; }

        /// <summary>
        /// Gets or sets the configs.
        /// </summary>
        public object Config { get; set; }
    }
}