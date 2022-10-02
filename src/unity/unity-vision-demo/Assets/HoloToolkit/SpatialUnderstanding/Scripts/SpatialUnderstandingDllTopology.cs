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
            public Vector3 position;
            public Vector3 normal;
            public float width;
            public float length;
        };

        // Functions
        /// <summary>
        /// Finds spaces on walls meeting the criteria specified by the parameters.
        /// </summary>
        /// <param name="minHeightOfWallSpace">Minimum height of space to be found by the query</param>
        /// <param name="minWidthOfWallSpace">Minimum width of space to be found by the query</param>
        /// <param name="minHeightAboveFloor">Minimum distance above the floor for the bottom edge of the space</param>
        /// <param name="minFacingClearance">Minimum amount of space in front of the space</param>
        /// <param name="locationCount">Number of location results supplied by the user in locationData</param>
        /// <param name="locationData">Location result array of TopologyResult to be filled with the spaces found by the query</param>
        /// <returns>Number of spaces found by the query. This value is limited by the number of results supplied by the caller (locationCount)</returns>
#if UNITY_METRO && !UNITY_EDITOR
        // Queries (topology)
        [DllImport("SpatialUnderstanding")]
        public static extern int QueryTopology_FindPositionsOnWalls(
            [In] float minHeightOfWallSpace,
            [In] float minWidthOfWallSpace,
            [In] float minHeightAboveFloor,
            [In] float minFacingClearance,
            [In] int locationCount,         // Pass in the space allocated in locationData
            [Out] IntPtr locationData);     // TopologyResult
#else
        public static int QueryTopology_FindPositionsOnWalls(
            [In] float minHeightOfWallSpace,
            [In] float minWidthOfWallSpace,
            [In] float minHeightAboveFloor,
            [In] float minFacingClearance,
            [In] int locationCount,         // Pass in the space allocated in locationData
            [Out] IntPtr locationData)
        {
            return 0;
        }
#endif

        /// <summary>
        /// Finds only large spaces on walls meeting the criteria specified by the parameters.
        /// </summary>
        /// <param name="minHeightOfWallSpace">Minimum height of space to be found by the query</param>
        /// <param name="minWidthOfWallSpace">Minimum width of space to be found by the query</param>
        /// <param name="minHeightAboveFloor">Minimum distance above the floor for the bottom edge of the space</param>
        /// <param name="minFacingClearance">Minimum amount of space in front of the space</param>
        /// <param name="locationCount">Number of location results supplied by the user in locationData</param>
        /// <param name="locationData">Location result array of TopologyResult to be filled with the spaces found by the query</param>
        /// <returns>Number of spaces found by the query. This value is limited by the number of results supplied by the caller (locationCount)</returns>
#if UNITY_METRO && !UNITY_EDITOR
        [DllImport("SpatialUnderstanding")]
        public static extern int QueryTopology_FindLargePositionsOnWalls(
            [In] float minHeightOfWallSpace,
            [In] float minWidthOfWallSpace,
            [In] float minHeightAboveFloor,
            [In] float minFacingClearance,
