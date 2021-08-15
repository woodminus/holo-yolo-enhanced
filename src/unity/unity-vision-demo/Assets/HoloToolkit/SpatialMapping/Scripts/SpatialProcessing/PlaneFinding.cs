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

            public MeshData(MeshFilter meshFilter)
            {
                Transform = meshFilter.transform.localToWorldMatrix;
                Verts = meshFilter.sharedMesh.vertices;
                Normals = meshFilter.sharedMesh.normals;
                Indices = meshFilter.sharedMesh.triangles;
            }
        }

        /// <summary>
        /// Finds small planar patches that are contained within individual meshes.  The output of this
        /// API can then be passed to MergeSubPlanes() in order to find larger planar surfaces that
        /// potentially span across multiple meshes.
        /// </summary>
        /// <param name="meshes">
        /// List of meshes to run the plane finding algorithm on.
        /// </param>
        /// <param name="snapToGravityThreshold">
        /// Planes whose normal vectors are within this threshold (in degrees) from vertical/horizontal
        /// will be snapped to be perfectly gravity aligned.  When set to something other than zero, the
        /// bounding boxes for each plane will be gravity aligned as well, rather than rotated for an
        /// optimally tight fit. Pass 0.0 for this parameter to completely disable the gravity alignment
        /// logic.
        /// </param>
        public static BoundedPlane[] FindSubPlanes(List<MeshData> meshes, float snapToGravityThreshold = 0.0f)
        {
            StartPlaneFinding();

            try
            {
                int planeCount;
                IntPtr planesPtr;
                IntPtr pinnedMeshData = PinMeshDataForMarshalling(meshes);
                DLLImports.FindSubPlanes(meshes.Count, pinnedMeshData, snapToGravityThreshold, out planeCount, out planesPtr);
                return MarshalBoundedPlanesFromIntPtr(planesPtr, planeCount);
            }
            finally
            {
                FinishPlaneFinding();
            }
        }

        /// <summary>
        /// Takes the subplanes returned by one or more previous calls to FindSubPlanes() and merges
        /// them together into larger planes that can potentially span across multiple meshes.
        /// Overlapping subplanes that have similar plane equations will be merged together to form
        /// larger planes.
        /// </summary>
        /// <param name="subPlanes">
        /// The output from one or more previous calls to FindSubPlanes().
        /// </param>
        /// <param name="snapToGravityThreshold">
        /// Planes whose normal vectors are within this threshold (in degrees) from vertical/horizontal
        /// will be snapped to be perfectly gravity aligned.  When set to something other than zero, the
        /// bounding boxes for each plane will be gravity aligned as well, rather than rotated for an
        /// optimally tight fit. Pass 0.0 for this parameter to completely disable the gravity alignment
        /// logic.
        /// </param>
        /// <param name="minArea">
        /// While merging subplanes together, any candidate merged plane whose constituent mesh
        /// triangles have a total area less than this threshold are ignored.
        /// </param>
        public static BoundedPlane[] MergeSubPlanes(BoundedPlane[] subPlanes, float snapToGravityThreshold = 0.0f, float minArea = 0.0f)
        {
            StartPlaneFinding();

            try
            {
                int planeCount;
  