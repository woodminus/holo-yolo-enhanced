// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using UnityEngine;

namespace HoloToolkit.Unity
{
    /// <summary>
    /// 1. Decides when to show the cursor.
    /// 2. Positions the cursor at the gazed location.
    /// 3. Rotates the cursor to match hologram normals.
    /// </summary>
    public class BasicCursor : MonoBehaviour
    {
        public struct RaycastResult
        {
            public bool Hit;
            public Vector3 Position;
            public Vector3 Normal;
        }

        [Tooltip("Distance, in meters, to offset the cursor from the collision point.")]
        public float DistanceFromCollision = 0.01f;

        private Quaternion cursorDefaultRotation;

        private MeshRenderer meshRenderer;

        private GazeManager gazeManager;

        protected virtual void Awake()
        {
            meshRenderer = gameObject.GetComponent<MeshRenderer>();

            if (meshRenderer == null)
            {
                Debug.LogError("This script requires that your cursor asset has a MeshRenderer component on it.");
                return;
            }

            // Hide the Cursor to begin with.
            meshRenderer.enabled = false;

            // Cache the cursor default rotation so the cursor can be rotated with respect to the original orientation.
            c