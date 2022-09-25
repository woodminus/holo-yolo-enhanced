
﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using System;
using System.Collections.Generic;

namespace HoloToolkit.Unity
{
    /// <summary>
    /// Encapsulates the primary dll functions, including marshalling helper functions.
    /// The dll functions are organized into four parts - in this behavior, 
    /// SpatialUnderstandingDllTopology, SpatialUnderstandingDllShapes, and
    /// SpatialUnderstandingDllObjectPlacement. The scan flow, raycast, and alignment
    /// functions are included in this class.
    /// </summary>
    public class SpatialUnderstandingDll
    {
        /// <summary>
        /// Representation of the mesh data to be passed to the understanding dll.
        /// Used by SpatialUnderstandingSourceMesh to store local copies of the mesh data.
        /// </summary>
        public struct MeshData
        {
            public int MeshID;
            public int LastUpdateID;
            public Matrix4x4 Transform;
            public Vector3[] Verts;
            public Vector3[] Normals;
            public Int32[] Indices;

            public MeshData(MeshFilter meshFilter)
            {
                MeshID = 0;
                LastUpdateID = 0;

                Transform = meshFilter.transform.localToWorldMatrix;
                Verts = meshFilter.sharedMesh.vertices;
                Normals = meshFilter.sharedMesh.normals;
                Indices = meshFilter.sharedMesh.triangles;
            }
            public void CopyFrom(MeshFilter meshFilter, int meshID = 0, int lastUpdateID = 0)
            {
                MeshID = meshID;
                LastUpdateID = lastUpdateID;

                if (meshFilter != null)
                {
                    Transform = meshFilter.transform.localToWorldMatrix;

                    // Note that we are assuming that Unity's getters for vertices/normals/triangles make
                    // copies of the array.  As of unity 5.4 this assumption is correct.
                    Verts = meshFilter.sharedMesh.vertices;
                    Normals = meshFilter.sharedMesh.normals;
                    Indices = meshFilter.sharedMesh.triangles;
                }
            }
        }

        // Privates
        private Imports.MeshData[] reusedMeshesForMarshalling = null;
        private List<GCHandle> reusedPinnedMemoryHandles = new List<GCHandle>();

        private Imports.RaycastResult reusedRaycastResult = new Imports.RaycastResult();
        private IntPtr reusedRaycastResultPtr;

        private Imports.PlayspaceStats reusedPlayspaceStats = new Imports.PlayspaceStats();
        private IntPtr reusedPlayspaceStatsPtr;

        private Imports.PlayspaceAlignment reusedPlayspaceAlignment = new Imports.PlayspaceAlignment();
        private IntPtr reusedPlayspaceAlignmentPtr;

        private SpatialUnderstandingDllObjectPlacement.ObjectPlacementResult reusedObjectPlacementResult = new SpatialUnderstandingDllObjectPlacement.ObjectPlacementResult();
        private IntPtr reusedObjectPlacementResultPtr;

        /// <summary>
        /// Pins the specified object so that the backing memory can not be relocated, adds the pinned
        /// memory handle to the tracking list, and then returns that address of the pinned memory so
        /// that it can be passed into the DLL to be access directly from native code.
        /// </summary>
        public IntPtr PinObject(System.Object obj)
        {
            GCHandle h = GCHandle.Alloc(obj, GCHandleType.Pinned);
            reusedPinnedMemoryHandles.Add(h);
            return h.AddrOfPinnedObject();
        }

        /// <summary>
        /// Pins the string, converting to the format expected by the dll. See PinObject for
        /// additional details.
        /// </summary>
        public IntPtr PinString(string str)
        {
            byte[] obj = System.Text.Encoding.ASCII.GetBytes(str);
            GCHandle h = GCHandle.Alloc(obj, GCHandleType.Pinned);
            reusedPinnedMemoryHandles.Add(h);
            return h.AddrOfPinnedObject();
        }

        /// <summary>
        /// Unpins all of the memory previously pinned by calls to PinObject().
        /// </summary>
        public void UnpinAllObjects()
        {
            for (int i = 0; i < reusedPinnedMemoryHandles.Count; ++i)
            {
                reusedPinnedMemoryHandles[i].Free();
            }
            reusedPinnedMemoryHandles.Clear();
        }

        /// <summary>
        /// Copies the supplied mesh data into the reusedMeshesForMarhsalling array. All managed arrays
        /// are pinned so that the marshalling only needs to pass a pointer and the native code can
        /// reference the memory in place without needing the marshaller to create a complete copy of
        /// the data.
        /// </summary>
        public IntPtr PinMeshDataForMarshalling(List<MeshData> meshes)
        {
            // if we have a big enough array reuse it, otherwise create new
            if (reusedMeshesForMarshalling == null || reusedMeshesForMarshalling.Length < meshes.Count)
            {
                reusedMeshesForMarshalling = new Imports.MeshData[meshes.Count];
            }

            for (int i = 0; i < meshes.Count; ++i)
            {
                IntPtr pinnedVerts = (meshes[i].Verts != null) && (meshes[i].Verts.Length > 0) ? PinObject(meshes[i].Verts) : IntPtr.Zero;
                IntPtr pinnedNormals = (meshes[i].Verts != null) && (meshes[i].Verts.Length > 0) ? PinObject(meshes[i].Normals) : IntPtr.Zero;
                IntPtr pinnedIndices = (meshes[i].Indices != null) && (meshes[i].Indices.Length > 0) ? PinObject(meshes[i].Indices) : IntPtr.Zero;
                reusedMeshesForMarshalling[i] = new Imports.MeshData()
                {
                    meshID = meshes[i].MeshID,
                    lastUpdateID = meshes[i].LastUpdateID,
                    transform = meshes[i].Transform,
                    vertCount = (meshes[i].Verts != null) ? meshes[i].Verts.Length : 0,
                    indexCount = (meshes[i].Indices != null) ? meshes[i].Indices.Length : 0,
                    verts = pinnedVerts,
                    normals = pinnedNormals,
                    indices = pinnedIndices,
                };
            }

            return PinObject(reusedMeshesForMarshalling);
        }

        /// <summary>
        /// Reusable raycast result object pointer. Can be used for inline raycast calls.
        /// </summary>
        /// <returns>Raycast result pointer</returns>
        public IntPtr GetStaticRaycastResultPtr()
        {
            if (reusedRaycastResultPtr == IntPtr.Zero)
            {
                GCHandle h = GCHandle.Alloc(reusedRaycastResult, GCHandleType.Pinned);
                reusedRaycastResultPtr = h.AddrOfPinnedObject();
            }
            return reusedRaycastResultPtr;
        }
        /// <summary>
        /// Resuable raycast result object. Can be used for inline raycast calls.
        /// </summary>
        /// <returns>Raycast result structure</returns>
        public Imports.RaycastResult GetStaticRaycastResult()
        {
            return reusedRaycastResult;
        }

        /// <summary>
        /// Resuable playspace statistics pointer. Can be used for inline playspace statistics calls.
        /// </summary>
        /// <returns>playspace statistics pointer</returns>
        public IntPtr GetStaticPlayspaceStatsPtr()
        {
            if (reusedPlayspaceStatsPtr == IntPtr.Zero)
            {
                GCHandle h = GCHandle.Alloc(reusedPlayspaceStats, GCHandleType.Pinned);
                reusedPlayspaceStatsPtr = h.AddrOfPinnedObject();
            }
            return reusedPlayspaceStatsPtr;
        }
        /// <summary>
        /// Resuable playspace statistics. Can be used for inline playspace statistics calls.
        /// </summary>