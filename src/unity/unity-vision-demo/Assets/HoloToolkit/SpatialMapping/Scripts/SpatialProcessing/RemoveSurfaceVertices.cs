// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HoloToolkit.Unity
{
    /// <summary>
    /// RemoveSurfaceVertices will remove any vertices from the Spatial Mapping Mesh that fall within the bounding volume.
    /// This can be used to create holes in the environment, or to help reduce triangle count after finding planes.
    /// </summary>
    public class RemoveSurfaceVertices : Singleton<RemoveSurfaceVertices>
    {
        [Tooltip("The amount, if any, to expand each bounding volume by.")]
        public float BoundsExpansion = 0.0f;

        /// <summary>
        /// Delegate which is called when the RemoveVerticesComplete event is triggered.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="args"></param>
        public delegate void EventHandler(object source, EventArgs args);

        /// <summary>
        /// EventHandler which is triggered when the RemoveSurfaceVertices is finished.
        /// </summary>
        public event EventHandler RemoveVerticesComplete;

        /// <summary>
        /// Indicates if RemoveSurfaceVertices is currently removing vertices from the Spatial Mapping Mesh.
        /// </summary>
        private bool removingVerts = false;

        /// <summary>
        /// Queue of bounding objects to remove surface vertices