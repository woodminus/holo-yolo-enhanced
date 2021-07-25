// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace HoloToolkit.Unity
{
    /// <summary>
    /// The SpatialMappingManager class allows applications to use a SurfaceObserver or a stored 
    /// Spatial Mapping mesh (loaded from a file).
    /// When an application loads a mesh file, the SurfaceObserver is stopped.
    /// Calling StartObserver() clears the stored mesh and enables real-time SpatialMapping updates.
    /// </summary>
    [RequireComponent(typeof(SpatialMappingObserver))]
    public partial class SpatialMappi