
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
    /// Encapsulates the object placement queries of the understanding dll.
    /// These queries will not be valid until after scanning is finalized.
    /// </summary>
    public static class SpatialUnderstandingDllObjectPlacement
    {
        /// <summary>
        /// Defines an object placement query. A query consists of
        /// a type a name, type, set of rules, and set of constraints.
        /// 
        /// Rules may not be violated by the returned query. Possible 
        /// locations that satisfy the type and rules are selected
        /// by optimizing within the constraint list.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct ObjectPlacementDefinition
        {
            /// <summary>
            /// Type of object placement. Each type has a custom set
            /// of parameter.
            /// </summary>
            public enum PlacementType
            {
                Place_OnFloor,
                Place_OnWall,
                Place_OnCeiling,
                Place_OnShape,
                Place_OnEdge,
                Place_OnFloorAndCeiling,
                Place_RandomInAir,
                Place_InMidAir,
                Place_UnderPlatformEdge,
            };

            /// <summary>
            /// Type of wall.
            /// External walls bound the playspace. Virtual walls are created
            /// at the edge of the playspace when an external wall does not
            /// exist.
            /// </summary>
            [FlagsAttribute]
            public enum WallTypeFlags
            {
                None = 0,
                Normal = (1 << 0),
                External = (1 << 1),
                Virtual = (1 << 2),
                ExternalVirtual = (1 << 3),
            };

            /// <summary>
            /// Constructs an object placement query definition requiring the object to
            /// be placed on the floor.
            /// </summary>
            /// <param name="halfDims">Required half size of the requested bounding volume</param>
            /// <returns>Constructed object placement definition</returns>
            public static ObjectPlacementDefinition Create_OnFloor(Vector3 halfDims)
            {
                ObjectPlacementDefinition placement = new ObjectPlacementDefinition();
                placement.Type = PlacementType.Place_OnFloor;
                placement.HalfDims = halfDims;
                return placement;
            }

            /// <summary>
            /// Constructs an object placement query definition requiring the object to
            /// be placed on a wall.
            /// </summary>
            /// <param name="halfDims">Required half size of the requested bounding volume</param>
            /// <param name="heightMin">Minimum height of the requested volume above the floor</param>
            /// <param name="heightMax">Maximum height of the requested volume above the floor</param>
            /// <param name="wallTypes">Bit mask of possible walls to consider, defined by WallTypeFlags</param>
            /// <param name="marginLeft">Required empty wall space to the left of the volume, as defined by facing the wall</param>
            /// <param name="marginRight">Required empty wall space to the right of the volume, as defined by facing the wall</param>
            /// <returns>Constructed object placement definition</returns>
            public static ObjectPlacementDefinition Create_OnWall(
                Vector3 halfDims,
                float heightMin,
                float heightMax,
                WallTypeFlags wallTypes = WallTypeFlags.External | WallTypeFlags.Normal,
                float marginLeft = 0.0f,
                float marginRight = 0.0f)
            {
                ObjectPlacementDefinition placement = new ObjectPlacementDefinition();
                placement.Type = PlacementType.Place_OnWall;
                placement.HalfDims = halfDims;
                placement.PlacementParam_Float_0 = heightMin;
                placement.PlacementParam_Float_1 = heightMax;
                placement.PlacementParam_Float_2 = marginLeft;
                placement.PlacementParam_Float_3 = marginRight;
                placement.WallFlags = (int)wallTypes;
                return placement;
            }

            /// <summary>
            /// Constructs an object placement query definition requiring the object to
            /// be place on the ceiling.
            /// </summary>
            /// <param name="halfDims">Required half size of the requested bounding volume</param>
            /// <returns>Constructed object placement definition</returns>
            public static ObjectPlacementDefinition Create_OnCeiling(Vector3 halfDims)
            {
                ObjectPlacementDefinition placement = new ObjectPlacementDefinition();
                placement.Type = PlacementType.Place_OnCeiling;
                placement.HalfDims = halfDims;
                return placement;
            }

            /// <summary>
            /// Constructs an object placement query definition requiring the object to
            /// be placed on top of another object placed object.
            /// </summary>
            /// <param name="halfDims">Required half size of the requested bounding volume</param>
            /// <param name="shapeName">Name of the placed object</param>
            /// <param name="componentIndex">Index of the component within shapeName</param>
            /// <returns>Constructed object placement definition</returns>
            public static ObjectPlacementDefinition Create_OnShape(Vector3 halfDims, string shapeName, int componentIndex)
            {
                ObjectPlacementDefinition placement = new ObjectPlacementDefinition();
                placement.Type = PlacementType.Place_OnShape;
                placement.HalfDims = halfDims;
                placement.PlacementParam_Str_0 = SpatialUnderstanding.Instance.UnderstandingDLL.PinString(shapeName);
                placement.PlacementParam_Int_0 = componentIndex;
                return placement;
            }

            /// <summary>
            /// Constructs an object placement query definition requiring the object to
            /// be placed on the edge of a platform.
            /// </summary>
            /// <param name="halfDims">Required half size of the requested bounding volume</param>
            /// <param name="halfDimsBottom">Half size of the bottom part of the placement volume</param>
            /// <returns>Constructed object placement definition</returns>
            public static ObjectPlacementDefinition Create_OnEdge(Vector3 halfDims, Vector3 halfDimsBottom)
            {
                ObjectPlacementDefinition placement = new ObjectPlacementDefinition();
                placement.Type = PlacementType.Place_OnEdge;
                placement.HalfDims = halfDims;
                placement.PlacementParam_Float_0 = halfDimsBottom.x;
                placement.PlacementParam_Float_1 = halfDimsBottom.y;
                placement.PlacementParam_Float_2 = halfDimsBottom.z;
                return placement;
            }

            /// <summary>
            /// Constructs an object placement query definition requiring the object to
            /// be have space on the floor and ceiling within the same vertical space.
            /// </summary>
            /// <param name="halfDims">Required half size of the requested bounding volume</param>
            /// <param name="halfDimsBottom">Half size of the bottom part of the placement volume</param>
            /// <returns>Constructed object placement definition</returns>
            public static ObjectPlacementDefinition Create_OnFloorAndCeiling(Vector3 halfDims, Vector3 halfDimsBottom)
            {
                ObjectPlacementDefinition placement = new ObjectPlacementDefinition();
                placement.Type = PlacementType.Place_OnFloorAndCeiling;
                placement.HalfDims = halfDims;
                placement.PlacementParam_Float_0 = halfDimsBottom.x;
                placement.PlacementParam_Float_1 = halfDimsBottom.y;
                placement.PlacementParam_Float_2 = halfDimsBottom.z;
                return placement;
            }
