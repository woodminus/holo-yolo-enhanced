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
            public Vector3 Norma