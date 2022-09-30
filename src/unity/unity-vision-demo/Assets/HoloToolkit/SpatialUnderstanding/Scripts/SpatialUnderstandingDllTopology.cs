// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using System;
using System.Collections.Generic;

namespace HoloToolkit.Unity
{
    /// <summary>
    /// Encapsulates the topology queries of the understanding dll.
    /// These queries will not be valid until after scanning is finalized.
    /// </summary>
    public static class SpatialUnderstandingDllTopology
    {
        /// <summary>
        /// Result of a topology query. Typically results return an array 
        /// of these structures.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct TopologyResult
        {
            pu