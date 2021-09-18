
﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using UnityEngine;

namespace HoloToolkit.Unity
{
    /// <summary>
    /// All possible plane types that a SurfacePlane can be.
    /// </summary>
    [Flags]
    public enum PlaneTypes
    {
        Wall = 0x1,
        Floor = 0x2,
        Ceiling = 0x4,
        Table = 0x8,
        Unknown = 0x10
    }

    /// <summary>
    /// The SurfacePlane class is used by SurfaceMeshesToPlanes to create different types of planes (walls, floors, tables, etc.) 
    /// based on the Spatial Mapping data returned by the SpatialMappingManager's source.
    /// This script should be a component on the SufacePlane prefab, which is used by SurfaceMeshesToPlanes.
    /// </summary>
    public class SurfacePlane : MonoBehaviour
    {
        [Tooltip("Thickness to make each plane.")]
        [Range(0.0f, 1.0f)]
        public float PlaneThickness = 0.01f;

        [Tooltip("Threshold for acceptable normals (the closer to 1, the stricter the standard). Used when determining plane type.")]
        [Range(0.0f, 1.0f)]
        public float UpNormalThreshold = 0.9f;

        [Tooltip("Buffer to use when determining if a horizontal plane near the floor should be considered part of the floor.")]
        [Range(0.0f, 1.0f)]
        public float FloorBuffer = 0.1f;

        [Tooltip("Buffer to use when determining if a horizontal plane near the ceiling should be considered part of the ceiling.")]
        [Range(0.0f, 1.0f)]
        public float CeilingBuffer = 0.1f;

        [Tooltip("Material to use when rendering Wall planes.")]
        public Material WallMaterial;

        [Tooltip("Material to use when rendering floor planes.")]
        public Material FloorMaterial;

        [Tooltip("Material to use when rendering ceiling planes.")]
        public Material CeilingMaterial;

        [Tooltip("Material to use when rendering table planes.")]
        public Material TableMaterial;

        [Tooltip("Material to use when rendering planes of the unknown type.")]
        public Material UnknownMaterial;

        [Tooltip("Type of plane that the object has been classified as.")]
        public PlaneTypes PlaneType = PlaneTypes.Unknown;

        /// <summary>
        /// The BoundedPlane associated with the SurfacePlane object.
        /// </summary>
        private BoundedPlane plane = new BoundedPlane();

        /// <summary>
        /// Gets or Sets the BoundedPlane, which determines the orientation/size/position of the gameObject.
        /// </summary>
        public BoundedPlane Plane
        {
            get
            {
                return plane;