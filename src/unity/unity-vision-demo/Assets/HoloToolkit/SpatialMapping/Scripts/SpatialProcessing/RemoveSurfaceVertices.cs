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
        /// Queue of bounding objects to remove surface vertices from.
        /// Bounding objects are queued so that RemoveSurfaceVerticesWithinBounds can be called even when the previous task has not finished.
        /// </summary>
        private Queue<Bounds> boundingObjectsQueue;

#if UNITY_EDITOR
        /// <summary>
        /// How much time (in sec), while running in the Unity Editor, to allow RemoveSurfaceVertices to consume before returning control to the main program.
        /// </summary>
        private static readonly float FrameTime = .016f;
#else
        /// <summary>
        /// How much time (in sec) to allow RemoveSurfaceVertices to consume before returning control to the main program.
        /// </summary>
        private static readonly float FrameTime = .008f;
#endif

        // GameObject initialization.
        private void Start()
        {
            boundingObjectsQueue = new Queue<Bounds>();
            removingVerts = false;
        }

        /// <summary>
        /// Removes portions of the surface mesh that exist within the bounds of the boundingObjects.
        /// </summary>
        /// <param name="boundingObjects">Collection of GameObjects that define the bounds where spatial mesh vertices should be removed.</param>
        public void RemoveSurfaceVerticesWithinBounds(IEnumerable<GameObject> boundingObjects)
        {
            if (boundingObjects == null)
            {
                return;
            }

            if (!removingVerts)
            {
                removingVerts = true;
                AddBoundingObjectsToQueue(boundingObjects);

                // We use Coroutine to split the work across multiple frames and avoid impacting the frame rate too much.
                StartCoroutine(RemoveSurfaceVerticesWithinBoundsRoutine());
            }
            else
            {
                // Add new boundingObjects to end of queue.
                AddBoundingObjectsToQueue(boundingObjects);
            }
        }

        /// <summary>
        /// Adds new bounding objects to the end of the Queue.
        /// </summary>
        /// <param name="boundingObjects">Collection of GameObjects which define the bounds where spatial mesh vertices should be removed.</param>
        private void AddBoundingObjectsToQueue(IEnumerable<GameObject> boundingObjects)
        {
            foreach (GameObject item in boundingObjects)
            {
                Bounds bounds = new Bounds();

                Collider boundingCollider = item