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
        public Or