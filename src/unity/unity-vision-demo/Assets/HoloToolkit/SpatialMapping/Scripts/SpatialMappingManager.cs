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
    public partial class SpatialMappingManager : Singleton<SpatialMappingManager>
    {
        [Tooltip("The physics layer for spatial mapping objects to be set to.")]
        public int PhysicsLayer = 31;

        [Tooltip("The material to use for rendering spatial mapping data.")]
        public Material surfaceMaterial;

        [Tooltip("Determines if the surface observer should be automatically started.")]
        public bool autoStartObserver = true;

        [Tooltip("Determines if spatial mapping data will be rendered.")]
        public bool drawVisualMeshes = false;

        [Tooltip("Determines if spatial mapping data will cast shadows.")]
        public bool castShadows = false;

        /// <summary>
        /// Used for gathering real-time Spatial Mapping data on the HoloLens.
        /// </summary>
        private SpatialMappingObserver surfaceObserver;

        /// <summary>
        /// Time when StartObserver() was called.
        /// </summary>
        [HideInInspector]
        public float StartTime { get; private set; }

        /// <summary>
        /// The current source of spatial mapping data.
        /// </summary>
        public SpatialMappingSource Source { get; private set; }

        // Called when the GameObject is first created.
        private void Awake()
        {
            surfaceObserver = gameObject.GetComponent<SpatialMappingObserver>();
            Source = surfaceObserver;
        }

        // Use for initialization.
        private void Start()
        {
            if (autoStartObserver)
            {
                StartObserver();
            }
        }

        /// <summary>
        /// Returns the layer as a bit mask.
        /// </summary>
        public int LayerMask
        {
            get { return (1 << PhysicsLayer); }
        }

        /// <summary>
        /// The material to use when rendering surfaces.
        /// </summary>
        public Material SurfaceMaterial
        {
            get
            {
                return surfaceMaterial;
            }
            set
            {
                if (value != surfaceMaterial)
                {
                    surfaceMaterial = value;
                    SetSurfaceMaterial(surfaceMaterial);
                }
            }
        }

        /// <summary>
        /// Specifies whether or not the SpatialMapping meshes are to be rendered.
        /// </summary>
        public bool DrawVisualMeshes
        {
            get
            {
        