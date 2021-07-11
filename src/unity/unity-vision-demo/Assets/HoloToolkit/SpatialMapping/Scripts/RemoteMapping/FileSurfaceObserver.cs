// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Collections.Generic;
using UnityEngine;

namespace HoloToolkit.Unity
{
    public class FileSurfaceObserver : SpatialMappingSource
    {
        [Tooltip("The file name to use when saving and loading meshes.")]
        public string MeshFileName = "roombackup";

        [Tooltip("Key to press in editor to load a spatial mapping mesh from a .room file.")]
        public KeyCode LoadFileKey = KeyCode.L;

        [Tooltip("Key to press in editor to save a spatial mapping mesh to file.")]
        public KeyCode SaveFileKey = KeyCode.S;

        /// <summary>
        /// Loads the SpatialMapping mesh from the specified file.
        /// </summary>
        /// <param name="fileName">The name, without path or extension, of the file to load.</param>
        public void Load(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                Debug.Log("No mesh file specified.");
                return;
            }

            Cleanup();

            List<Mesh> storedMeshes = new List<Mesh>();
            try
            {
                storedMeshes.AddRange(MeshSaver.Load(fileName));

                foreach (Mesh mesh in storedMeshes)
                {
                    GameObject surface = AddSurfaceObject(mesh, "storedmesh-" + SurfaceObjects.Count, transform);
                    Renderer renderer = surface.GetComponent<MeshRenderer>();

                    if (SpatialMappingManager.Instance.DrawVisualMeshes == false)
                    {
                        renderer.enabled = false;
                    }

                    if (SpatialMappingManager.Instance.CastShadows == false)
                    {
                        renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                    }

                    // Reset the sur