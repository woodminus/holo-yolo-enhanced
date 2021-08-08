// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace HoloToolkit.Unity
{
    [StructLayout(LayoutKind.Sequential)]
    public struct OrientedBoundingBox
    {
        public Vector3 Center;
        public Vector3 Extents;
        public Quaternion Rotation;
    };

    [StructLayout(LayoutKind.Sequential)]
    public struct BoundedPlane
    {
        public Plane Plane;
        public OrientedBoundingBox Bounds;
        public float Area;

        /// <summary>
        /// Builds the bounded plane to match the obb defined by xform
        /// </summary>
        public BoundedPlane(Transform xform)
        {
            Plane = new Plane(xform.forward, xform.position);
            Bounds = new OrientedBoundingBox()
            {
                Center = xform.position,
                Extents = xform.localScale / 2,
                Rotation = xform.rotation
            };
            Area = Bounds.Extents.x * Bounds.Extents.y;
        }
    };

    public class PlaneFinding
    {
        #region Public APIs

        /// <summary>
        /// PlaneFinding is an expensive task that should not be run from Unity's main thread as it
        /// will stall the thread and cause a frame rate dip.  Instead, the PlaneFinding APIs should be
        /// exclusively called from background threads.  Unfortunately, Unity's built-in data types
        /// (such as MeshFilter) are not thread safe and cannot be accessed from background threads.
        /// The MeshData struct exists to work-around this limitation.  When you want to find planes
        /// in a collection of MeshFilter objects, start by constructing a list of MeshData structs
        /// from those MeshFilters. You can then take the resulting list of MeshData structs, and
        /// safely pass it to the FindPlanes() API from a background thread.
        /// </summary>
        public struct MeshData
        {
            public Matrix4x4 Transform;
            public Vector3[] Verts;
            public Vector3[] Normals;
            public Int32[] Indices;

            public MeshData(MeshFilter